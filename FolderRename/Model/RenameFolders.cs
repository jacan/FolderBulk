using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderRename.Model
{
	public class RenameFolders : IRenameDiskFileAndFolders
	{
		public string BasePath { get; set; }

		public RenameFolders(string basePath)
		{
			BasePath = basePath;
		}

		public void RenameAll(Func<string, string> renameSeed)
		{
			if (!Directory.Exists(BasePath))
			{
				throw new DirectoryNotFoundException();
			}

			string newFolderName = null;
			string seed = null;

			string[] folders = Directory.GetDirectories(BasePath);

			folders.ToList().ForEach(subFolder =>
			{
				seed = renameSeed(seed);

				if (CreateFolderName(seed, out newFolderName))
				{
					Directory.Move(subFolder, newFolderName);
				}
			});
		}

		private bool CreateFolderName(string seed, out string folderName)
		{
			var sbIndexString = new StringBuilder();

			sbIndexString.Append(BasePath.EndsWith("\\") ? BasePath : BasePath + "\\");
			sbIndexString.Append(seed);

			folderName = sbIndexString.ToString();

			return !Directory.Exists(folderName);
		}
	}
}
