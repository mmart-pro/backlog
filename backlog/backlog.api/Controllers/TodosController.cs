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
    /// �������� ������
    /// </summary>
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public IActionResult Insert(TodoCreateRequest todoCreateRequest)
    {
        try
        {
            _logger.LogDebug("������ �� �������� ������ {name}", todoCreateRequest.Title);

            var userId = this.CurrentUserId();

            if (!_context.UserProjects
                .Any(t => t.ProjectId == todoCreateRequest.ProjectId && t.UserId == userId && t.UserRoleId != (int)EnumUserRoles.Reader))
                throw new Exception("� ��� ��� ������� � ���������� �� �������");

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

            _logger.LogDebug("������� ������ {id} ������������� {userId}", todo.Id, userId);

            return Ok(todo.Id);
        }
        catch (Exception ex)
        {
            var msg = "������ ��� �������� ������";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// ������� ������
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpDelete("{todoId}")]
    public IActionResult Delete(int todoId)
    {
        try
        {
            _logger.LogDebug("������ �� �������� ������ {id}", todoId);

            var userId = this.CurrentUserId();

            var todo = _context.Todos.First(t => t.Id == todoId);

            if (!_context.UserProjects
                .Any(t => t.ProjectId == todo.ProjectId && t.UserId == userId && t.UserRoleId != (int)EnumUserRoles.Reader))
                throw new Exception("� ��� ��� ������� � ���������� �� �������");

            _context.Todos.Remove(todo);
            _context.SaveChanges();

            _logger.LogDebug("������� ������ {id} ������������� {userId}", todoId, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            var msg = "������ ��� �������� ������";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// ��������� ������
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public IActionResult Update([FromBody] TodoUpdateRequest todoUpdateRequest)
    {
        try
        {
            _logger.LogDebug("������ �� ��������� ������ {id}", todoUpdateRequest.TodoId);

            var userId = this.CurrentUserId();

            var todo = _context.Todos.First(t => t.Id == todoUpdateRequest.TodoId);

            if (!_context.UserProjects
                .Any(t => t.ProjectId == todo.ProjectId && t.UserId == userId && t.UserRoleId != (int)EnumUserRoles.Reader))
                throw new Exception("� ��� ��� ������� � ���������� �� �������");

            todo.Title = todoUpdateRequest.Title;
            todo.Content= todoUpdateRequest.Content;
            todo.PriorityId = todoUpdateRequest.PriorityId;

            _context.SaveChanges();

            _logger.LogDebug("�������� ������ {id} ������������� {userId}", todo.Id, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            var msg = "������ ��� ��������� ������";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// ������ �� �������
    /// </summary>
    [ProducesResponseType(typeof(IEnumerable<Todo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public IActionResult Get([FromQuery] int projectId)
    {
        try
        {
            _logger.LogDebug("������ �� ��������� ����� �� ������� {proj}", projectId);

            var userId = this.CurrentUserId();
            if (!_context.UserProjects.Any(up => up.ProjectId == projectId && up.UserId == userId))
                return StatusCode(StatusCodes.Status403Forbidden, "� ��� ��� ������� � �������");

            var todos = _context.Todos
                .Include(t=>t.Priority)
                .Include(t=>t.Creator)
                .Where(t => t.ProjectId == projectId)
                .ToArray();

            _logger.LogDebug("���������� ������������ {userId} ������ ����� {count} ��", userId, todos.Length);

            return Ok(todos);
        }
        catch (Exception ex)
        {
            var msg = "������ ��� ��������� ������ �����";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// ������ �� id
    /// </summary>
    [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("{todoId}")]
    public IActionResult GetById(int todoId)
    {
        try
        {
            _logger.LogDebug("������ �� ��������� ������ {id}", todoId);

            var userId = this.CurrentUserId();
            var todo = _context.Todos
                .Include(t => t.Creator)
                .First(t => t.Id == todoId);
            if (!_context.UserProjects.Any(u => u.UserId == userId && u.ProjectId == todo.ProjectId))
                return StatusCode(StatusCodes.Status403Forbidden, "� ��� ��� ������� � �������");

            _logger.LogDebug("��������� ������������ ������ {id}", todo.Id);

            return Ok(todo);
        }
        catch (Exception ex)
        {
            var msg = "������ ��� ��������� ������";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }
}
