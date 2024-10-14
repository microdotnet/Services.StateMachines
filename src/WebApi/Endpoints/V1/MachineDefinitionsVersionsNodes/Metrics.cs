namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using System.Diagnostics.Metrics;

public sealed class Metrics
{
    public const string MeterName = "StateMachines.Api.MachineDefinitionsVersionsNodes";

    private readonly Histogram<double> nodesAddingDuration;

    public Metrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        this.nodesAddingDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.addnodes.duration",
            "ms");
    }

    public TrackedRequestDuration MeasureNodesAddingDuration()
    {
        return new TrackedRequestDuration(this.nodesAddingDuration);
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
