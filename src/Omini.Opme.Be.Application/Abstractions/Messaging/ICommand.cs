using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse, ValidationResult>>{

}