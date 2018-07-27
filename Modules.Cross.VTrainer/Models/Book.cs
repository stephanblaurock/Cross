using System;
using System.Collections.Generic;
using System.Text;

namespace VTrainerCommon.Models
{
    public class Book
    {
		public int Id { get; set; }
		public string BookID { get; set; }
		public string Version { get; set; }
		public string Caption { get; set; }
		public string Lang1 { get; set; }
		public string Lang2 { get; set; }
		public int UnitsCount { get; set; }
		public int WordsCount { get; set; }
		public int PercentLearned { get; set; }

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
    }
}
