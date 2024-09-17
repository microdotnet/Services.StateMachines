namespace MicroDotNet.Services.StateMachines.Domain.MachineDetails
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events;

    public sealed class MachineDetailsAggregateRoot : AggregateRootBase
    {
        private readonly Collection<short> versions = new Collection<short>();

        public MachineDetailsAggregateRoot()
        {
            this.Code = string.Empty;
            this.Name = string.Empty;
            this.Description = string.Empty;
        }

        private MachineDetailsAggregateRoot(
            Guid id,
            string code,
            string name,
            string description)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.Description = description;
            var @event = new MachineDetailCreated(id, code, name, description);
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public IReadOnlyCollection<short> Versions => new ReadOnlyCollection<short>(this.versions);

        public override string PublicIdentifier => CreatePublicIdentifier(this.Code);

        public static string CreatePublicIdentifier(string code)
        {
            return code;
        }

        public static MachineDetailsAggregateRoot Create(
            Guid id,
            string code,
            string name,
            string description)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(
                    MachineDetailsAggregateRootResources.Create_IdIsEmpty,
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(
                    MachineDetailsAggregateRootResources.Create_CodeIsEmpty,
                    nameof(code));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    MachineDetailsAggregateRootResources.Create_NameIsEmpty,
                    nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(
                    MachineDetailsAggregateRootResources.Create_DescriptionIsEmpty,
                    nameof(description));
            }

            return new MachineDetailsAggregateRoot(id, code, name, description);
        }

        public short AddNewVersion()
        {
            short currentVersionNumber = 0;
            if (this.versions.Count != 0)
            {
                currentVersionNumber = this.Versions.Max();
            }

            currentVersionNumber++;
            var @event = new VersionAdded(currentVersionNumber);
            this.Enqueue(@event);
            this.Apply(@event);
            return currentVersionNumber;
        }

        public override void When(object @event)
        {
            switch (@event)
            {
                case MachineDetailCreated created:
                    this.Apply(created);
                    break;
                case VersionAdded versionAdded:
                    this.Apply(versionAdded);
                    break;
            }

            base.When(@event);
        }

        private void Apply(MachineDetailCreated @event)
        {
            this.Id = @event.Id;
            this.Code = @event.Code;
            this.Name = @event.Name;
            this.Description = @event.Description;
        }

        private void Apply(VersionAdded @event)
        {
            this.versions.Add(@event.VersionNumber);
        }
    }
}
