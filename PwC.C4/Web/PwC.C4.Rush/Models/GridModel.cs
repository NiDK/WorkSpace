using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Rush.Models
{
    public class GridModel<T>
    {
        public int total { get; set; }

        public List<T> rows { get; set; } 
    }
}
