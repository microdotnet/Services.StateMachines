namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDefinitions;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;

public sealed class MachineDefinitionDto
{
    [BsonId]
    public ObjectId BsonId { get; set; } = default;

    public required Guid MachineDefinitionId { get; set; }

    public required string Code { get; set; }

    public required short Version { get; set; }

    public required string Status { get; set; }

    public required Node[] Nodes { get; set; }

    public required Transition[] Transitions { get; set; }

    public sealed class Node
    {
        public required string Name { get; set; }
    }

    public sealed class Transition
    {
        public required string Source { get; set; }

        public required string Target { get; set; }

        public required string Trigger { get; set; }
    }
}
