using Hackathon.HealthMed.Kernel.Shared;
using MediatR;

namespace Hackathon.HealthMed.Kernel.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}