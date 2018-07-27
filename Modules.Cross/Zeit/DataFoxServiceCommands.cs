using CrossUtils.Json;
using Modules.Cross.Zeit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Cross.Zeit
{
    public class DataFoxServiceCommands
    {
		public static JsonCommand CreateAddOrUpdateStempelzeitCommand(Stempelzeit stempelzeit) {
			JsonCommand retval = new JsonCommand("GinkgoDataFoxLib.Service.GinkgoDataFoxService", "AddOrUpdateStempelzeit");
			retval.SetParameter("Stempelzeit", JsonConvert.SerializeObject(stempelzeit));
			return retval;
		}

		public static JsonCommand CreateGetStempelzeitenCommand(int idKontakt, DateTime datum1, DateTime datum2, bool groupedByDay) {
			JsonCommand retval = new JsonCommand("GinkgoDataFoxLib.Service.GinkgoDataFoxService", "GetStempelzeiten");
			retval.SetParameter("IDKontakt", idKontakt);
			retval.SetParameter("Datum1", datum1);
			retval.SetParameter("Datum2", datum2);
			retval.SetParameter("GroupedByDay", groupedByDay);
			return retval;
		}
	}
}
