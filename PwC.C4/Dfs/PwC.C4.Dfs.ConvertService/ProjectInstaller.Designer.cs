namespace PwC.C4.Dfs.ConvertService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConvertServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.convertServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ConvertServiceProcessInstaller
            // 
            this.ConvertServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ConvertServiceProcessInstaller.Password = null;
            this.ConvertServiceProcessInstaller.Username = null;
            // 
            // convertServiceInstaller
            // 
            this.convertServiceInstaller.DelayedAutoStart = true;
            this.convertServiceInstaller.Description = "File storge service base on c4";
            this.convertServiceInstaller.DisplayName = "PwC.C4.Dfs.ConvertService";
            this.convertServiceInstaller.ServiceName = "PwC.C4.Dfs.ConvertService";
            this.convertServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ConvertServiceProcessInstaller,
            this.convertServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ConvertServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller convertServiceInstaller;
    }
}