namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using System.Diagnostics.Metrics;

public sealed class Metrics
{
    public const string MeterName = "StateMachines.Api.MachineDefinitionsVersions";

    private readonly Histogram<double> versionCreationDuration;

    private readonly Histogram<double> versionRetrievalDuration;

    private readonly Histogram<double> versionAcceptationDuration;

    public Metrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        this.versionCreationDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.create.duration",
            "ms");

        this.versionRetrievalDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.get.duration",
            "ms");

        this.versionAcceptationDuration = meter.CreateHistogram<double>(
            $"{MeterName.ToLower()}.accept.duration",
            "ms");
    }

    public TrackedRequestDuration MeasureVersionCreationDuration()
    {
        return new TrackedRequestDuration(this.versionCreationDuration);
    }

    public TrackedRequestDuration MeasureVersionRetrievalDuration()
    {
        return new TrackedRequestDuration(this.versionRetrievalDuration);
    }

    public TrackedRequestDuration MeasureVersionAcceptationDuration()
    {
        return new TrackedRequestDuration(this.versionAcceptationDuration);
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
