using CrossUtils.Json;
using Modules.Cross;
using Modules.Cross.Users;
using Modules.Cross.Users.Models;
using Modules.Cross.Zeit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrossUtils;
using CrossUtils.Extensions;
using Modules.Cross.Zeit.Models;

namespace CrossUtilsTest {
	public partial class Form1 : Form {
		private Modules.Cross.ModulesClientEnvironment _ClientEnvironment;
		public Form1() {
			InitializeComponent();
			string urlAussen = "http://int.graule-technik.de:8075";
			_ClientEnvironment = new Modules.Cross.ModulesClientEnvironment("http://AS1:8077", "blaurock@graule-technik.de", "res");
		}

		private async void _ButtGetUser_Click(object sender, EventArgs e) {
			User usr = await UserModule.Default.LoadCurrentUser();
			if (usr != null) {
				this._Ausgabe.Text += JsonConvert.SerializeObject(usr);
			}
	
		}

		private async void _ButtGetStempelzeiten_Click(object sender, EventArgs e) {
			JsonCommand cmd = DataFoxServiceCommands.CreateGetStempelzeitenCommand(45310, DateTime.Today.AddDays(-7), DateTime.Today.MaximizeTimePart(), true);
			JsonCommandRetValue retval = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			this._Ausgabe.Text = retval.ReturnCode + "\r\n" + retval.ReturnMessage + "\r\n" + retval.ReturnValue;
			
		}

		private async void _ButtAddStempelzeiten_Click(object sender, EventArgs e) {
			Stempelzeit szeit = new Stempelzeit();
			szeit.IDKontakt = 45310;
			szeit.Datum = new DateTime(2018, 8, 3, 8, 2, 13);	// DateTime.Now;
			szeit.UserIDCreated = 2;
			szeit.Grund = "K";
			szeit.Manual = true;
			szeit.ShouldDeleted = true;
			
			JsonCommand cmd = DataFoxServiceCommands.CreateAddOrUpdateStempelzeitCommand(szeit);
			JsonCommandRetValue retval = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			this._Ausgabe.Text = retval.ReturnCode + "\r\n" + retval.ReturnMessage + "\r\n" + retval.ReturnValue;
		}

		private async void _ButtLoginUser_Click(object sender, EventArgs e) {
			JsonCommand cmd = DataFoxServiceCommands.CreateGetUserByPINCommand("01055854");
			JsonCommandRetValue retval = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			this._Ausgabe.Text = retval.ReturnCode + "\r\n" + retval.ReturnMessage + "\r\n" + retval.ReturnValue;
			User usr = JsonConvert.DeserializeObject<User>(retval.ReturnValue);

			this._Ausgabe.Text += "\r\nUrl: " + UserModule.Default.GetUserImageUrl(usr.ID);

			this.pictureEdit1.LoadAsync(UserModule.Default.GetUserImageUrl(usr.ID));
		}
	}
}
