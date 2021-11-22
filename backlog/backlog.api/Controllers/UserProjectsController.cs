using backlog.api.Classes;
using backlog.api.DTO;
using backlog.api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backlog.api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class UserProjectsController : ControllerBase
{
    private readonly ILogger<UserProjectsController> _logger;
    private readonly Context _context;

    public UserProjectsController(
        ILogger<UserProjectsController> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Добавить проект
    /// </summary>
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public IActionResult Insert(string projectName)
    {
        try
        {
            _logger.LogDebug("Запрос на создание проекта {name}", projectName);

            var userId = this.CurrentUserId();

            var userProject = new UserProject
            {
                Project = new Project
                {
                    CreateTimeStamp = DateTime.Now,
                    Name = projectName
                },
                UserId = userId,
                UserRoleId = (int)EnumUserRoles.Owner
            };
            _context.UserProjects.Add(userProject);
            _context.SaveChanges();

            _logger.LogDebug("Создан проект {id} пользователем {userId}", userProject.ProjectId, userId);

            return Ok(userProject.ProjectId);
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при создании проекта";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Удалить проект
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpDelete("{projectId}")]
    public IActionResult Delete(int projectId)
    {
        try
        {
            _logger.LogDebug("Запрос на удаление проекта {id}", projectId);

            var userId = this.CurrentUserId();

            _context.Projects.Remove(
                _context.UserProjects
                    .Include(up => up.Project)
                    .First(up => up.ProjectId == projectId && up.UserId == userId && up.UserRoleId == (int)EnumUserRoles.Owner)
                    .Project);
            _context.SaveChanges();

            _logger.LogDebug("Удалён проект {id} пользователем {userId}", projectId, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при удалении проекта";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Переименовать проект
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public IActionResult Update([FromBody] ProjectUpdateRequest projectUpdateRequest)
    {
        try
        {
            _logger.LogDebug("Запрос на изменение проекта {id}", projectUpdateRequest.ProjectId);

            var userId = this.CurrentUserId();

            var project = _context.UserProjects
                .Include(up => up.Project)
                .First(up => up.ProjectId == projectUpdateRequest.ProjectId && up.UserId == userId && up.UserRoleId == (int)EnumUserRoles.Owner)
                .Project;
            project.Name = projectUpdateRequest.Name;
            _context.SaveChanges();

            _logger.LogDebug("Изменён проект {id} пользователем {userId}", project.Id, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при изменении проекта";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Доступные пользователю проекты
    /// </summary>
    [ProducesResponseType(typeof(IEnumerable<UserProject>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            _logger.LogDebug("Запрос на получение проектов");

            var userId = this.CurrentUserId();
            var projects = _context.UserProjects
                .Include(up => up.Project)
                .Include(up => up.UserRole)
                .Where(up => up.UserId == userId)
                .ToArray();

            _logger.LogDebug("Возвращаем пользователю {userId} список проектов {count} шт", userId, projects.Length);

            return Ok(projects);
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при получении списка проектов";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }

    /// <summary>
    /// Проект по id
    /// </summary>
    [ProducesResponseType(typeof(UserProject), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("{projectId}")]
    public IActionResult Get(int projectId)
    {
        try
        {
            _logger.LogDebug("Запрос на получение проекта {id}", projectId);

            var userId = this.CurrentUserId();
            var project = _context.UserProjects
                .Include(up => up.Project)
                .Include(up => up.UserRole)
                .First(up => up.UserId == userId && up.ProjectId == projectId);

            _logger.LogDebug("Возращаем пользователю проект {id}", project.ProjectId);

            return Ok(project);
        }
        catch (Exception ex)
        {
            var msg = "Ошибка при получении проекта";
            _logger.LogError(ex, msg);
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }
    }
}
