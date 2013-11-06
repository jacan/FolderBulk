using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FolderRename.Model;

namespace FolderRename.Tests
{
	public class FolderRenameTest : IUseFixture<FileFixture>
	{
		private FileFixture _fixture;

		[Fact]
		public void WhenRenamingFoldersExceptionIsThrownIfDirectoryDoesNotExist()
		{
			var badDirectory = Path.Combine(_fixture.TestDirectory, Guid.NewGuid().ToString());
			var folderRename = new RenameFiles(badDirectory);

			folderRename
				.Invoking(invoke => invoke.RenameAll((x) => "10"))
				.ShouldThrow<DirectoryNotFoundException>();
		}

		[Fact]
		public void WhenRenamingFoldersThenFoldersAreRenamedBasedOnSeed()
		{
			CreateTestFolderSet();

			Func<string, string> seedGenerator = (x) =>
			{
				var existingX = Convert.ToInt32(x);

				return (++existingX).ToString();
			};
			var fileRename = new RenameFiles(_fixture.TestDirectory);

			fileRename.RenameAll(seedGenerator);

			_fixture.DirectoryExists("1").Should().BeTrue();
			_fixture.DirectoryExists("2").Should().BeTrue();
		}

		public void SetFixture(FileFixture fixture)
		{
			_fixture = fixture;
		}

		private void CreateTestFolderSet()
		{
			_fixture.CreateDirectory("TestFolder1");
			_fixture.CreateDirectory("TestFolder2");
		}
	}
}
