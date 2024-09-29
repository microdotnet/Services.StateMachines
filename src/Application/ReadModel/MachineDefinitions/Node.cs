namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    public sealed class Node
    {
        public Node(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
