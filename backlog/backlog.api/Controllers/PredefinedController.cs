using backlog.api.Model;
using Microsoft.AspNetCore.Mvc;

namespace backlog.api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PredefinedController : ControllerBase
{
    private readonly Context _context;

    public PredefinedController(
        Context context
        )
    {
        _context = context;
    }

    /// <summary>
    /// ���������� �����
    /// </summary>
    [HttpGet]
    public IEnumerable<Priority> GetPriorities()
    {
        return _context.Priorities.ToArray();
    }

    /// <summary>
    /// ���� ������������� � �������
    /// </summary>
    [HttpGet]
    public IEnumerable<UserRole> GetUserRoles()
    {
        return _context.UserRoles.ToArray();
    }
}
