using SurvayBasket.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [MinAge(MinAge:18,MaxAge:30 ), DisplayName("Date Of Birth")]
    public DateTime DateOfBirth { get; set; }
}
