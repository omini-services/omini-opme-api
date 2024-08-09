using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;

namespace Omini.Opme.Business.Commands;

public sealed record PrepareDatabaseCommand : ICommand
{
    public class PrepareDatabaseCommandHandler : ICommandHandler<PrepareDatabaseCommand>
    {
        private readonly IDatabaseRepository _databaseRepository;

        public PrepareDatabaseCommandHandler(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public async Task Handle(PrepareDatabaseCommand request, CancellationToken cancellationToken)
        {
            await _databaseRepository.PrepareDatabase();
        }
    }
}