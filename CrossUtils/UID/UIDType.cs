using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.UID
{
	public enum UIDType : int {
		// *** OMS : 100 *** //
		OMSMandant = 100,
		OMSContainer = 101,
		OMSTopic = 102,
		OMSFilesystemHook = 103,
		OMSFilesystemFile = 104,

		// **** Archivar : 200 **** //
		ArchivarFile = 201,

		// **** GFiles: 300 **** //
		GFile = 301,

		GFDokument = 320,
		GFAuftrag = 321,
		GFNachtrag = 322,
		GFVertrag = 323,
		GFProtokoll = 324,
		GFHandbuch = 325,
		//GFBesprechungsprotokoll = 324,
		//GFAbnahmeprotokoll = 325,
		//GFMessprotokoll = 326,
		//GFPrüfprotokoll = 327,
		GFLeistungsverzeichnis = 328,
		GFMiniArchiv = 330,
		GFBildergalerie = 331,
		GFAufmass = 332,
		GFTagesbericht = 333,
		GFZeichnung = 334,
		GFEingangsrechnung = 335,
		GFEingangslieferschein = 336,
		GFLieferschein = 337,
		GFRuecklieferschein = 338,
		GFRechnung = 339,
		GFGutschrift = 340,
		GFPreisanfrage = 341,
		GFBestellung = 342,
		GFAuftragsbestaetigung = 343,
		GFAngebot = 344,
		GFPersonalunterlagen = 345,

		// **** Kontakte: 400 *** //
		Kontakt = 401,
		Ansprechpartner = 402,
		Kommission = 403,

		// **** Projekte: 500 *** //
		Projekt = 501,
		Nachkalkulation = 502,
		Anlage = 550,
		Geraet = 551,
		Geraetetyp = 552,

		// **** Eingangs-Belege: 600 **** //
		Beleg = 600,
		Eingangsrechnung = 611,
		Eingangslieferschein = 612,
		Bestellung = 613,
		Preisanfrage = 614,
		Preisspiegel = 615,
		Bestellanforderung = 616,

		// **** Ausgangsbelege 700 **** //
		InternBeleg = 701,
		Zwischenbeleg = 702,
		Angebot = 703,
		Auftragsbestaetigung = 704,
		Lieferschein = 705,
		Ruecklieferschein = 706,
		Belastung = 707,
		Entlastung = 708,
		Teilrechnung = 709,
		Rechnung = 710,
		Gutschrift = 711,
		Barbeleg = 712,
		Ausgabebeleg = 713,
		Materialermittlung = 714,
		Zeichnung = 715,
		Elektroverteilung = 716,
		Plan = 717,
		Kundendienstauftrag = 718,
		Auftrag = 719,



		// **** CTI: 750 **** //

		// **** Wartung: 800 **** //
		Wartungsvertrag = 801,
		Wartung = 802,

		// **** Mail & Messaging & Blogging: 900 **** //
		EMail = 901,

		MessageChannel = 951,
		MessageTopic = 952,

		BlogArchive = 971,
		BlogPost = 972,

		// **** Kasse: 14000 **** //
		Kassenbuch = 14000,
		KassenbuchMonat = 14001,
		KassenbuchGeschaeftsvorfall = 14002,
		KassenbuchGegenkonto = 14003,

		// **** Zeiterfassung: 14100 **** //
		MetaStundenItem = 14100,
		MitarbeiterManuell = 14101,


		// *** LUMARA *** //
		LumaraFachberater = 22100,
		LumaraRechnung = 22110,
		LumaraBuchungslauf = 22120,


		//// *** netLogger *** //
		//NetLoggerSoftware = 30100,
		//NetLoggerSensor = 30101,
		//NetLoggerIPC = 30102,
		//NetLoggerDevice = 30103,
		//NetLoggerCustomer = 30104,
		//NetLoggerSensorGroup = 30105,
		//NetLoggerDeviceGroup = 30106,
		//NetLoggerPlant = 30107,
		//NetLoggerDashboard = 30108

	}
}
