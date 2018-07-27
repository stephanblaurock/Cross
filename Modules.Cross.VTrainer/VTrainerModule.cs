using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VTrainerCommon.Controller;

namespace VTrainerCommon {
	public class VTrainerModule {
		public static VTrainerModule Default;
		public string RootPath = "";
		public string BookPath = "_books_";

		public UserController UserController = new UserController();
		public BookController BookController = new BookController();
		public UnitController UnitController = new UnitController();
		public WordController WordController = new WordController();
		public TestController TestController = new TestController();

		/// <summary>
		/// RootPath ist das User-Root-Verzeichnis
		/// </summary>
		/// <param name="rootPath"></param>
		public VTrainerModule(string rootPath) {
			Default = this;
			RootPath = rootPath;
			string bookPath = Path.Combine(this.RootPath, this.BookPath);
			if (!Directory.Exists(bookPath))
				Directory.CreateDirectory(bookPath);
		}

		public string GetBookFullPath(string bookFilename) {
			return Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, bookFilename);
		}
		public string GetBookFullPath() {
			return Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath);
		}
		public string GetBookUserDbFullPath(string username, string bookFilename) {
			return Path.Combine(VTrainerModule.Default.RootPath, username, bookFilename + ".db");
		}

	}
}
