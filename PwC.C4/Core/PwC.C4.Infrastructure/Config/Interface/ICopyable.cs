using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Infrastructure.Config
{
    public interface ICopyable
    {
        /// <summary>
        /// copy property to destination object
        /// </summary>
        /// <param name="destObject">destination object</param>
        void CopyTo(object destObject);
    }
}
