using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using VTrainerCommon.Models;
using VTrainerCommon.ViewModels;

namespace VTrainerCommon.Controller {
	public class BookController {
		public Book CurrentBook;
		public BookController() {

		}

		public Tuple<bool, string, List<BookViewModel>> GetRemoteBookList() {
			try {
				// Auf jedem Server ist die FileExtension .zip, da die meisten Webserver mit anderen Extensions Probleme bereiten
				WebClient wc = new WebClient();
				string bookList = wc.DownloadString("http://www.vtrainer.eu/books/booklist.json");
				List<BookViewModel> bookArr = JsonConvert.DeserializeObject<List<BookViewModel>>(bookList);
				bookArr.ForEach(x => x.ThumbUrl = "http://www.vtrainer.eu/books/" + x.BookID + ".png");
				return new Tuple<bool, string, List<BookViewModel>>(true, "", bookArr);
			} catch (Exception ex) {
				return new Tuple<bool, string, List<BookViewModel>>(false, ex.Message, null);
			}
			// return new Tuple<bool, string>(false, "Buch erfolgreich heruntergeladen");
		}
		public Tuple<bool, string, List<BookViewModel>> GetLocalBookList() {
			try {
				string path = Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, "booklist.json");
				if (!File.Exists(path))
					return new Tuple<bool, string, List<BookViewModel>>(false, "Keine lokalen Bücher installiert!", null);
				string bookList = CrossUtils.IO.IOUtil.StringFromFile(path, Encoding.UTF8);
				List<BookViewModel> bookArr = JsonConvert.DeserializeObject<List<BookViewModel>>(bookList);
				bookArr.ForEach(x => x.ThumbUrl = Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, x.BookID + ".png"));
				return new Tuple<bool, string, List<BookViewModel>>(true, "", bookArr);
			} catch (Exception ex) {
				return new Tuple<bool, string, List<BookViewModel>>(false, ex.Message, null);
			}
			// return new Tuple<bool, string>(false, "Buch erfolgreich heruntergeladen");
		}

		public Tuple<bool, string, List<BookViewModel>> GetUserBookList(string username) {
			try {
				string path = Path.Combine(VTrainerModule.Default.RootPath, username);
				string[] dbFiles = Directory.GetFiles(path, "*.db");
				if (dbFiles == null || dbFiles.Length == 0)
					return new Tuple<bool, string, List<BookViewModel>>(false, "Keine lokalen Bücher installiert", null);
				Dictionary<string, BookViewModel> localBooksDict = new Dictionary<string, BookViewModel>();
				List<BookViewModel> localBooks = GetLocalBookList().Item3;
				if (localBooks == null)
					return new Tuple<bool, string, List<BookViewModel>>(false, "Keine lokalen Bücher installiert", null);
				localBooks.ForEach(x => localBooksDict.Add(x.BookID, x));
				List<BookViewModel> userBookViewModels = new List<BookViewModel>();
				foreach (string dbFile in dbFiles) {
					// für jedes dbFile nun eine BookViewModel erzeugen
					string name = Path.GetFileNameWithoutExtension(dbFile);
					if (localBooksDict.ContainsKey(name)) {
						BookViewModel origBVM = localBooksDict[name];
						BookViewModel bvm = new BookViewModel();
						bvm.FromBookViewModel(origBVM);
						userBookViewModels.Add(bvm);
					}
				}
				return new Tuple<bool, string, List<BookViewModel>>(true, "", userBookViewModels);
			} catch (Exception ex) {
				return new Tuple<bool, string, List<BookViewModel>>(false, ex.Message, null);
			}
			// return new Tuple<bool, string>(false, "Buch erfolgreich heruntergeladen");
		}


		public List<BookItemViewModel> GetInstalledBooksFiles() {
			string[] books = Directory.GetFiles(Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath), "*.vbook");
			List<BookItemViewModel> retval = new List<BookItemViewModel>();
			foreach (string book in books) {
				BookItemViewModel vm = new BookItemViewModel();
				vm.FileName = Path.GetFileName(book);
				vm.FilePath = book;
				vm.ImageFilePath = book + ".png";
				retval.Add(vm);
			}

			return retval;
		}

		public Book GetBook(string username, string bookID) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookID);
			using (var db = new LiteDatabase(bookUserDbFullPath)) {
				var tableBooks = db.GetCollection<Book>("books");
				return tableBooks.FindOne(x => x.BookID == bookID);
			}
		}

		private Dictionary<string, BookViewModel> GetRemoteBookDictionary() {
			Dictionary<string, BookViewModel> remoteBooksDic = new Dictionary<string, BookViewModel>();
			List<BookViewModel> remoteBooks = GetRemoteBookList().Item3;
			if (remoteBooks == null)
				return remoteBooksDic;
			remoteBooks.ForEach(x => remoteBooksDic.Add(x.BookID, x));
			return remoteBooksDic;
		}

		public Tuple<bool, string> DownloadBook(string bookID) {
			try {
				// Auf jedem Server ist die FileExtension .zip, da die meisten Webserver mit anderen Extensions Probleme bereiten
				WebClient wc = new WebClient();
				wc.DownloadFile("http://www.vtrainer.eu/books/" + bookID.Replace(".vbook", ".zip"), VTrainerModule.Default.GetBookFullPath(bookID));
				wc.DownloadFile("http://www.vtrainer.eu/books/" + bookID + ".png", VTrainerModule.Default.GetBookFullPath(bookID) + ".png");
				// Sicherstellen, daß die lokale Buchliste auch aktuell ist
				Dictionary<string, BookViewModel> remoteBooksDic = this.GetRemoteBookDictionary();
				List<BookViewModel> localBooks = this.GetLocalBookList().Item3;
				bool updated = false;
				if (localBooks == null) {
					localBooks = new List<BookViewModel>();
					localBooks.Add(remoteBooksDic[bookID]);
					updated = true;
				} else {
					foreach(BookViewModel bvm in localBooks) {
						if (bvm.BookID == bookID) {
							bvm.FromBookViewModel(remoteBooksDic[bookID]);
							updated = true;
						}
					}
				}
				if (!updated)
					localBooks.Add(remoteBooksDic[bookID]);
				string path = Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, "booklist.json");
				CrossUtils.IO.IOUtil.StringToFile(path, JsonConvert.SerializeObject(localBooks), Encoding.UTF8);
			} catch (Exception ex) {
				return new Tuple<bool, string>(false, ex.Message);
			}
			return new Tuple<bool, string>(true, "Buch erfolgreich heruntergeladen");
		}
		/// <summary>
		/// Löscht ein lokal installiertes Buch (geht aber nur, wenn es von keinem Benutzer mehr referenziert wird
		/// </summary>
		/// <param name="bookID"></param>
		/// <returns></returns>
		public Tuple<bool, string> DeleteLocalBook(string bookID) {
			List<string> users = VTrainerModule.Default.UserController.GetUsers();
			foreach(string user in users) {
				string path = VTrainerModule.Default.UserController.GetUserFullPath(user);
				string pathDb = Path.Combine(path, bookID + ".db");
				if (File.Exists(pathDb))
					return new Tuple<bool, string>(false, $"Buch kann nicht gelöscht werden, da es von '{user}' noch in Gebrauch ist!");
			}
			// Buch wird von niemand referenziert, dann darf es gelöscht werden
			string bookPath = VTrainerModule.Default.GetBookFullPath(bookID);
			if (File.Exists(bookPath))
				File.Delete(bookPath);
			string bookThumbPath = bookPath + ".png";
			if (File.Exists(bookThumbPath))
				File.Delete(bookThumbPath);
			// jetzt noch die booklist updaten
			List<BookViewModel> localBookList = this.GetLocalBookList().Item3;
			if (localBookList == null)
				return new Tuple<bool, string>(true, "");
			int foundIndex = -1;
			for (int i = 0; i < localBookList.Count; i++) {
				if (localBookList[i].BookID == bookID) {
					foundIndex = i;
					break;
				}
			}
			if (foundIndex >= 0)
				localBookList.RemoveAt(foundIndex);
			// Liste wieder speichern
			string path2 = Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, "booklist.json");
			CrossUtils.IO.IOUtil.StringToFile(path2, JsonConvert.SerializeObject(localBookList), Encoding.UTF8);
			return new Tuple<bool, string>(true, "");
		}

		public Tuple<bool, string> DeleteUserBookDatabase(string username, string bookID) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookID);
			if (!File.Exists(bookUserDbFullPath))
				return new Tuple<bool, string>(false, $"Buch '{bookID}' wurde nicht gefunden!");
			File.Delete(bookUserDbFullPath);
			return new Tuple<bool, string>(true, $"Buch '{bookID}' wurde erfolgreich gelöscht!");
		}

		/// <summary>
		/// Lädt die Buchdatendatei und parst sie
		/// </summary>
		/// <param name="username"></param>
		/// <param name="bookFilename"></param>
		public Tuple<bool, string> CreateOrUpdateBookDatabase(string username, string bookID) {
			try {
				string bookFullPath = VTrainerModule.Default.GetBookFullPath(bookID);
				string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookID);
				if (!File.Exists(bookFullPath))
					return new Tuple<bool, string>(false, $"Buch '{bookID}' wurde nicht gefunden!");

				string[] lines = File.ReadAllLines(bookFullPath);
				if (lines.Length == 0)
					return new Tuple<bool, string>(false, "Es wurden keine Daten gefunden!");

				using (var db = new LiteDatabase(bookUserDbFullPath)) {
					var tableBooks = db.GetCollection<Book>("books");

					var tableUnits = db.GetCollection<Unit>("units");           // Tabelle "units" anlegen und holen
					tableUnits.EnsureIndex(x => x.UnitID);                          // Index erstellen
					var tableWords = db.GetCollection<Word>("words");       // Tabelle "words" anlegen und holen
					tableWords.EnsureIndex(x => x.UnitID);
					tableWords.EnsureIndex(x => x.WordID);

					Unit currentUnit = null;
					int wordCounter = 0, unitCounter = 0;
					int lastWordID = 1;
					foreach (string line in lines) {
						if (string.IsNullOrWhiteSpace(line))
							continue;       // Leerzeile
						else if (line.StartsWith("##"))
							continue;           // Kommentar
						else if (line.StartsWith("#BF")) {
							// Book-Filename
							Book book = tableBooks.FindOne(x => x.BookID == bookID);
							if (book == null) {
								book = new Book();
								book.BookID = bookID;
								tableBooks.Insert(book);
							}
							continue;
						} else if (line.StartsWith("#BN")) {
							// Name des Buchs
							Book book = tableBooks.FindOne(x => x.BookID == bookID);
							if (book != null) {
								book.Caption = line.Substring(4).Trim();
								tableBooks.Update(book);
							}
							continue;
						} else if (line.StartsWith("#BL")) {
							// Name des Buchs
							Book book = tableBooks.FindOne(x => x.BookID == bookID);
							if (book != null) {
								string[] lang = line.Substring(3).Split(";".ToCharArray());
								book.Lang1 = lang[0];
								book.Lang2 = lang[1];
								tableBooks.Update(book);
							}
							continue;
						} else if (line.StartsWith("#BV")) {
							// Buchversion
							Book book = tableBooks.FindOne(x => x.BookID == bookID);
							if (book != null) {
								book.Version = line.Substring(4).Trim();
								tableBooks.Update(book);
							}
							continue;
						} else if (line.StartsWith("#U")) {
							unitCounter++;
							// Wortsammlung (Unit)
							//string[] cols = line.Split(" ".ToCharArray());
							string unitID = line.Substring(2, 4);       // cols[0].Replace("#U", "");
							string unitCaption = line.Substring(7).Trim();      // cols[1].Trim();
																				// schauen, ob die Unit in der db enthalten ist
							currentUnit = tableUnits.FindOne(x => x.UnitID == unitID);
							if (currentUnit == null) {
								currentUnit = new Unit();
								tableUnits.Insert(currentUnit);
							}
							currentUnit.UnitID = unitID;
							currentUnit.Caption = unitCaption;
							tableUnits.Update(currentUnit);
							lastWordID = 1;
						} else {
							wordCounter++;
							// Vokabelzeile
							// Wenn noch keine Unit geöffnet ist, dann weitermachen, das ist ungültig!
							if (currentUnit == null)
								continue;
							string[] cols = line.Split(";".ToCharArray());
							string lang1 = cols[0].Trim();
							string lang2 = cols[1].Trim();
							Word word = tableWords.FindOne(x => x.UnitID == currentUnit.UnitID && x.WordID == lastWordID);
							if (word == null) {
								word = new Word();
								word.UnitID = currentUnit.UnitID;
								tableWords.Insert(word);
							}
							word.WordID = lastWordID; lastWordID++;
							word.Lang1 = lang1;
							word.Lang2 = lang2;
							tableWords.Update(word);
						}
					}
					Book bookx = tableBooks.FindOne(x => x.BookID == bookID);
					if (bookx != null) {
						bookx.UnitsCount = unitCounter;
						bookx.WordsCount = wordCounter;
						tableBooks.Update(bookx);
					}
				}
			} catch (Exception ex) {
				return new Tuple<bool, string>(false, ex.Message);
			}

			return new Tuple<bool, string>(true, $"Buch '{bookID}' wurde erfolgreich eingelesen");
		}
	}
}
