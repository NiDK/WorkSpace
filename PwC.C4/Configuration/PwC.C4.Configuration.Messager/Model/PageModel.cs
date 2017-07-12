using System.Collections.Generic;

namespace PwC.C4.Configuration.Messager.Model
{
    public class PageModel<T>
    {
        public List<T> Datas { get; set; }
        public int TotalCount { get; set; } 
        public int CurrentPage { get; set; }
    }
}