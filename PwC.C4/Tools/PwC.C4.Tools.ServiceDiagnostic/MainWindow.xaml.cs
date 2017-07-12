using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PwC.C4.Tools.ServiceDiagnostic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void comboBoxEnv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var str = comboBoxEnv.SelectionBoxItem.ToString();

            switch (str)
            {
                case "Dev":
                    this.textBoxServerDomain.Text = "http://CNPEKAPPDWV031/PwC.Configuration/ConfigVersionHandler.ashx";
                    break;
                case "Uat":
                    this.textBoxServerDomain.Text = "http://10.150.64.11/PwC.Configuration/ConfigVersionHandler.ashx";
                    break;
                case "Prod":
                    this.textBoxServerDomain.Text = "https://hkapppwv173/PwC.Configuration/ConfigVersionHandler.ashx";
                    break;
            }

        }
    }
}
