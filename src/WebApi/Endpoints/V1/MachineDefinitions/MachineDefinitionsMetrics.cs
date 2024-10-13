namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Diagnostics.Metrics;

public sealed class MachineDefinitionsMetrics
{
    public const string MeterName = "MicroDotNet.StateMachines.Api.MachineDefintitions";

    private readonly Counter<int> machineCreationCounter;

    private readonly Histogram<double> machineCreationDuration;

    public MachineDefinitionsMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        this.machineCreationCounter = meter.CreateCounter<int>(
            $"{MeterName.ToLower()}.create.count");

        this.machineCreationDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.create.duration",
            "ms");
    }

    public void IncreaseMachineCreationCount()
    {
        this.machineCreationCounter.Add(1);
    }

    public TrackedRequestDuration MeasureMachineCreationDuration()
    {
        return new TrackedRequestDuration(this.machineCreationDuration);
    }

    public class TrackedRequestDuration : IDisposable
    {
        private readonly long requestStartTime = TimeProvider.System.GetTimestamp();

        private readonly Histogram<double> histogram;

        public TrackedRequestDuration(Histogram<double> histogram)
        {
            this.histogram = histogram;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var elapsed = TimeProvider.System.GetElapsedTime(this.requestStartTime);
                this.histogram.Record(elapsed.TotalMilliseconds);
            }            }
        }
}
