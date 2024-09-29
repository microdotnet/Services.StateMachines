namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using System.Threading;
using System.Threading.Tasks;

using MicroDotNet.Services.StateMachines.Application.ReadModel;
using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails;
using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDetails;

using MongoDB.Driver;

using OneOf;
using OneOf.Types;

public sealed class MongoMachineDetailsRespository : MongoCollectionRepositoryBase<MachineDetailsDto, string>, IMachineDetailsRepository
{
    private readonly ICollectionProvider collectionProvider;

    public MongoMachineDetailsRespository(ICollectionProvider collectionProvider)
    {
        this.collectionProvider = collectionProvider ?? throw new ArgumentNullException(nameof(collectionProvider));
    }

    public async Task<OneOf<CreateMachineResponse, Error>> CreateMachineAsync(CreateMachineRequest request, CancellationToken cancellationToken)
    {
        var payload = new MachineDetailsDto()
        {
            MachineDetailsId = request.Machine.Id,
            Code = request.Machine.Code,
            Name = request.Machine.Name,
            Description = request.Machine.Description,
            Versions = [.. request.Machine.Versions],
        };

        await this.CreateItemAsync(payload, cancellationToken)
            .ConfigureAwait(false);
        return new CreateMachineResponse(CreateMachineResponse.Result.Created);
    }

    public async Task<OneOf<GetMachineResponse, Error>> GetMachineAsync(GetMachineRequest request, CancellationToken cancellationToken)
    {
        var item = await this.GetItemAsync(request.Code, cancellationToken)
            .ConfigureAwait(false);
        if (item is null)
        {
            return new Error();
        }

        var result = new Machine(
            item.MachineDetailsId,
            item.Code,
            item.Name,
            item.Description,
            item.Versions);
        return new GetMachineResponse(result);
    }

    public async Task<OneOf<UpdateMachineResponse, Error>> UpdateMachineAsync(UpdateMachineRequest request, CancellationToken cancellationToken)
    {
        var item = await this.GetItemAsync(request.Machine.Code, cancellationToken)
            .ConfigureAwait(false);
        if (item is null)
        {
            return new Error();
        }

        item.Name = request.Machine.Name;
        item.Description = request.Machine.Description;
        item.Versions = [.. request.Machine.Versions];
        await this.UpdateItemAsync(
            item,
            item.Code,
            cancellationToken)
            .ConfigureAwait(false);
        return new UpdateMachineResponse();
    }

    protected override FilterDefinition<MachineDetailsDto> FilterById(FilterDefinitionBuilder<MachineDetailsDto> filterDefinitionBuilder, string id)
    {
        return filterDefinitionBuilder.Eq(i => i.Code, id);
    }

    protected override UpdateDefinition<MachineDetailsDto> CreateUpdateDefinition(MachineDetailsDto payload)
    {
        var update = Builders<MachineDetailsDto>.Update
            .Set(r => r.Name, payload.Name)
            .Set(r => r.Description, payload.Description)
            .Set(r => r.Versions, payload.Versions);
        return update;
    }

    protected override IMongoCollection<MachineDetailsDto> GetCollection()
    {
        return this.collectionProvider.Machines;
    }
}
