using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Model;

namespace PwC.C4.Dfs.Converter.Service
{
   public static class DfsHelper
   {
       private static readonly string ServerRootPath = DfsServerConfig.Instance.ServerRootFolder;
       public static string GetFilePhysicalPath(this DfsPath path)
       {
           var p = Path.Combine(ServerRootPath, path.AppCode);
           p = Path.Combine(p, path.Keyspace);
           p = Path.Combine(p, path.FileId + "." + path.FileExtension);
           return p;
       }
    }
}
