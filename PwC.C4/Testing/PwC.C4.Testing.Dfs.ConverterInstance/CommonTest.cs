using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PwC.C4.Dfs.Converter;
using PwC.C4.Dfs.Converter.Config;

namespace PwC.C4.Testing.Dfs.ConverterInstance
{
    public static class CommonTest
    {
        public static void TestConvert()
        {
            var convertServer = new FileProcessProvider();
            convertServer.StartScheduler();
        }
    }
}
