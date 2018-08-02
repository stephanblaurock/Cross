using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CrossUtils.Json;
using CrossUtils.Mvvm;
using Modules.Cross.Users.Models;
using Modules.Cross.Zeit.Models;
using Newtonsoft.Json;

namespace Modules.Cross.Zeit.ViewModels {
	public class StempelPageViewModel : BaseViewModel {
		public StempelPageViewModel() {
		}

		public string CurrentUserImageUrl {
			get { return "http://netlogger.eu:8992/img/cust_images/1.jpg"; }
		}

		private User _CurrentUser;
		public User CurrentUser {
			get { return _CurrentUser; }
			set {
				_CurrentUser = value;
				NotifyPropertyChanged();
			}
		}

		public async Task<(bool success, string message)> Login(string pin) {
			JsonCommand cmd = DataFoxServiceCommands.CreateGetUserByPINCommand(pin);
			JsonCommandRetValue retValue = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			if (retValue.ReturnCode == 200) {
				_CurrentUser = JsonConvert.DeserializeObject<User>(retValue.ReturnValue);
				this.RefreshStempelTagInfos();
				return (true, "Benutzer erfolgreich eingeloggt");
			}
			return (false, "Benutzer konnte nicht eingeloggt werden: " + retValue.ReturnMessage);
		}

		public void Logout() {
			this._CurrentUser = null;
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
			JsonCommand cmd = DataFoxServiceCommands.CreateGetStempelzeitenCommand(_CurrentUser.ID, DateTime.Today.AddDays(-7), DateTime.Today, true);
			JsonCommandRetValue retValue = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			if (retValue.ReturnCode == 200) {
				List<StempelTagInfo> infos = JsonConvert.DeserializeObject<List<StempelTagInfo>>(retValue.ReturnValue);
				if (infos.Count > 0)
					infos.ForEach(x => _StempelTagInfos.Add(x));
			}
		}
	}
}
