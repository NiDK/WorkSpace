namespace PwC.C4.Testing.Dfs.ClientInstance
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.DownloadButton = new System.Windows.Forms.Button();
            this.UploadButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.FileList = new System.Windows.Forms.ListView();
            this.FilenameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FullPathHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.txtDfsPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(353, 234);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(85, 31);
            this.DownloadButton.TabIndex = 1;
            this.DownloadButton.Text = "&Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // UploadButton
            // 
            this.UploadButton.Location = new System.Drawing.Point(262, 234);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(85, 31);
            this.UploadButton.TabIndex = 2;
            this.UploadButton.Text = "&Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(11, 234);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(85, 31);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.Text = "&Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FileList
            // 
            this.FileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FilenameColumnHeader,
            this.FullPathHeader,
            this.SizeHeader});
            this.FileList.FullRowSelect = true;
            this.FileList.GridLines = true;
            this.FileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FileList.Location = new System.Drawing.Point(11, 36);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(427, 192);
            this.FileList.TabIndex = 4;
            this.FileList.UseCompatibleStateImageBehavior = false;
            this.FileList.View = System.Windows.Forms.View.Details;
            // 
            // FilenameColumnHeader
            // 
            this.FilenameColumnHeader.Text = "Filename";
            this.FilenameColumnHeader.Width = 80;
            // 
            // FullPathHeader
            // 
            this.FullPathHeader.Text = "Storage path";
            this.FullPathHeader.Width = 160;
            // 
            // SizeHeader
            // 
            this.SizeHeader.Text = "Size";
            this.SizeHeader.Width = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Dfs path";
            // 
            // txtDfsPath
            // 
            this.txtDfsPath.Location = new System.Drawing.Point(64, 10);
            this.txtDfsPath.Name = "txtDfsPath";
            this.txtDfsPath.Size = new System.Drawing.Size(374, 20);
            this.txtDfsPath.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 271);
            this.Controls.Add(this.txtDfsPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileList);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.DownloadButton);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Server Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button DownloadButton;
		private System.Windows.Forms.Button UploadButton;
		private System.Windows.Forms.Button DeleteButton;
		private System.Windows.Forms.ListView FileList;
		private System.Windows.Forms.ColumnHeader FilenameColumnHeader;
		private System.Windows.Forms.ColumnHeader FullPathHeader;
		private System.Windows.Forms.ColumnHeader SizeHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDfsPath;
    }
}

