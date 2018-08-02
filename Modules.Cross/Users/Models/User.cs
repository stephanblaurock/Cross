using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Cross.Users.Models {
	public class User {
		public int ID { get; set; }
		public int UserIDGinkgo { get; set; }
		public bool IsAdmin { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string PIN { get; set; }
		public string Name { get; set; }
		public string EMail { get; set; }
		public string Telefon { get; set; }
		public string Telefax { get; set; }
		public string Mobil { get; set; }
		public string Durchwahl { get; set; }

		public bool Inactive { get; set; }
		public bool Locked { get; set; }

		public bool IsGroup { get; set; }

		public string Unterschriftszusatz { get; set; }

		public string Benutzerkennung { get; set; }
		public string Zeichen { get; set; }

		public string Signierung { get; set; }

		public string ChatToken { get; set; }
		public int ChatID { get; set; }

		public int MitarbeiterkontaktIDGinkgo { get; set; }

		public override string ToString() {
			return this.Name;
		}
	}
}
