using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderRename.Model
{
	public class RenameFiles : IRenameDiskFileAndFolders
	{
		public string BasePath { get; set; }

		public RenameFiles(string basePath)
		{
			BasePath = basePath;
		}

		public void RenameAll(Func<string, string> renameSeed)
		{
			if (!Directory.Exists(BasePath))
			{
				throw new DirectoryNotFoundException();
			}

			var files = new DirectoryInfo(BasePath).GetFiles().OrderBy(x => x.CreationTimeUtc);
			string seed = null;

			files.ToList().ForEach(file =>
			{
				seed = renameSeed(seed);
				var newFilename = CreateFileName(file, seed);

				if (File.Exists(newFilename))
				{
					var tempFileName = CreateFileName(file, Guid.NewGuid().ToString());
					File.Move(newFilename, tempFileName);
				}

				File.Move(
					file.FullName,
					newFilename);
			});
		}

		private string CreateFileName(FileInfo file, string seed)
		{
			var sbNewFileName = new StringBuilder();

			var filenameIndex = file.FullName.LastIndexOf(@"\") + 1;

			sbNewFileName.Append(file.FullName.Remove(filenameIndex));
			sbNewFileName.Append(seed);

			if (!String.IsNullOrEmpty(file.Extension))
			{
				sbNewFileName.Append(file.Extension);
			}

			return sbNewFileName.ToString();
		}
	}
}
