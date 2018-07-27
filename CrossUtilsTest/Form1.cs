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
			JsonCommand cmd = DataFoxServiceCommands.CreateGetStempelzeitenCommand(45310, DateTime.Today.AddDays(-3), DateTime.Today.MaximizeTimePart(), true);
			JsonCommandRetValue retval = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			this._Ausgabe.Text = retval.ReturnCode + "\r\n" + retval.ReturnMessage + "\r\n" + retval.ReturnValue;
			
		}
	}
}
