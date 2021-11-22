using Microsoft.EntityFrameworkCore;

namespace backlog.api.Model;

public class Context : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Todo> Todos { get; set; }

    public DbSet<UserProject> UserProjects { get; set; }

    public DbSet<Priority> Priorities { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProject>().HasKey(e => new { e.ProjectId, e.UserId });

        // Роли пользователей в проекте
        var users = new User[]
        {
            new (){ Id = -1, Login = "root", Password="*", Name = "root", CreateTimeStamp = new DateTime(2021, 11, 18)  }
        };
        modelBuilder.Entity<User>().HasData(users);

        // Роли пользователей в проекте
        var userRoles = new UserRole[]
        {
            new (){ Id = (int)EnumUserRoles.Owner, Name = "Администратор" },
            new (){ Id = (int)EnumUserRoles.Creator, Name = "Изменение" },
            new (){ Id = (int)EnumUserRoles.Reader, Name = "Просмотр" }
        };
        modelBuilder.Entity<UserRole>().HasData(userRoles);

        // Приоритеты
        var priorities = new Priority[]
        {
            new (){ Id = (int)EnumPriorities.Low, Name = "Низкий" },
            new (){ Id = (int)EnumPriorities.Normal, Name = "Нормальный" },
            new (){ Id = (int)EnumPriorities.High, Name = "Высокий" },
            new (){ Id = (int)EnumPriorities.Critical, Name = "Критичный" }
        };
        modelBuilder.Entity<Priority>().HasData(priorities);

        base.OnModelCreating(modelBuilder);
    }
}
