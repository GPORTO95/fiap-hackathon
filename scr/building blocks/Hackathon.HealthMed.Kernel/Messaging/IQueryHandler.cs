using Hackathon.HealthMed.Kernel.Shared;
using MediatR;

namespace Hackathon.HealthMed.Kernel.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}