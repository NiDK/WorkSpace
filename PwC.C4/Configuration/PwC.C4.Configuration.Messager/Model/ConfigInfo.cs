using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using PwC.C4.Infrastructure.Helper.NLP.Pinyin;

namespace PwC.C4.Configuration.Messager.Model
{
    public class ConfigInfo
    {
        public string ConfigName { get; set; }

        public List<ConfigSetting> ConfigSettings { get; set; } 
    }
}
