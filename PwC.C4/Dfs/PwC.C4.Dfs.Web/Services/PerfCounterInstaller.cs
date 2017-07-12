using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace PwC.C4.Dfs.Web.Services
{
    [RunInstaller(true)]
    public partial class PerfCounterInstaller : Installer
    {
        public PerfCounterInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            PerfCounters.InstallCounters();
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            PerfCounters.RemoveCounters();
            base.Uninstall(savedState);
        }
    }
}
