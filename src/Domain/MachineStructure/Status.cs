namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class Status
    {
        private static readonly Dictionary<string, Status> statuses = new Dictionary<string, Status>();

        private static readonly SemaphoreSlim createSemaphore = new SemaphoreSlim(1, 1);

        private Status(
            string code,
            string description)
        {
            this.Code = code;
            this.Description = description;
        }

        public static Status InDesign = Create(nameof(InDesign));

        public static Status Completed = Create(nameof(Completed));

        public static Status Disabled = Create(nameof(Disabled));

        public string Code { get; }

        public string Description { get; }

        public static Status Parse(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException(
                    StatusResources.Parse_CodeEmpty,
                    nameof(code));
            }

            if (!statuses.ContainsKey(code))
            {
                throw new InvalidOperationException(
                    StatusResources.Parse_CodeNotFound);
            }

            return statuses[code];
        }

        private static Status Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(
                    StatusResources.Create_CodeIsEmpty,
                    nameof(code));
            }

            var description = StatusResources.ResourceManager.GetString($"Description_{code}");
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException(
                    StatusResources.Create_CodeInvalid,
                    nameof(code));
            }

            createSemaphore.Wait();
            try
            {
                var result = new Status(code, description);
                statuses.Add(code, result);
                return result;
            }
            finally
            {
                createSemaphore.Release();
            }
        }
    }
}
