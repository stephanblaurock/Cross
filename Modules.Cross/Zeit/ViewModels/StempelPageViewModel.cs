using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CrossUtils.Json;
using CrossUtils.Mvvm;
using Modules.Cross.Users.Models;
using Modules.Cross.Zeit.Models;
using Newtonsoft.Json;
using Modules.Cross.Users;

namespace Modules.Cross.Zeit.ViewModels {
	public class StempelPageViewModel : BaseViewModel {
		public StempelPageViewModel() {
			CurrentUserImageUrl = UserModule.Default.GetUserImageUrl(0);
		}

		private string _CurrentUserImageUrl = "";
		public string CurrentUserImageUrl {
			get { return _CurrentUserImageUrl; }
			set { _CurrentUserImageUrl = value; NotifyPropertyChanged("CurrentUserImageUrl"); }
		}

		private string _LabelNameText = "BITTE EINLOGGEN!";
		public string LabelNameText {
			get { return _LabelNameText; }
			set { _LabelNameText = value; NotifyPropertyChanged("LabelNameText"); }
		}

		private bool _LoginTextboxVisible = true;
		public bool LoginTextboxVisible {
			get { return _LoginTextboxVisible; }
			set { _LoginTextboxVisible = value; NotifyPropertyChanged("LoginTextboxVisible"); }
		}

		private string _LoginButtonText = "LOGIN";
		public string LoginButtonText {
			get { return _LoginButtonText; }
			set { _LoginButtonText = value; NotifyPropertyChanged("LoginButtonText"); }
		}

		private bool _ButtKommenGehenVisible = false;
		public bool ButtKommenGehenVisible {
			get { return _ButtKommenGehenVisible; }
			set { _ButtKommenGehenVisible = value; NotifyPropertyChanged("ButtKommenGehenVisible"); }
		}

		private bool _ListViewStempelzeitenVisible = false;
		public bool ListViewStempelzeitenVisible {
			get { return _ListViewStempelzeitenVisible; }
			set { _ListViewStempelzeitenVisible = value; NotifyPropertyChanged("ListViewStempelzeitenVisible"); }
		}


		private User _CurrentUser;
		public User CurrentUser {
			get { return _CurrentUser; }
			set {
				_CurrentUser = value;
				NotifyPropertyChanged();
				if (_CurrentUser == null) {
					this.LabelNameText = "BITTE EINLOGGEN";
					this.LoginTextboxVisible = true;
					this.LoginButtonText = "LOGIN";
					this.ButtKommenGehenVisible = false;
					this.ListViewStempelzeitenVisible = false;
					CurrentUserImageUrl = UserModule.Default.GetUserImageUrl(0);
					NotifyPropertyChanged("CurrentUserImageUrl");
				} else {
					this.LabelNameText = this._CurrentUser.Name;
					this.LoginTextboxVisible = false;
					this.LoginButtonText = "LOGOUT";
					this.ButtKommenGehenVisible = true;
					CurrentUserImageUrl = UserModule.Default.GetUserImageUrl(_CurrentUser.ID);
					NotifyPropertyChanged("CurrentUserImageUrl");
				}
			}
		}

		public async Task<(bool success, string message)> Login(string pin) {
			JsonCommand cmd = DataFoxServiceCommands.CreateGetUserByPINCommand(pin);
			// string cmdUrl = ModulesClientEnvironment.Default.JsonCommandClient.BuildCommandUrl(cmd);
			JsonCommandRetValue retValue = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			if (retValue.ReturnCode == 200) {
				CurrentUser = JsonConvert.DeserializeObject<User>(retValue.ReturnValue);
				this.RefreshStempelTagInfos();
				return (true, "Benutzer erfolgreich eingeloggt");
			}
			return (false, "Benutzer konnte nicht eingeloggt werden: " + retValue.ReturnMessage);
		}

		public void Logout() {
			this.CurrentUser = null;
			this._StempelTagInfos.Clear();

		}

		public async Task<(bool success, string message)> AddStempelzeit(bool kommend) {
			if (_CurrentUser == null)
				return (false, "Es ist kein Benutzer eingeloggt!");
			Stempelzeit sz = new Stempelzeit();
			sz.IDKontakt = _CurrentUser.MitarbeiterkontaktIDGinkgo;
			sz.Datum = DateTime.Now;
			sz.Grund = kommend ? "K" : "G";
			sz.Manual = false;
			sz.UserIDCreated = _CurrentUser.ID;
			sz.TimeCreated = DateTime.Now;
			JsonCommand cmd = DataFoxServiceCommands.CreateAddOrUpdateStempelzeitCommand(sz);
			JsonCommandRetValue retValue = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			if (retValue.ReturnCode == 200) {
				// in diesem Fall die Stempeltag-Tabelle updaten
				this.RefreshStempelTagInfos();
				return (true, retValue.ReturnMessage);
			}
			return (false, retValue.ReturnMessage);
		}

		private ObservableCollection<StempelTagInfo> _StempelTagInfos = new ObservableCollection<StempelTagInfo>();
		public ObservableCollection<StempelTagInfo> StempelTagInfos {
			get { return _StempelTagInfos; }
			set { _StempelTagInfos = value; NotifyPropertyChanged(); }
		}

		public async void RefreshStempelTagInfos() {
			this.StempelTagInfos.Clear();
			this.ListViewStempelzeitenVisible = true;
			JsonCommand cmd = DataFoxServiceCommands.CreateGetStempelzeitenCommand(_CurrentUser.MitarbeiterkontaktIDGinkgo, DateTime.Today.AddDays(-7), DateTime.Today, true);
			JsonCommandRetValue retValue = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			if (retValue.ReturnCode == 200) {
				List<StempelTagInfo> infos = JsonConvert.DeserializeObject<List<StempelTagInfo>>(retValue.ReturnValue);
				if (infos.Count > 0)
					infos.ForEach(x => _StempelTagInfos.Add(x));
			}
		}
	}
}
