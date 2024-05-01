using FluentValidation;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreatePhysicianCommand : IRequest<Result<Physician, ValidationException>>
{
    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }

    public class CreatePhysicianCommandHandler : IRequestHandler<CreatePhysicianCommand, Result<Physician, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianRepository;
        public CreatePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository physicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = physicianRepository;
        }

        public async Task<Result<Physician, ValidationException>> Handle(CreatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var Physician = new Physician(){
                Cro = request.Cro,
                Crm = request.Crm,
                Name = new PersonName(request.FirstName, request.MiddleName, request.LastName),
                Comments = request.Comments
            };

            await _physicianRepository.Add(Physician);
            await _unitOfWork.Commit();

            return Physician;
        }
    }
}