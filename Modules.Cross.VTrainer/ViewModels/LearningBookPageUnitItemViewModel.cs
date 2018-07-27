using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using VTrainerCommon.Models;
using CrossUtils.Mvvm;

namespace VTrainerCommon.ViewModels
{
    public class LearningBookPageUnitItemViewModel : BaseViewModel
    {
		private Unit _Unit;
		public void LoadViewModel(Unit unit, Action<Unit> callbackOnActiveChanged) {
			_CallbackOnActiveChanged = callbackOnActiveChanged;
			_Unit = unit;
			xIsActive = _Unit.IsActive;
			xUnitCaption = _Unit.Caption;
			xPercentLearned = _Unit.PercentLearned;
			xWordsPhase0 = _Unit.WordsPhase0;
			xWordsPhase1 = _Unit.WordsPhase1;
			xWordsPhase2 = _Unit.WordsPhase2;
			xWordsPhase3 = _Unit.WordsPhase3;
			xWordsPhase4 = _Unit.WordsPhase4;
			xWordsPhase5 = _Unit.WordsPhase5;
			xWordsPhase6 = _Unit.WordsPhase6;
		}

		private Action<Unit> _CallbackOnActiveChanged;

		private bool xIsActive = false;
		public bool IsActive {
			get { return this.xIsActive; }
			set {
				if (xIsActive != value) {
					xIsActive = value;
					_Unit.IsActive = value;
					NotifyPropertyChanged();
					_CallbackOnActiveChanged?.Invoke(this._Unit);
				}
			}
		}


		private string xUnitCaption = "";
		public string UnitCaption {
			get { return this.xUnitCaption; }
			set {
				if (xUnitCaption != value) {
					xUnitCaption = value;
					NotifyPropertyChanged();
				}
			}
		}

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

		private int xWordsPhase0 = 0;
		public int WordsPhase0 {
			get { return this.xWordsPhase0; }
			set {
				if (xWordsPhase0 != value) {
					xWordsPhase0 = value;
					NotifyPropertyChanged();
				}
			}
		}
		
		public Color ColorPhase0 { get { return VTrainerResourceSettings.ColorPhase0; } }

		private int xWordsPhase1 = 0;
		public int WordsPhase1 {
			get { return this.xWordsPhase1; }
			set {
				if (xWordsPhase1 != value) {
					xWordsPhase1 = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Color ColorPhase1 { get { return VTrainerResourceSettings.ColorPhase1; } }

		private int xWordsPhase2 = 0;
		public int WordsPhase2 {
			get { return this.xWordsPhase2; }
			set {
				if (xWordsPhase2 != value) {
					xWordsPhase2 = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Color ColorPhase2 { get { return VTrainerResourceSettings.ColorPhase2; } }

		private int xWordsPhase3 = 0;
		public int WordsPhase3 {
			get { return this.xWordsPhase3; }
			set {
				if (xWordsPhase3 != value) {
					xWordsPhase3 = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Color ColorPhase3 { get { return VTrainerResourceSettings.ColorPhase3; } }

		private int xWordsPhase4 = 0;
		public int WordsPhase4 {
			get { return this.xWordsPhase4; }
			set {
				if (xWordsPhase4 != value) {
					xWordsPhase4 = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Color ColorPhase4 { get { return VTrainerResourceSettings.ColorPhase4; } }

		private int xWordsPhase5 = 0;
		public int WordsPhase5 {
			get { return this.xWordsPhase5; }
			set {
				if (xWordsPhase5 != value) {
					xWordsPhase5 = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Color ColorPhase5 { get { return VTrainerResourceSettings.ColorPhase5; } }

		private int xWordsPhase6 = 0;
		public int WordsPhase6 {
			get { return this.xWordsPhase6; }
			set {
				if (xWordsPhase6 != value) {
					xWordsPhase6 = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Color ColorPhase6 { get { return VTrainerResourceSettings.ColorPhase6; } }
	}
}
