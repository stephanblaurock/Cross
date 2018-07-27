using System;
using System.Collections.Generic;
using System.Text;

namespace VTrainerCommon.Models
{
    public class Test
    {
		public int Id { get; set; }
		public DateTime Timestamp { get; set; }
		public string UnitIDs { get; set; }
		public int WordsCount { get; set; }
		public int WordsSuccess { get; set; }
		public int WordsFailed { get; set; }
    }
}
