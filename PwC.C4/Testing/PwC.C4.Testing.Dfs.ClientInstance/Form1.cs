using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Model;

namespace PwC.C4.Testing.Dfs.ClientInstance
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Handles the Load event of the Form1 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Form1_Load(object sender, EventArgs e)
		{
			//RefreshFileList();
		}

		/// <summary>
		/// Refreshes the file list.
		/// </summary>
		private void RefreshFileList()
		{
			StorageFileInfo[] files = null;

			//using (C4.Dfs.Client.Dfs.GetDfsRecords() client = new C4.Dfs.Client.Client.FileRepositoryServiceClient())
			//{
			//	files = client.List(null);
			//}

			FileList.Items.Clear();

			int width = FileList.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;

			float[] widths = { .2f, .6f, .2f };

			for (int i = 0; i < widths.Length; i++)
				FileList.Columns[i].Width = (int)((float)width * widths[i]);

			foreach (var file in files)
			{
				ListViewItem item = new ListViewItem(Path.GetFileName(file.VirtualPath));

				item.SubItems.Add(file.VirtualPath);

				float fileSize = (float)file.Size / 1024.0f;
				string suffix = "Kb";

				if (fileSize > 1000.0f)
				{
					fileSize /= 1024.0f;
					suffix = "Mb";
				}
				item.SubItems.Add(string.Format("{0:0.0} {1}", fileSize, suffix));

				FileList.Items.Add(item);
			}
		}

		private void DownloadButton_Click(object sender, EventArgs e)
		{
		    var dfs = this.txtDfsPath.Text;
		    if (string.IsNullOrEmpty(dfs)) return;
		    var dfsPath = DfsPath.Parse(dfs);
		    var items = C4.Dfs.Client.Dfs.Get(dfsPath);
		    var fileName = dfsPath.FileId + "." + dfsPath.FileExtension;
		    var dlg = new SaveFileDialog()
		    {
		        RestoreDirectory = true,
		        OverwritePrompt = true,
		        Title = @"Save as...",
		        FileName = Path.GetFileName(fileName)
		    };

		    dlg.ShowDialog(this);
		        
		    using (var output = new FileStream(dlg.FileName, FileMode.Create))
		    {
		        items.FileDataStream.CopyTo(output);
		    }

		    Process.Start(dlg.FileName);
		}

		private void UploadButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog()
			{
				Title = "Select a file to upload",
				RestoreDirectory = true,
				CheckFileExists = true
			};

			dlg.ShowDialog();

			if (!string.IsNullOrEmpty(dlg.FileName))
			{
				string virtualPath = Path.GetFileName(dlg.FileName);

				using (Stream uploadStream = new FileStream(dlg.FileName, FileMode.Open))
				{
                    var dfsItme = new DfsItem("Image", dlg.SafeFileName, uploadStream,"utf-8","PwC-C4-Labs");
				    var dfsPath = C4.Dfs.Client.Dfs.Store(dfsItme,"");
				}

				//RefreshFileList();
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{

			if (FileList.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select a file to delete");
			}
			else
			{
				string virtualPath = FileList.SelectedItems[0].SubItems[1].Text;

				//using (C4.Dfs.Client.Client.FileRepositoryServiceClient client = new C4.Dfs.Client.Client.FileRepositoryServiceClient())
				//{
				//	client.DeleteFile(virtualPath);
				//}

				//RefreshFileList();
			}

		}

	}
}
