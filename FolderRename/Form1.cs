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
		private string _selectedPath = null;

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
					_selectedPath = dlg.SelectedPath;
				}
			}
		}

		private void OnRename(object sender, EventArgs e)
		{
			IRenameDiskFileAndFolders renamer = null;

			if(rbFiles.Checked)
			{
				renamer = new RenameFiles(_selectedPath);
			}
			else
			{
				renamer = new RenameFolders(_selectedPath);
			}
			
			renamer.RenameAll((x) => {
				var index = Convert.ToInt32(x);
				return (++index).ToString();
			});

			textBox2.Text = "Renamed in " + _selectedPath;
		}
	}
}