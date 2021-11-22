using backlog.api.Classes;
using backlog.api.DTO;
using backlog.api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backlog.api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class TodosController : ControllerBase
{
    private readonly ILogger<TodosController> _logger;
    private readonly Context _context;

    public TodosController(
        ILogger<TodosController> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Добавить задачу
    /// </summary>
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public IActionResult Insert(TodoCreateRequest todoCreateRequest)
    {
        try
        {
            _logger.LogDebug("Запрос на создание задачи {name}", todoCreateRequest.Title);

            var userId = this.CurrentUserId();

            if (!_context.UserProjects
                .Any(t => t.ProjectId == todoCreateRequest.ProjectId && t.UserId == userId && t.UserRoleId != (int)EnumUserRoles.Reader))
                throw new Exception("У Вас нет доступа к изменениям по проекту");

            var todo = new Todo
            {
                ProjectId = todoCreateRequest.ProjectId,
                Title = todoCreateRequest.Title,
                Content = todoCreateRequest.Content,
                PriorityId = todoCreateRequest.PriorityId,
                CreateTimeStamp = DateTime.Now,
                CreatorId = userId
            };
            _context.Todos.Add(todo);
            _context.SaveChanges();

            _logger.LogDebug("Создана задача {id} пользователем {userId}", todo.Id, userId);

            return Ok(todo.Id);
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при создании задачи";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpDelete("{todoId}")]
    public IActionResult Delete(int todoId)
    {
        try
        {
            _logger.LogDebug("Запрос на удаление задачи {id}", todoId);

            var userId = this.CurrentUserId();

            var todo = _context.Todos.First(t => t.Id == todoId);

            if (!_context.UserProjects
                .Any(t => t.ProjectId == todo.ProjectId && t.UserId == userId && t.UserRoleId != (int)EnumUserRoles.Reader))
                throw new Exception("У Вас нет доступа к изменениям по проекту");

            _context.Todos.Remove(todo);
            _context.SaveChanges();

            _logger.LogDebug("Удалена задача {id} пользователем {userId}", todoId, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при удалении задачи";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Изменение задачи
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public IActionResult Update([FromBody] TodoUpdateRequest todoUpdateRequest)
    {
        try
        {
            _logger.LogDebug("Запрос на изменение задачи {id}", todoUpdateRequest.TodoId);

            var userId = this.CurrentUserId();

            var todo = _context.Todos.First(t => t.Id == todoUpdateRequest.TodoId);

            if (!_context.UserProjects
                .Any(t => t.ProjectId == todo.ProjectId && t.UserId == userId && t.UserRoleId != (int)EnumUserRoles.Reader))
                throw new Exception("У Вас нет доступа к изменениям по проекту");

            todo.Title = todoUpdateRequest.Title;
            todo.Content= todoUpdateRequest.Content;
            todo.PriorityId = todoUpdateRequest.PriorityId;

            _context.SaveChanges();

            _logger.LogDebug("Изменена задача {id} пользователем {userId}", todo.Id, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при изменении задачи";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Задачи по проекту
    /// </summary>
    [ProducesResponseType(typeof(IEnumerable<Todo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public IActionResult Get([FromQuery] int projectId)
    {
        try
        {
            _logger.LogDebug("Запрос на получение задач по проекту {proj}", projectId);

            var userId = this.CurrentUserId();
            if (!_context.UserProjects.Any(up => up.ProjectId == projectId && up.UserId == userId))
                return StatusCode(StatusCodes.Status403Forbidden, "У Вас нет доступа к проекту");

            var todos = _context.Todos
                .Include(t=>t.Priority)
                .Include(t=>t.Creator)
                .Where(t => t.ProjectId == projectId)
                .ToArray();

            _logger.LogDebug("Возвращаем пользователю {userId} список задач {count} шт", userId, todos.Length);

            return Ok(todos);
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при получении списка задач";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Задача по id
    /// </summary>
    [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("{todoId}")]
    public IActionResult GetById(int todoId)
    {
        try
        {
            _logger.LogDebug("Запрос на получение задачи {id}", todoId);

            var userId = this.CurrentUserId();
            var todo = _context.Todos
                .Include(t => t.Creator)
                .First(t => t.Id == todoId);
            if (!_context.UserProjects.Any(u => u.UserId == userId && u.ProjectId == todo.ProjectId))
                return StatusCode(StatusCodes.Status403Forbidden, "У Вас нет доступа к проекту");

            _logger.LogDebug("Возращаем пользователю задачу {id}", todo.Id);

            return Ok(todo);
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при получении задачи";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }
}
