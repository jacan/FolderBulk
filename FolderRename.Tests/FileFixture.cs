using System;
using System.IO;
using System.Reflection;

namespace FolderRename.Tests
{
	public class FileFixture : IDisposable
	{
		public string TestDirectory
		{
			get;
			set;
		}

		public bool DeleteFilesOnTeardown
		{
			get;
			set;
		}

		public FileFixture()
		{
			var testDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), string.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyyHHmm"), Guid.NewGuid().ToString()));
			Directory.CreateDirectory(testDirectory);

			TestDirectory = testDirectory;
			DeleteFilesOnTeardown = true;
		}

		public void CreateDirectory(string name)
		{
			Directory.CreateDirectory(Path.Combine(TestDirectory, name));
		}

		public void CreateFile(string name)
		{
			using (File.Create(Path.Combine(TestDirectory, name))) { }
		}

		public bool FileExists(string directoryName)
		{
			return File.Exists(Path.Combine(TestDirectory, directoryName));
		}

		public bool DirectoryExists(string fileName)
		{
			return Directory.Exists(Path.Combine(TestDirectory, fileName));
		}

		public void Dispose()
		{
			if(DeleteFilesOnTeardown)
			{
				Directory.Delete(TestDirectory, true);
			}
		}
	}
}
