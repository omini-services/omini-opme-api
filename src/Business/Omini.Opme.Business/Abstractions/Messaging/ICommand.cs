using FluentValidation.Results;
using MediatR;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Application.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse, ValidationResult>>{

}