using System.ComponentModel.DataAnnotations;
using MediatR;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse, ValidationResult>>{

}