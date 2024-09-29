namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDetails;

using System.Collections.ObjectModel;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public sealed class MachineDetailsDto
{
    [BsonId]
    public ObjectId BsonId { get; set; } = default;

    public required Guid MachineDetailsId { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required short[] Versions { get; set; }
}
