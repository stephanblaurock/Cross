using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Cross.Zeit.Models {
	public class Stempelzeit {
		public int IDKontakt { get; set; }
		public string Grund { get; set; }
		public DateTime Datum { get; set; }
		public bool Processed { get; set; }
		public bool Manual { get; set; }
		public bool ManualAccepted { get; set; }
		public int ManualAcceptedUserID { get; set; }
		public float GeoLat { get; set; }
		public float GeoLng { get; set; }
		public DateTime TimeCreated { get; set; }
		public int UserIDCreated { get; set; }
		public bool ShouldDeleted { get; set; }

	}
}
