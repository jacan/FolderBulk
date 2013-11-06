using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderRename.Model
{
	public interface IRenameDiskFileAndFolders
	{
		string BasePath { get; set; }

		void RenameAll(Func<string, string> renameSeed);
	}
}
