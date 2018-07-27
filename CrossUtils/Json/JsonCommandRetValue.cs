using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json;

namespace CrossUtils.Json
{
	public class JsonCommandRetValue {
		/// <summary>
		/// 0 = undefiniert, -1 = Error, 1 = Successfull
		/// </summary>
		public int ReturnCode = 0;
		/// <summary>
		/// Detaillierte Textmeldung im Zusammenhang mit dem Return-Code
		/// </summary>
		public string ReturnMessage = "";
		/// <summary>
		/// Json-Objekt(e), die an den Aufrufer zurückgegeben werden sollen
		/// </summary>
		public string ReturnValue = "";

		public JsonCommandRetValue() {
		}

		public JsonCommandRetValue(JsonCommandWebResponse webresponse) {
			this.ReturnCode = 999;
			this.ReturnValue = JsonConvert.SerializeObject(webresponse);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="returnCode">0 = undefiniert, -1 = Error, 1 = Successfull</param>
		public JsonCommandRetValue(int returnCode) {
			ReturnCode = returnCode;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="returnCode">0 = undefiniert, -1 = Error, 1 = Successfull</param>
		/// <param name="returnMessage">Detaillierte Textmeldung im Zusammenhang mit dem Return-Code</param>
		public JsonCommandRetValue(int returnCode, string returnMessage) {
			ReturnCode = returnCode;
			ReturnMessage = returnMessage;
		}
		public JsonCommandRetValue(int returnCode, string returnMessage, string returnValue) {
			ReturnCode = returnCode;
			ReturnMessage = returnMessage;
			ReturnValue = returnValue;
		}
		public JsonCommandRetValue(string data) {
			if (!string.IsNullOrEmpty(data))
				JsonConvert.PopulateObject(data, this);
		}

		public string ToJson() {
			return JsonConvert.SerializeObject(this);
		}
	}
}
