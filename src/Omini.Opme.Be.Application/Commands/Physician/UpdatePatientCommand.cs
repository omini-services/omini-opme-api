using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdatePhysicianCommand : IRequest<Result<Physician, ValidationException>>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }


    public class UpdatePhysicianCommandHandler : IRequestHandler<UpdatePhysicianCommand, Result<Physician, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianRepository;
        public UpdatePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository physicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = physicianRepository;
        }

        public async Task<Result<Physician, ValidationException>> Handle(UpdatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianRepository.GetById(request.Id);
            if (physician is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            physician.Cro = request.Cro;
            physician.Crm = request.Crm;
            physician.Name = new PersonName(request.FirstName, request.MiddleName, request.LastName);
            physician.Comments = request.Comments;

            await _unitOfWork.Commit();

            return physician;
        }
    }
}