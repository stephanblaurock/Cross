using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using VTrainerCommon.Models;

namespace VTrainerCommon.Controller
{
    public class UnitController
    {
		public IEnumerable<Unit> GetUnits(string username, string bookFilename) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookFilename);
			using (var db = new LiteDatabase(bookUserDbFullPath)) {
				var tableUnits = db.GetCollection<Unit>("units");
				return tableUnits.FindAll();
			}
		}
		public void SaveUnit(string username, string bookFilename, Unit unit, bool updateUnitStatistik) {
			string bookUserDbFullPath = VTrainerModule.Default.GetBookUserDbFullPath(username, bookFilename);
			using (var db = new LiteDatabase(bookUserDbFullPath)) {
				var tableUnits = db.GetCollection<Unit>("units");
				if (updateUnitStatistik)
					this.CalculateUnitStatistik(db, unit);
				tableUnits.Update(unit);
			}
		}
		public void CalculateUnitStatistik(LiteDatabase db, Unit unit) {
			var tableWords = db.GetCollection<Models.Word>("words");
			IEnumerable<Word> words = tableWords.Find(x => x.UnitID == unit.UnitID);

			unit.WordsCount = 0;
			unit.WordsPhase0 = 0; unit.WordsPhase1 = 0; unit.WordsPhase2 = 0; unit.WordsPhase3 = 0; unit.WordsPhase4 = 0; unit.WordsPhase5 = 0; unit.WordsPhase6 = 0;
			foreach (Word word in words) {
				unit.WordsCount++;
				if (word.Phase == 0) unit.WordsPhase0++;
				else if (word.Phase == 1) unit.WordsPhase1++;
				else if (word.Phase == 2) unit.WordsPhase2++;
				else if (word.Phase == 3) unit.WordsPhase3++;
				else if (word.Phase == 4) unit.WordsPhase4++;
				else if (word.Phase == 5) unit.WordsPhase5++;
				else if (word.Phase == 6) unit.WordsPhase6++;
			}
			int points = 0;
			points += unit.WordsPhase1 * 1;
			points += unit.WordsPhase2 * 2;
			points += unit.WordsPhase3 * 3;
			points += unit.WordsPhase4 * 4;
			points += unit.WordsPhase5 * 5;
			points += unit.WordsPhase6 * 6;
			int gesamtPoints = unit.WordsCount * 6;
			unit.PercentLearned = gesamtPoints > 0 ? points / gesamtPoints : 0;
		}
	}
}
