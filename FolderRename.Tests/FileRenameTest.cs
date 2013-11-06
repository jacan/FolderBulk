using System;
using Xunit;
using FluentAssertions;
using FolderRename.Model;
using System.IO;

namespace FolderRename.Tests
{
	public class FileRenameTest : IUseFixture<FileFixture>
	{
		private FileFixture _fixture;

		[Fact]
		public void WhenRenamingFilesExceptionIsThrownIfDirectoryDoesNotExist()
		{
			var badDirectory = Path.Combine(_fixture.TestDirectory, Guid.NewGuid().ToString());
			var folderRename = new RenameFolders(badDirectory);

			folderRename
				.Invoking(invoke => invoke.RenameAll((x) => "10"))
				.ShouldThrow<DirectoryNotFoundException>();
		}

		[Fact]
		public void WhenRenamingFilesThenFilesAreRenamedBasedOnSeed()
		{
			CreateTestFileSet();

			Func<string, string> seedGenerator = (x) =>
			{
				var existingX = Convert.ToInt32(x);

				return (++existingX).ToString();
			};
			var fileRename = new RenameFolders(_fixture.TestDirectory);

			fileRename.RenameAll(seedGenerator);

			_fixture.FileExists("1.txt").Should().BeTrue();
			_fixture.FileExists("2.txt").Should().BeTrue();
		}

		private void CreateTestFileSet()
		{
			_fixture.CreateFile("TestFile1.txt");
			_fixture.CreateFile("TestFile2.txt");
		}

		public void SetFixture(FileFixture fixture)
		{
			_fixture = fixture;
		}
	}
}
