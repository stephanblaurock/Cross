using System;
using VTrainerCommon.Models;
using CrossUtils.Mvvm;

namespace VTrainerCommon.ViewModels {
	public class BookViewModel : BaseViewModel {
		public BookViewModel() {
		}


		private string xBookID = "";
		public string BookID {
			get { return this.xBookID; }
			set {
				if (xBookID != value) {
					xBookID = value;
					NotifyPropertyChanged();
				}
			}
		}


		private string xThumbUrl = "";
		public string ThumbUrl {
			get { return this.xThumbUrl; }
			set {
				if (xThumbUrl != value) {
					xThumbUrl = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string xVersion = "";
		public string Version {
			get { return this.xVersion; }
			set {
				if (xVersion != value) {
					xVersion = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string xCaption = "";
		public string Caption {
			get { return this.xCaption; }
			set {
				if (xCaption != value) {
					xCaption = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string xLang1 = "";
		public string Lang1 {
			get { return this.xLang1; }
			set {
				if (xLang1 != value) {
					xLang1 = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string xLang2 = "";
		public string Lang2 {
			get { return this.xLang2; }
			set {
				if (xLang2 != value) {
					xLang2 = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int xUnitsCount = 0;
		public int UnitsCount {
			get { return this.xUnitsCount; }
			set {
				if (xUnitsCount != value) {
					xUnitsCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int xWordsCount = 0;
		public int WordsCount {
			get { return this.xWordsCount; }
			set {
				if (xWordsCount != value) {
					xWordsCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string GetLang1FullCaption() {
			return GetLangFullCaption(this.Lang1);
		}
		public string GetLang2FullCaption() {
			return GetLangFullCaption(this.Lang2);
		}

		public string GetLangFullCaption(string lang) {
			if (lang == "DE")
				return "Deutsch";
			if (lang == "EN")
				return "Englisch";
			if (lang == "FR")
				return "Französisch";
			return "";
		}

		public void FromBook(Book book) {
			this.BookID = book.BookID;
			this.Version = book.Version;
			this.Caption = book.Caption;
			this.Lang1 = book.Lang1;
			this.Lang2 = book.Lang2;
			this.UnitsCount = book.UnitsCount;
			this.WordsCount = book.WordsCount;
		}
		public void FromBookViewModel(BookViewModel book) {
			this.BookID = book.BookID;
			this.Version = book.Version;
			this.Caption = book.Caption;
			this.Lang1 = book.Lang1;
			this.Lang2 = book.Lang2;
			this.UnitsCount = book.UnitsCount;
			this.WordsCount = book.WordsCount;
		}
	}
}
