using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;

namespace PwC.C4.Metadata.Interface
{
    public interface IPackageService
    {
        bool PackagingZip(List<Attachment> atts, string zipName, string password = null);
    }
}
