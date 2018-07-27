using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CrossUtils.Extensions;

namespace CrossUtils.Extensions
{
	public static class DateExtensions {
		public static DateTime DefaultTime {
			get { return new DateTime(1800, 1, 1); }
		}

		public static int Kalenderwoche(this DateTime Datum) {
			CultureInfo CUI = CultureInfo.CurrentCulture;
			return CUI.Calendar.GetWeekOfYear(Datum, CUI.DateTimeFormat.CalendarWeekRule, CUI.DateTimeFormat.FirstDayOfWeek);
		}
		public static DateTime ResetTimePart(this DateTime dateTime) {
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		}
		public static DateTime ResetTimePartToMinute(this DateTime dateTime) {
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
		}
		public static DateTime ResetTimePartToHour(this DateTime dateTime) {
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
		}
		public static DateTime MaximizeTimePart(this DateTime dateTime) {
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
		}
		public static DateTime FirstDayOfMonth(this DateTime dateTime) {
			return new DateTime(dateTime.Year, dateTime.Month, 1);
		}
		public static DateTime LastDayOfMonth(this DateTime dateTime) {
			if (dateTime == DateTime.MaxValue)
				return dateTime;
			return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
		}
		public static DateTime LastDayOfMonth(this DateTime dateTime, bool maximizeTimePart) {
			DateTime retval = LastDayOfMonth(dateTime);
			if (maximizeTimePart)
				retval = retval.MaximizeTimePart();
			return retval;
		}
		public static DateTime FirstDayOfYear(this DateTime dateTime) {
			return new DateTime(dateTime.Year, 1, 1);
		}
		public static DateTime LastDayOfYear(this DateTime dateTime) {
			if (dateTime == DateTime.MaxValue)
				return dateTime;
			return new DateTime(dateTime.Year, 12, 1).AddMonths(1).AddDays(-1).MaximizeTimePart();
		}
		public static DateTime LastDayOfYear(this DateTime dateTime, bool maximizeTimePart) {
			DateTime retval = LastDayOfYear(dateTime);
			if (maximizeTimePart)
				retval = retval.MaximizeTimePart();
			return retval;
		}
		public static DateTime NextBusinessDay(this DateTime dateTime) {
			DateTime retval = dateTime;
			if (dateTime.IsWochenende()) {
				int dayofweek = (int)dateTime.DayOfWeek;
				if (dayofweek == 6)
					retval = retval.AddDays(2);
				else if (dayofweek == 0)
					retval = retval.AddDays(1);

			}
			if (retval.IsFeiertag())
				retval = retval.AddDays(1);
			if (retval.IsWochenende() || retval.IsFeiertag())
				retval = retval.NextBusinessDay();
			return retval;
		}
		public static string GenerateMySQLDateString(this DateTime dateTime) {
			return string.Format("{0:yyyy-MM-dd HH:mm:ss}", dateTime);
		}
		public static string GenerateMsSQLDateString(this DateTime dateTime) {
			// Beachten: das niedrigste Datum ist der 01.01.1753! Wenn ein niedrigeres angegeben wurde, dann einfach auf dieses stellen
			if (dateTime < new DateTime(1753, 1, 1))
				dateTime = new DateTime(1753, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
			return string.Format("{0:yyyyMMdd HH:mm:ss}", dateTime);
		}
		/// <summary>
		/// Gibt das Datum in Form von "dd.MM.yyyy" zurück
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToGermanDateString(this DateTime dateTime) {
			return string.Format("{0:dd.MM.yyyy}", dateTime);
		}
		/// <summary>
		/// Gibt das Datum in Form von "dd.MM.yyyy HH:mm" zurück
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToGermanDateTimeString(this DateTime dateTime) {
			return string.Format("{0:dd.MM.yyyy HH:mm}", dateTime);
		}
		/// <summary>
		/// Gibt das Datum in Form von "dd.MM.yyyy HH:mm:ss" zurück
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToGermanDateTimeSecondString(this DateTime dateTime) {
			return string.Format("{0:dd.MM.yyyy HH:mm:ss}", dateTime);
		}
		/// <summary>
		/// Gibt die Zeitangabe des Datums in Form von "HH:mm" zurück
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToGermanTimeString(this DateTime dateTime) {
			return string.Format("{0:HH:mm}", dateTime);
		}
		/// <summary>
		/// Gibt die Zeitangabe des Datums in Form von "HH:mm:ss" zurück
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToGermanTimeSecondString(this DateTime dateTime) {
			return string.Format("{0:HH:mm:ss}", dateTime);
		}

		public static bool IsWochenende(this DateTime dateTime) {
			int dayofweek = (int)dateTime.DayOfWeek;
			if (dayofweek == 6 || dayofweek == 0)
				return true;
			return false;
		}

		public static bool IsFeiertag(this DateTime dateTime) {
			return GetAllFeiertage(dateTime.Year, false, true).ContainsValue(dateTime.ResetTimePart());
		}

		public static bool IsDayInMonth(this DateTime dateTime, DateTime day) {
			if (dateTime.Year == day.Year && dateTime.Month == day.Month)
				return true;
			return false;
		}

		public static bool IsDayInYear(this DateTime dateTime, DateTime day) {
			if (dateTime.Year == day.Year)
				return true;
			return false;
		}
		public static bool IsFirstDayOfMonth(this DateTime dateTime) {
			return dateTime.Day == 1 ? true : false;
		}
		public static bool IsLastDayOfMonth(this DateTime dateTime) {
			return dateTime.LastDayOfMonth().Day == dateTime.Day ? true : false;
		}

		public static int MonthDifference(this DateTime begin, DateTime end) {
			return Math.Abs((begin.Month - end.Month) + 12 * (begin.Year - end.Year));
		}
		public static int YearDifference(this DateTime begin, DateTime end) {
			return begin.MonthDifference(end) / 12;
		}

		public static DateTime[] GetWeek(this DateTime dateTime) {
			DateTime[] retval = new DateTime[2];
			DateTime today = dateTime.ResetTimePart();
			double offset = 0;
			switch (today.DayOfWeek) {
				case DayOfWeek.Monday:
					offset = 0;
					break;
				case DayOfWeek.Tuesday:
					offset = -1;
					break;
				case DayOfWeek.Wednesday:
					offset = -2;
					break;
				case DayOfWeek.Thursday:
					offset = -3;
					break;
				case DayOfWeek.Friday:
					offset = -4;
					break;
				case DayOfWeek.Saturday:
					offset = -5;
					break;
				case DayOfWeek.Sunday:
					offset = -6;
					break;
			}
			retval[0] = today.AddDays(offset);
			retval[1] = retval[0].AddDays(7).AddSeconds(-1);
			return retval;
		}

		public static string DayOfWeekShortName(this DateTime dateTime) {
			if ((int)dateTime.DayOfWeek == 0)
				return "So";
			else if ((int)dateTime.DayOfWeek == 1)
				return "Mo";
			else if ((int)dateTime.DayOfWeek == 2)
				return "Di";
			else if ((int)dateTime.DayOfWeek == 3)
				return "Mi";
			else if ((int)dateTime.DayOfWeek == 4)
				return "Do";
			else if ((int)dateTime.DayOfWeek == 5)
				return "Fr";
			else if ((int)dateTime.DayOfWeek == 6)
				return "Sa";
			return "";
		}
		public static string DayOfWeekName(this DateTime dateTime) {
			if ((int)dateTime.DayOfWeek == 0)
				return "Sonntag";
			else if ((int)dateTime.DayOfWeek == 1)
				return "Montag";
			else if ((int)dateTime.DayOfWeek == 2)
				return "Dienstag";
			else if ((int)dateTime.DayOfWeek == 3)
				return "Mittwoch";
			else if ((int)dateTime.DayOfWeek == 4)
				return "Donnerstag";
			else if ((int)dateTime.DayOfWeek == 5)
				return "Freitag";
			else if ((int)dateTime.DayOfWeek == 6)
				return "Samstag";
			return "";
		}
		public static string MonthName(this DateTime dateTime) {
			switch (dateTime.Month) {
				case 1: return "Januar";
				case 2: return "Februar";
				case 3: return "März";
				case 4: return "April";
				case 5: return "Mai";
				case 6: return "Juni";
				case 7: return "Juli";
				case 8: return "August";
				case 9: return "September";
				case 10: return "Oktober";
				case 11: return "November";
				case 12: return "Dezember";
			}
			return "<unbekannter Monat>";
		}
		public static string MonthShortName(this DateTime dateTime) {
			switch (dateTime.Month) {
				case 1: return "Jan";
				case 2: return "Feb";
				case 3: return "Mär";
				case 4: return "Apr";
				case 5: return "Mai";
				case 6: return "Jun";
				case 7: return "Jul";
				case 8: return "Aug";
				case 9: return "Sep";
				case 10: return "Okt";
				case 11: return "Nov";
				case 12: return "Dez";
			}
			return "<unbekannt>";
		}
		public static string HumanReadable(this DateTime dateTime) {
			DateTime datumWithoutTime = dateTime.ResetTimePart();
			if (datumWithoutTime == DateTime.Today)
				return dateTime.ToString("HH:mm");
			else if (datumWithoutTime == DateTime.Today.AddDays(-1))
				return "gestern " + dateTime.ToString("HH:mm");
			else if (datumWithoutTime > DateTime.Today.AddDays(-7))
				return dateTime.DayOfWeekName() + " " + dateTime.ToString("HH:mm");
			else if (datumWithoutTime.Year == DateTime.Today.Year)
				return dateTime.ToString("dd. MMMM HH:mm");
			else
				return dateTime.ToString("g");
		}
		public static string HumanReadableForGrid_Old(this DateTime dateTime) {
			DateTime datumWithoutTime = dateTime.ResetTimePart();
			if (datumWithoutTime == DateTime.Today)
				return dateTime.ToString("HH:mm");
			else if (datumWithoutTime == DateTime.Today.AddDays(-1))
				return "gestern " + dateTime.ToString("HH:mm");
			else if (datumWithoutTime == DateTime.Today.AddDays(1))
				return "morgen " + dateTime.ToString("HH:mm");
			else if (datumWithoutTime > DateTime.Today.AddDays(-7) && datumWithoutTime < DateTime.Today.AddDays(7))
				return dateTime.DayOfWeekShortName() + " " + dateTime.ToString("HH:mm");
			else if (datumWithoutTime.Year == DateTime.Today.Year)
				return dateTime.ToString("dd. MMMM HH:mm");
			else
				return dateTime.ToString("g");
		}
		public static string HumanReadableForGrid(this DateTime dateTime) {
			bool showTime = dateTime.Hour == 0 || dateTime.Minute == 0 ? false : true;
			DateTime datumWithoutTime = dateTime.ResetTimePart();
			if (datumWithoutTime == DateTime.Today) {
				if (!showTime)
					return "heute";
				else
					return dateTime.ToString("HH:mm");
			} else if (datumWithoutTime == DateTime.Today.AddDays(1))
				return "morgen " + (showTime ? dateTime.ToString("HH:mm") : "");
			else if (datumWithoutTime > DateTime.Today.AddDays(-7) && datumWithoutTime < DateTime.Today.AddDays(7))
				return dateTime.DayOfWeekShortName() + " " + (showTime ? dateTime.ToString("HH:mm") : "");
			else if (datumWithoutTime.Year == DateTime.Today.Year) {
				if (showTime)
					return dateTime.ToString("dd.MM HH:mm");
				else
					return dateTime.ToString("dd.MM");
			} else {
				return dateTime.ToString("d");
			}
		}

		#region Feiertage

		public static Stream GetAllFeiertageAsOutlookHolidayStream(int jahr, bool onlyWorkdays, bool onlyOfficial) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("[Deutschland] 209");
			Dictionary<string, DateTime> feiertageVJ = GetAllFeiertage(jahr - 1, onlyWorkdays, onlyOfficial);
			Dictionary<string, DateTime> feiertage = GetAllFeiertage(jahr, onlyWorkdays, onlyOfficial);
			Dictionary<string, DateTime> feiertageNJ = GetAllFeiertage(jahr + 1, onlyWorkdays, onlyOfficial);

			foreach (var key in feiertageVJ.Keys) {
				DateTime datum = feiertageVJ[key];
				sb.AppendLine(key + "," + datum.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
			}
			foreach (var key in feiertage.Keys) {
				DateTime datum = feiertage[key];
				sb.AppendLine(key + "," + datum.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
			}
			foreach (var key in feiertageNJ.Keys) {
				DateTime datum = feiertageNJ[key];
				sb.AppendLine(key + "," + datum.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
			}
			return sb.ToString().ToStream(Encoding.UTF8);		// tostre CUtils.IO.IOUtil.String2Stream(sb.ToString());
		}
		public static Dictionary<string, DateTime> GetAllFeiertage(int jahr, bool onlyWorkdays, bool onlyOfficial) {

			Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();

			//Feste Feiertage
			dict.Add("Neujahr", new DateTime(jahr, 1, 1));
			dict.Add("Heilige Drei Könige", new DateTime(jahr, 1, 6));
			dict.Add("Tag der Arbeit", new DateTime(jahr, 5, 1));
			if (!onlyOfficial)
				dict.Add("Maria Himmelfahrt", new DateTime(jahr, 8, 15));
			dict.Add("Tag der deutschen Einheit", new DateTime(jahr, 10, 3));
			if (!onlyOfficial || jahr == 2017)
				dict.Add("Reformationstag", new DateTime(jahr, 10, 31));
			dict.Add("Allerheiligen ", new DateTime(jahr, 11, 1));
			if (!onlyOfficial)
				dict.Add("Heiliger Abend", new DateTime(jahr, 12, 24));
			dict.Add("1. Weihnachtstag", new DateTime(jahr, 12, 25));
			dict.Add("2. Weihnachtstag", new DateTime(jahr, 12, 26));
			if (!onlyOfficial)
				dict.Add("Silvester", new DateTime(jahr, 12, 31));

			if (!onlyOfficial)
				dict.Add("Augsburger Hohe Friedensfest", new DateTime(jahr, 8, 8));

			//bewegliche Feiertage oder besondere Tage ausgehend vom Ostersonntag
			DateTime os = CrossUtils.Extensions.DateExtensions.GetOstersonntag(jahr);
			dict.Add("Ostersonntag", os);
			if (!onlyOfficial)
				dict.Add("Gründonnerstag", os.AddDays(-3));
			dict.Add("Karfreitag", os.AddDays(-2));
			dict.Add("Ostermontag", os.AddDays(1));
			dict.Add("Christi Himmelfahrt", os.AddDays(39));
			dict.Add("Pfingstsonntag", os.AddDays(49));
			dict.Add("Pfingstmontag", os.AddDays(50));
			dict.Add("Fronleichnam", os.AddDays(60));
			if (!onlyOfficial)
				dict.Add("Mess'-Umzug", os.AddDays(62));
			if (!onlyOfficial)
				dict.Add("Herrenmontag", os.AddDays(71));

			//bewegliche Feiertage ausgehend vom 1. Weihnachtsfeiertag
			DateTime w1 = new DateTime(jahr, 12, 25);
			int wochentag1W = (int)w1.DayOfWeek;
			dict.Add("Volkstrauertag", w1.AddDays(-wochentag1W - 35));
			if (!onlyOfficial)
				dict.Add("Buss- und Bettag", w1.AddDays(-wochentag1W - 32));
			dict.Add("Totensonntag", w1.AddDays(-wochentag1W - 28));
			dict.Add("1. Advent", w1.AddDays(-wochentag1W - 21));
			dict.Add("2. Advent", w1.AddDays(-wochentag1W - 14));
			dict.Add("3. Advent", w1.AddDays(-wochentag1W - 7));
			dict.Add("4. Advent", w1.AddDays(-wochentag1W));

			DateTime ed = new DateTime(jahr, 10, 1);
			dict.Add("Erntedankfest", ed.AddDays(7 - (int)ed.DayOfWeek));

			DateTime m1 = new DateTime(jahr, 5, 1); //Tag der Arbeit
			dict.Add("Muttertag", (int)m1.DayOfWeek == 0 ? m1.AddDays(7) : m1.AddDays(14 - (int)m1.DayOfWeek));

			//var sortedDict = from c in dict orderby c.Value ascending select c;

			//dict.Clear();


			//for (int i = dict.Count - 1; i > 0; i--) {
			//}

			Dictionary<string, DateTime> retval = new Dictionary<string, DateTime>();
			foreach (KeyValuePair<string, DateTime> kvp in dict) {
				if (!onlyWorkdays || !kvp.Value.IsWochenende())
					retval.Add(kvp.Key, kvp.Value);
			}
			return retval;
		}

		public static DateTime GetOstersonntag(int jahr) {
			int a = 0;
			int b = 0;
			int c = 0;
			int d = 0;
			int e = 0;
			int f = 0;

			a = jahr % 19;
			b = jahr / 100;
			c = (8 * b + 13) / 25 - 2;
			d = b - (jahr / 400) - 2;
			e = (19 * (jahr % 19) + ((15 - c + d) % 30)) % 30;

			if (e == 28) {
				if (a > 10)
					e = 27;
			} else if (e == 29) {
				e = 28;
			}
			f = (d + 6 * e + 2 * (jahr % 4) + 4 * (jahr % 7) + 6) % 7;
			DateTime retval = new DateTime(jahr, 3, 1);
			retval = retval.AddDays(Convert.ToDouble(e + f + 21));
			return retval;
		}

		//public static DateTime GetOstermontag(int jahr) {
		//	DateTime retval = GetOstersonntag(jahr);
		//	retval = retval.AddDays(Convert.ToDouble(1));
		//	return retval;
		//}

		#endregion

	}
}
