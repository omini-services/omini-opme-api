using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdatePhysicianCommand : ICommand<Physician>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }


    public class UpdatePhysicianCommandHandler : ICommandHandler<UpdatePhysicianCommand, Physician>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianRepository;
        public UpdatePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository physicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = physicianRepository;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(UpdatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianRepository.GetById(request.Id, cancellationToken);
            if (physician is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            physician.SetData(name: new PersonName(request.FirstName, request.LastName, request.MiddleName),
                cro: request.Cro,
                crm: request.Crm,
                comments: request.Comments    
            );

            _physicianRepository.Update(physician, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return physician;
        }
    }
}