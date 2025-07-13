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
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém todas as tarefas de um projeto
        /// </summary>
        /// <param name="projectId">ID do projeto</param>
        /// <param name="userId">ID do usuário que está fazendo a requisição</param>
        /// <returns>Lista de tarefas do projeto</returns>
        [HttpGet("project/{projectId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTasksByProject(Guid projectId, [FromQuery] Guid userId)
        {
            try
            {
                var query = new GetTasksByProjectQuery { ProjectId = projectId, UserId = userId };
                var tasks = await _mediator.Send(query);
                return Ok(tasks);
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

        /// <summary>
        /// Cria uma nova tarefa
        /// </summary>
        /// <param name="command">Dados da tarefa a ser criada</param>
        /// <returns>Tarefa criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            try
            {
                var task = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetTasksByProject),
                    new { projectId = task.ProjectId, userId = task.UserId }, task);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (BusinessRuleViolationException ex)
            {
                return Forbid(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza uma tarefa existente
        /// </summary>
        /// <param name="taskId">ID da tarefa</param>
        /// <param name="command">Dados atualizados da tarefa</param>
        /// <returns>Tarefa atualizada</returns>
        [HttpPut("{taskId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            try
            {
                command.TaskId = taskId;
                var task = await _mediator.Send(command);
                return Ok(task);
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

        /// <summary>
        /// Remove uma tarefa
        /// </summary>
        /// <param name="taskId">ID da tarefa</param>
        /// <param name="userId">ID do usuário que está removendo</param>
        /// <returns>Confirmação da remoção</returns>
        [HttpDelete("{taskId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(Guid taskId, [FromQuery] Guid userId)
        {
            try
            {
                var command = new DeleteTaskCommand { TaskId = taskId, UserId = userId };
                await _mediator.Send(command);
                return NoContent();
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

        /// <summary>
        /// Adiciona um comentário a uma tarefa
        /// </summary>
        /// <param name="taskId">ID da tarefa</param>
        /// <param name="command">Dados do comentário</param>
        /// <returns>Comentário criado</returns>
        [HttpPost("{taskId:guid}/comments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddTaskComment(Guid taskId, [FromBody] AddTaskCommentCommand command)
        {
            try
            {
                command.TaskId = taskId;
                var comment = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetTasksByProject),
                    new { projectId = taskId }, comment);
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
