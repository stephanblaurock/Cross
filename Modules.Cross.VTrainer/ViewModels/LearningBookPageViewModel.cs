using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using CrossUtils.Mvvm;
using VTrainerCommon.Models;

namespace VTrainerCommon.ViewModels
{
    public class LearningBookPageViewModel : BaseViewModel
    {

		public void LoadViewModel(string username, string bookFilename) {
			Book book = VTrainerModule.Default.BookController.GetBook(username, bookFilename);
			this.BookFilename = bookFilename;
			this.BookImageFilePath = Path.Combine(VTrainerModule.Default.RootPath, VTrainerModule.Default.BookPath, bookFilename + ".png");
			this.BookCaption = book?.Caption;
			this.Username = username;
			this.PercentLearned = book?.PercentLearned ?? 0;

			Units.Clear();
			IEnumerable<Unit> units = VTrainerModule.Default.UnitController.GetUnits(username, bookFilename);
			if (units == null)
				return;
			foreach(Unit unit in units) {
				LearningBookPageUnitItemViewModel vm = new LearningBookPageUnitItemViewModel();
				vm.LoadViewModel(unit, OnUnitIsActiveChanged);
				Units.Add(vm);
			}
		}

		private void OnUnitIsActiveChanged(Unit unit) {
			VTrainerModule.Default.UnitController.SaveUnit(this.Username, BookFilename, unit, true);
		}

		// *** Buchinformationen *** //
		public string BookFilename { get; private set; }
		public string BookImageFilePath { get; private set; }
		public string BookCaption { get; private set; }
		public string Username { get; private set; }

		private int xPercentLearned = 0;
		public int PercentLearned {
			get { return this.xPercentLearned; }
			set {
				if (xPercentLearned != value) {
					xPercentLearned = value;
					NotifyPropertyChanged();
				}
			}
		}



		// *** Unit-Liste *** //
		public ObservableCollection<LearningBookPageUnitItemViewModel> Units = new ObservableCollection<LearningBookPageUnitItemViewModel>();
    }

}
