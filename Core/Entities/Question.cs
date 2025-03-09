using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public sealed class Question : AuditableEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int PollId { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Answer> Answers { get; set; } = [];
    public Poll Poll { get; set; } = default!;
}
