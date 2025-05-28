using MediatR;

namespace Questao5.Extensions;

public static class MediatorExtension
{

    public static async Task SendCommand(this IMediator mediator, IRequest request)
    {
        if (mediator == null)
            throw new ArgumentNullException(nameof(mediator));

        await mediator.Send(request);
    }

    public static async Task<TResult> SendCommand<TResult>(this IMediator mediator, IRequest<TResult> request)
    {
        if (mediator == null)
            throw new ArgumentNullException(nameof(mediator));

        return await mediator.Send(request);
    }

}
