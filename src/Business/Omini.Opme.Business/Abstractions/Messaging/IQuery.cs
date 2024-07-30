using MediatR;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Abstractions.Messaging;


public interface IQuery<TResponse> : IRequest<PagedResult<TResponse>>{

}