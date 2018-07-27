using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using VTrainerCommon.Models;

namespace VTrainerCommon.Controller
{
    public class WordController
    {
		public IEnumerable<Word> GetWordsTable(string username, string bookFilename) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookFilename);
			using (var db = new LiteDatabase(bookUserDbFullPath)) {
				var tableWords= db.GetCollection<Word>("words");
				return tableWords.FindAll();
			}
		}
		public void SaveWords(string username, string bookFilename, List<Word> words) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookFilename);
			using (var db = new LiteDatabase(bookUserDbFullPath)) {
				var tableWords = db.GetCollection<Word>("words");
				tableWords.Update(words);
			}
		}

		public List<Word> GetWords(string username, string bookFilename, string unitID) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookFilename);
			using (var db = new LiteDatabase(bookUserDbFullPath)) {
				var tableWords = db.GetCollection<Models.Word>("words");
				IEnumerable<Word> words = tableWords.Find(x => x.UnitID == unitID);
				List<Word> retval = new List<Word>(words);
				return retval;
			}
		}
	}
}
