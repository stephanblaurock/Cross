using System;
using System.Collections.Generic;
using System.Text;

namespace VTrainerCommon.Models
{
    public class Word
    {
		public int Id { get; set; }
		public string UnitID { get; set; }
		public int WordID { get; set; }
		public string Lang1 { get; set; }
		public string Lang2 { get; set; }
		public int Phase { get; set; }
		public int SuccessCounter { get; set; }
		public DateTime LastSuccess { get; set; }
		public int FailedCounter { get; set; }
		public DateTime LastFailed { get; set; }
    }
}
