using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VTrainerCommon.Controller
{
    public class UserController
    {

		public string GetUserFullPath(string username) {
			return Path.Combine(VTrainerModule.Default.RootPath, username);
		}
		/// <summary>
		/// erzeugt einen neuen Benutzer. Dieser besteht nur aus einem Namen, der gleichzeitig dann auch der Verzeichnisname ist
		/// </summary>
		/// <param name="caption"></param>
		public string AddUser(string caption) {
			caption = caption.Replace(" ", "");
			string fullpath = Path.Combine(VTrainerModule.Default.RootPath, caption);
			if (!Directory.Exists(fullpath)) {
				Directory.CreateDirectory(fullpath);
			}
			return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		}

		public List<string> GetUsers() {
			string[] dirs = Directory.GetDirectories(VTrainerModule.Default.RootPath);
			List<string> retval = new List<string>();
			foreach (string dir in dirs) {
				string filename = Path.GetFileName(dir);
				if (!filename.StartsWith("_"))
					retval.Add(filename);
			}
			return retval;
		}
	}
}
