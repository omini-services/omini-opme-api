using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdatePhysicianCommand : ICommand<Physician>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; set; }
    public string? MiddleName { get; init; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }


    public class UpdatePhysicianCommandHandler : ICommandHandler<UpdatePhysicianCommand, Physician>
    {
        private readonly IPhysicianService _physicianService;
        public UpdatePhysicianCommandHandler(IPhysicianService physicianService)
        {
            _physicianService = physicianService;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(UpdatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianService.GetById(request.Id, cancellationToken);
            if (physician is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            physician.Cro = request.Cro;
            physician.Crm = request.Crm;
            physician.Name = new PersonName(request.FirstName, request.LastName, request.MiddleName);
            physician.Comments = request.Comments;

            await _physicianService.Update(physician, cancellationToken);

            return physician;
        }
    }
}