using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Infrastructure.Model.Enum
{
    public enum ErrorStatus
    {
        Untreated = 0,
        Processing = 1,
        Fixed = 2,
        Igrone = 3,
        NeverRemind =4
    }
}
