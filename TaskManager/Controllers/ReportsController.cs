using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Queries;
using TaskManager.Domain.Exceptions;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gera relatório de performance de um usuário (somente para gerentes)
        /// </summary>
        /// <param name="userId">ID do usuário para gerar relatório</param>
        /// <param name="requestingUserId">ID do usuário que está solicitando o relatório</param>
        /// <param name="days">Número de dias para o relatório (padrão: 30)</param>
        /// <returns>Relatório de performance</returns>
        [HttpGet("performance/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPerformanceReport(
            Guid userId,
            [FromQuery] Guid requestingUserId,
            [FromQuery] int days = 30)
        {
            try
            {
                var query = new GetPerformanceReportQuery
                {
                    UserId = userId,
                    RequestingUserId = requestingUserId,
                    Days = days
                };

                var report = await _mediator.Send(query);
                return Ok(report);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (BusinessRuleViolationException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
