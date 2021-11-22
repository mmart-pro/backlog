using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backlog.api.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(40)]
    public string Login { get; set; }

    [MaxLength(32)]
    public string Password { get; set; }

    [MaxLength(80)]
    public string Name { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
