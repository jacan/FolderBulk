using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FolderRename.Model;

namespace FolderRename
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (Directory.Exists(dlg.SelectedPath))
					{
						if (MessageBox.Show(string.Format("Last chance!!\r\n\r\n Proceed renaming directories in {0}", dlg.SelectedPath), "Warning: Renaming folders", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
						{
							return;
						}

						int index = 0;
						string newFolderName = null;

						string[] folders = Directory.GetDirectories(dlg.SelectedPath);

						foreach (string subFolder in folders)
						{
							if (CreateIndexFoldername(index, dlg.SelectedPath, out newFolderName))
							{
								//listBox1.Items.Add(string.Format("Renaming directory {0} => {1}", subFolder, newFolderName));
								Directory.Move(subFolder, newFolderName);
							}

							index++;
						}
					}
				}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (MessageBox.Show(string.Format("Last chance!!\r\n\r\n Proceed renaming files in directory\r\n\r\n{0}", dlg.SelectedPath), "Warning: Renaming files", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
					{
						return;
					}

					//listBox1.Items.Clear();
					if (Directory.Exists(dlg.SelectedPath))
					{
						var files = new DirectoryInfo(dlg.SelectedPath).GetFiles().OrderBy(x => x.CreationTimeUtc);
						var index = 0;
						files.ToList().ForEach(file =>
						{
							var newFilename = CreateNewIndexedFilename(file, index++.ToString());
							//listBox1.Items.Add(string.Format("Renaming file {0} => {1}", file.FullName, newFilename));
							File.Move(
								file.FullName,
								newFilename);
						});
					}
				}
			}
		}

		private bool CreateIndexFoldername(int indexName, string basePath, out string indexString)
		{
			indexString = string.Format("{0:000}", indexName);

			if (!basePath.EndsWith("\\"))
				basePath += "\\";

			indexString = basePath + indexString;

			return !Directory.Exists(indexString);
		}

		private string CreateNewIndexedFilename(FileInfo file, string filenameToChangeTo)
		{
			var sbNewFileName = new StringBuilder();

			var filenameIndex = file.FullName.LastIndexOf(@"\")+1;
			
			sbNewFileName.Append(file.FullName.Remove(filenameIndex));
			sbNewFileName.Append(filenameToChangeTo);

			if (!String.IsNullOrEmpty(file.Extension))
			{
				sbNewFileName.Append(file.Extension);
			}

			return sbNewFileName.ToString();
		}

		private void OnRename(object sender, EventArgs e)
		{
			IRenameDiskFileAndFolders renamer = rbFiles.Checked ? new RenameFiles("") : new RenameFolders("");

			// Stuff to do... UI
		
		}
	}
}