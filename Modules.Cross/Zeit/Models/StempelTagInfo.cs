using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Cross.Zeit.Models
{
    public class StempelTagInfo
    {
		public int IDKontakt { get; set; }
		public string Name { get; set; }
		public DateTime Datum { get; set; }
		public int DayOfWeek { get; set; }
		public bool IsWochenende { get; set; }
		public bool IsFeiertag { get; set; }
		public decimal Stunden { get; set; }
		public decimal Mehrstunden { get; set; }
		public decimal PausenzeitGestempelt { get; set; }
		public decimal AnwesenheitGestempelt { get; set; }
		public decimal Zeitgutschrift { get; set; }
		public bool Processed { get; set; }
		public string StempelzeitenAsString { get; set; }
		public string PausenabzuegeAsString { get; set; }
		public decimal Pausenabzuege { get; set; }
		public bool Error { get; set; }
		public string ErrorString { get; set; }
    }
}
