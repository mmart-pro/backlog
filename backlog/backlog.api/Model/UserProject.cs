namespace backlog.api.Model;

public class UserProject
{
    public int UserId { get; set; }

    public virtual User User { get; set; }

    public int ProjectId { get; set; }

    public virtual Project Project { get; set; }

    public int UserRoleId { get; set; }

    public virtual UserRole UserRole { get; set; }
}
