namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo
{
    using System.Threading;
    using System.Threading.Tasks;

    using Google.Rpc;

    using MicroDotNet.Services.StateMachines.Application.ReadModel;
    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;
    using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDefinitions;

    using MongoDB.Driver;

    internal class MongoMachineDefinitionsRepository : MongoCollectionRepositoryBase<MachineDefinitionDto, (string Code, short Version)>, IMachineDefinitionsRepository
    {
        private readonly ICollectionProvider collectionProvider;

        public MongoMachineDefinitionsRepository(ICollectionProvider collectionProvider)
        {
            this.collectionProvider = collectionProvider ?? throw new ArgumentNullException(nameof(collectionProvider));
        }

        public async Task<CreateMachineDefinitionResponse> CreateMachineDefinitionAsync(CreateMachineDefinitionRequest request, CancellationToken cancellationToken)
        {
            var payload = new MachineDefinitionDto()
            {
                MachineDefinitionId = request.Machine.Id,
                Code = request.Machine.Code,
                Version = request.Machine.Version,
                Status = request.Machine.Status,
                Nodes = request.Machine.Nodes.Select(n => new MachineDefinitionDto.Node() { Name = n.Name }).ToArray(),
                Transitions = request.Machine.Transitions.Select(t => new MachineDefinitionDto.Transition() { Source = t.Source, Target = t.Target, Trigger = t.Trigger }).ToArray(),
            };

            await this.CreateItemAsync(payload, cancellationToken)
                .ConfigureAwait(false);
            return new CreateMachineDefinitionResponse();
        }

        public async Task<GetMachineDefinitionResponse> GetMachineDefinitionAsync(GetMachineDefinitionRequest request, CancellationToken cancellationToken)
        {
            var item = await this.GetItemAsync((request.Code, request.Version), cancellationToken)
                .ConfigureAwait(false);
            if (item == null)
            {
                throw new InvalidOperationException();
            }

            var machineDefinition = new MachineDefinition(
                item.MachineDefinitionId,
                item.Code,
                item.Version,
                item.Status,
                item.Nodes.Select(n => new Node(n.Name)),
                item.Transitions.Select(t => new Transition(t.Source, t.Target, t.Trigger)));
            return new GetMachineDefinitionResponse(machineDefinition);
        }

        public async Task<UpdateMachineDefinitionResponse> UpdateMachineDefinitionAsync(UpdateMachineDefinitionRequest request, CancellationToken cancellationToken)
        {
            var item = await this.GetItemAsync((request.NewMachineDefinition.Code, request.NewMachineDefinition.Version), cancellationToken)
                .ConfigureAwait(false);
            if (item == null)
            {
                throw new InvalidOperationException();
            }

            item.Status = request.NewMachineDefinition.Status;
            item.Nodes = request.NewMachineDefinition.Nodes
                .Select(n => new MachineDefinitionDto.Node() { Name = n.Name })
                .ToArray();
            item.Transitions = request.NewMachineDefinition.Transitions
                .Select(t => new MachineDefinitionDto.Transition() { Source = t.Source, Target = t.Target, Trigger = t.Trigger })
                .ToArray();
            await this.UpdateItemAsync(
                item,
                (request.NewMachineDefinition.Code, request.NewMachineDefinition.Version),
                cancellationToken)
                .ConfigureAwait(false);
            return new UpdateMachineDefinitionResponse();
        }

        protected override UpdateDefinition<MachineDefinitionDto> CreateUpdateDefinition(MachineDefinitionDto payload)
        {
            var update = Builders<MachineDefinitionDto>.Update
                .Set(m => m.Nodes, payload.Nodes)
                .Set(m => m.Transitions, payload.Transitions)
                .Set(m => m.Status, payload.Status);
            return update;
        }

        protected override FilterDefinition<MachineDefinitionDto> FilterById(FilterDefinitionBuilder<MachineDefinitionDto> filterDefinitionBuilder, (string Code, short Version) id)
        {
            var codeFilter = filterDefinitionBuilder
               .Eq(p => p.Code, id.Code);
            var versionFilter = filterDefinitionBuilder
               .Eq(p => p.Version, id.Version);
            return codeFilter & versionFilter;
        }

        protected override IMongoCollection<MachineDefinitionDto> GetCollection()
        {
            return this.collectionProvider.MachineDefinitions;
        }
    }
}
