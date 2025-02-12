using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;
[Table("Students", Schema = "dbo")]
public class Student
{

    public int Id { get; set; }
    //[NotMapped]
    //[Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; } = string.Empty;
    //[MinAge(MinAge:18,MaxAge:30 ), DisplayName("Date Of Birth")]
    //[Column(TypeName = "datetime2")]
    public DateTime? DateOfBirth { get; set; }
    [MaxLength(150)]
    public string Email { get; set; }
}
