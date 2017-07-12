namespace PwC.C4.Dfs.ReceiveService
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
            this.ReceiveServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.receiveServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ReceiveServiceProcessInstaller
            // 
            this.ReceiveServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ReceiveServiceProcessInstaller.Password = null;
            this.ReceiveServiceProcessInstaller.Username = null;
            // 
            // receiveServiceInstaller
            // 
            this.receiveServiceInstaller.DelayedAutoStart = true;
            this.receiveServiceInstaller.Description = "File storge service base on c4";
            this.receiveServiceInstaller.DisplayName = "PwC.C4.Dfs.ReceiveService";
            this.receiveServiceInstaller.ServiceName = "PwC.C4.Dfs.ReceiveService";
            this.receiveServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ReceiveServiceProcessInstaller,
            this.receiveServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ReceiveServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller receiveServiceInstaller;
    }
}