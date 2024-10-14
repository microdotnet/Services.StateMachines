namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Diagnostics.Metrics;

public sealed class Metrics
{
    public const string MeterName = "StateMachines.Api.MachineDefinitions";

    private readonly Histogram<double> machineCreationDuration;

    private readonly Histogram<double> machineRetrievalDuration;

    public Metrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        this.machineCreationDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.create.duration",
            "ms");

        this.machineRetrievalDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.get.duration",
            "ms");
    }

    public TrackedRequestDuration MeasureMachineCreationDuration()
    {
        return new TrackedRequestDuration(this.machineCreationDuration);
    }

    public TrackedRequestDuration MeasureMachineRetrievalDuration()
    {
        return new TrackedRequestDuration(this.machineRetrievalDuration);
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
