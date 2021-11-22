using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backlog.api.Model;

public class Todo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(80)]
    public string Title { get; set; }

    public string Content { get; set; }

    public int ProjectId { get; set; }

    public virtual Project Project { get; set; }

    public int CreatorId { get; set; }

    public virtual User Creator { get; set; }

    public int PriorityId { get; set; }

    public virtual Priority Priority { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
