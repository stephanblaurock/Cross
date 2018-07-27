using System;
using System.Collections.Generic;
using System.Text;

namespace VTrainerCommon.Models {
	public class History {
		public int Id { get; set; }
		public string UnitID { get; set; }
		public DateTime TimeStamp { get; set; }
		public int WordsCount { get; set; }
		public int WordsSuccess { get; set; }
		public int WordsFailed { get; set; }

	}
}
