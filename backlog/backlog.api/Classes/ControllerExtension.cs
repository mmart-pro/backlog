using Microsoft.AspNetCore.Mvc;

namespace backlog.api.Classes;

public static class ControllerExtension
{
    public static int CurrentUserId(this ControllerBase controller)
    {
        return int.Parse(controller.User?.Claims?.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "-1");
    }
}
