using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PwC.C4.DataService.Model;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Interface
{
    public interface IHtmlSnippetService
    {
        HtmlSnippet GetHtmlSnippet(string code);
    }
}
