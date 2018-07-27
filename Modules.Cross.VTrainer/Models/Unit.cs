using System;
using System.Collections.Generic;
using System.Text;

namespace VTrainerCommon.Models
{
    public class Unit
    {
		public int Id { get; set; }
		public string UnitID { get; set; }
		public string Caption { get; set; }
		public bool IsActive { get; set; }
		public int WordsCount { get; set; }
		public int WordsPhase0 { get; set; }
		public int WordsPhase1 { get; set; }
		public int WordsPhase2 { get; set; }
		public int WordsPhase3 { get; set; }
		public int WordsPhase4 { get; set; }
		public int WordsPhase5 { get; set; }
		public int WordsPhase6 { get; set; }
		public DateTime LastLearned { get; set; }
		public int PercentLearned { get; set; }

	}
}
