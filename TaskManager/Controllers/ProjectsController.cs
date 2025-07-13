using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Commands;
using TaskManager.Application.Queries;
using TaskManager.Domain.Exceptions;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém todos os projetos de um usuário
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <returns>Lista de projetos do usuário</returns>
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectsByUser(Guid userId)
        {
            try
            {
                var query = new GetProjectsByUserQuery { UserId = userId };
                var projects = await _mediator.Send(query);
                return Ok(projects);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo projeto
        /// </summary>
        /// <param name="command">Dados do projeto a ser criado</param>
        /// <returns>Projeto criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            try
            {
                var project = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetProjectsByUser),
                    new { userId = project.UserId }, project);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
