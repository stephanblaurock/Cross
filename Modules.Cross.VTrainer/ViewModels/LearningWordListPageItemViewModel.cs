using CrossUtils.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using VTrainerCommon.Models;
using CrossUtils.Extensions;

namespace VTrainerCommon.ViewModels
{
    public class LearningWordListPageItemViewModel : BaseViewModel
    {
		public Word Word { get; private set; }

		public void LoadViewModel(Word word) {
			Word = word;
			this.UnitID = Word.UnitID;
			this.WordID = Word.WordID;
			this.Lang1 = Word.Lang1;
			this.Lang2 = Word.Lang2;
			this.Phase = Word.Phase;
			this.SuccessCounter = Word.SuccessCounter;
			this.LastSuccess = Word.LastSuccess;
			this.FailedCounter = Word.FailedCounter;
			this.LastFailed = Word.LastFailed;
		}


		private string xUnitID = "";
		public string UnitID {
			get { return this.xUnitID; }
			set {
				if (xUnitID != value) {
					xUnitID = value;
					NotifyPropertyChanged();
				}
			}
		}


		private int xWordID = 0;
		public int WordID {
			get { return this.xWordID; }
			set {
				if (xWordID != value) {
					xWordID = value;
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



		private int xPhase = 0;
		public int Phase {
			get { return this.xPhase; }
			set {
				if (xPhase != value) {
					xPhase = value;
					NotifyPropertyChanged();
				}
			}
		}


		private int xSuccessCounter = 0;
		public int SuccessCounter {
			get { return this.xSuccessCounter; }
			set {
				if (xSuccessCounter != value) {
					xSuccessCounter = value;
					NotifyPropertyChanged();
				}
			}
		}


		private DateTime xLastSuccess = DateExtensions.DefaultTime;
		public DateTime LastSuccess {
			get { return this.xLastSuccess; }
			set {
				if (xLastSuccess != value) {
					xLastSuccess = value;
					NotifyPropertyChanged();
				}
			}
		}


		private int xFailedCounter = 0;
		public int FailedCounter {
			get { return this.xFailedCounter; }
			set {
				if (xFailedCounter != value) {
					xFailedCounter = value;
					NotifyPropertyChanged();
				}
			}
		}

		private DateTime xLastFailed = DateExtensions.DefaultTime;
		public DateTime LastFailed {
			get { return this.xLastFailed; }
			set {
				if (xLastFailed != value) {
					xLastFailed = value;
					NotifyPropertyChanged();
				}
			}
		}


	}
}
