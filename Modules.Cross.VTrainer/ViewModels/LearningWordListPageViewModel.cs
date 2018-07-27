using CrossUtils.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using VTrainerCommon.Models;

namespace VTrainerCommon.ViewModels
{
    public class LearningWordListPageViewModel : BaseViewModel
    {
		public void LoadViewModel(string username, string bookFilename, string unitID) {
			Book book = VTrainerModule.Default.BookController.GetBook(username, bookFilename);
			this.BookFilename = bookFilename;
			this.BookImageFilePath = Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, bookFilename + ".png");
			this.BookCaption = book?.Caption;
			this.Username = username;
			this.UnitID = unitID;
			this.Lang1 = book.GetLang1FullCaption();
			this.Lang2 = book.GetLang2FullCaption();

			Words.Clear();
			IEnumerable<Word> words = VTrainerModule.Default.WordController.GetWords(username, bookFilename, unitID);
			if (words == null)
				return;
			foreach (Word word in words) {
				LearningWordListPageItemViewModel vm = new LearningWordListPageItemViewModel();
				vm.LoadViewModel(word);
				Words.Add(vm);
			}
		}

		// *** Buchinformationen *** //
		public string BookFilename { get; private set; }
		public string BookImageFilePath { get; private set; }
		public string BookCaption { get; private set; }
		public string Username { get; private set; }
		public string UnitID { get; private set; }
		public string Lang1 { get; private set; }
		public string Lang2 { get; private set; }

		// *** Word-Liste *** //
		public BindingList<LearningWordListPageItemViewModel> Words = new BindingList<LearningWordListPageItemViewModel>();
	}
}
