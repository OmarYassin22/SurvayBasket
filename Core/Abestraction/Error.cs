using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abestraction;
public record Error(string code, string description)
{
    public static Error None = new Error(string.Empty, string.Empty);
}
