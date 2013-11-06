using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FolderRename.Model
{
	public class FileRename
	{
		public string BasePath { get; set; }
		public FileRename(string basePath)
		{
			BasePath = basePath;
		}

		public void RenameAllFolders(Func<string, string> nextSeedGenerator)
		{
			if(!Directory.Exists(BasePath))
			{
				throw new DirectoryNotFoundException();
			}

			string newFolderName = null;
			string seed = null;
				
			string[] folders = Directory.GetDirectories(BasePath);

			folders.ToList().ForEach(subFolder =>
			{
				seed = nextSeedGenerator(seed);
				
				if (CreateFolderName(seed, out newFolderName))
				{
					Directory.Move(subFolder, newFolderName);
				}
			});
		}

		public void RenameAllFiles(Func<string, string> nextSeedGenerator)
		{
			if(!Directory.Exists(BasePath))
			{
				throw new DirectoryNotFoundException();
			}

			var files = new DirectoryInfo(BasePath).GetFiles().OrderBy(x => x.CreationTimeUtc);
			string seed = null;

			files.ToList().ForEach(file =>
			{
				seed = nextSeedGenerator(seed);
				var newFilename = CreateFileName(file, seed);

				if(File.Exists(newFilename))
				{
					var tempFileName = CreateFileName(file, Guid.NewGuid().ToString());
					File.Move(newFilename, tempFileName);
				}

				File.Move(
					file.FullName,
					newFilename);
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
