using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Blog
{
    public int Id { get; set; }
    public string url { get; set; } = string.Empty;
    public IList<Commints> Commints { get; set; } = new List<Commints>();
    //[ForeignKey("Post")]
    //public int PostId { get; set; }
    //public IList<Post> Post { get; set; }
}
