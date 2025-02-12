using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Commints
{
    public int Id { get; set; }
    //public string BlogUrl { get; set; }
    public string CommentUser { get; set; }
    public Blog Blog { get; set; }
}
