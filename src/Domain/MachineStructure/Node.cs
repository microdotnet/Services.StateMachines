namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure
{
    using System;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    [DebuggerDisplay("Node '{Name}'")]
    public sealed class Node
    {
        [JsonConstructor]
        private Node(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public static Node Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    NodeResources.Create_NameIsEmpty,
                    nameof(name));
            }

            return new Node(name);
        }

        public static explicit operator Node(string name) => Node.Create(name);

        public static explicit operator string(Node node) => node.Name;
    }
}
