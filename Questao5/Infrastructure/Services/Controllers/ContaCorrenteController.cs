using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain;
using Questao5.Extensions;
using System.Net;

namespace Questao5.Infrastructure.Services.Controllers;

public class ContaCorrenteController : BaseController
{

    private readonly IMediator _mediator;

    public ContaCorrenteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("movimento")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> PostMovimentoContaCorrenteAsync([FromBody] MovimentoRequestCommand request)
    {
        try
        {
            if (request == null)
                throw new ArgumentNullOrEmptyException(nameof(request));

            var result = await _mediator.Send(request);

            return Ok(result);
        }
        catch (NotificationException ex)
        {
            return BadRequest(new Notification(ex.Message, ex.Code));
        }
        catch (ArgumentException ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("saldo/{numeroConta}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSaldoContaCorrenteAsync(int numeroConta)
    {
        try
        {
            if (numeroConta <= 0)
                throw new ArgumentNullOrEmptyException(nameof(numeroConta));

            var request = new SaldoRequestQuery(numeroConta);

            var result = await _mediator.SendCommand<SaldoResponseQuery>(request);

            return Ok(result);
        }
        catch (NotificationException ex)
        {
            return BadRequest(new Notification(ex.Message, ex.Code));
        }
        catch (ArgumentException ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

}
