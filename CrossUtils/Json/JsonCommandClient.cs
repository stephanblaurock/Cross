using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CrossUtils.Extensions;

namespace CrossUtils.Json
{
	public class JsonCommandClient {
		public static JsonCommandClient Default;
		public event JsonCommandEventHandler CommandReceived;
		/// <summary>
		/// Url, über die die Commands gesendet werden können (Einbahnstraße, aber mit Rückmeldungen).
		/// z.B. http://resprovider/jsoncommands/docommand (im Header wird dann der Command mit allen Parametern übergeben)
		/// </summary>
		public string CommandUrl { get; set; } = "http://api.lumara.de/?jsoncommand";
		private string _Username = "";
		private string _Password = "";
		private string _Token = "";
		//public string Username { get; set; }
		//public string ApiToken { get; set; }

		//public Utils.Net.WebSocketClient WebSocketClient { get; private set; }

		public JsonCommandClient(string webserverUrl, string username, string password) {
			Default = this;
			_Username = username;
			_Password = password;
			if (_Username == "ipc_user" && _Password == "")
				this._Token = "Xdf#17Nx";
			if (_Username == "admin_user" && _Password == "")
				this._Token = "Xfüg#19Rs@";
			CommandUrl = webserverUrl + "/cmd?jsoncommand";
			//if (!string.IsNullOrWhiteSpace(websocketUrl)) {
			//	WebSocketClient = new Net.WebSocketClient(websocketUrl, userID, username);
			//	WebSocketClient.MessageReceived += _WebsocketClient_MessageReceived;
			//}
		}

		/// <summary>
		/// Diese Methode erzeugt eine Url, die direkt abgesetzt werden kann. Diese URL enthält alle Parameter, die im
		/// JsonCommand angegeben sind. Achtung! Es kann kein Content übergeben werden!
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public string BuildCommandUrl(JsonCommand cmd) {
			StringBuilder sb = new StringBuilder();
			sb.Append(CommandUrl + "&user=" + _Username + "&token=" + _Token + "&modulename=" + cmd.ModuleName +
				"&commandname=" + cmd.CommandName);
			Dictionary<string, string> parameterList = cmd.GetParameterList();
			foreach (string key in parameterList.Keys) {
				sb.Append("&" + key + "=" + parameterList[key]);
			}
			return sb.ToString();
		}

		//private void _WebsocketClient_MessageReceived(object sender, Net.WebSocketClientMessageEventArgs e) {
		//	string data = e.Message;
		//	if (!string.IsNullOrWhiteSpace(data) && data.StartsWith("{")) {
		//		CommandReceived?.Invoke(this, new JsonCommandEventArgs(new JsonCommand(data)));
		//	}
		//}

		//public void Start() {
		//	if (WebSocketClient != null)
		//		WebSocketClient.Open();
		//}
		/// <summary>
		/// Führt einen JsonCommand aus. Dies kann entweder über eine TCP-Verbindung oder über eine URL (Webservice) geschehen
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public async Task<JsonCommandRetValue> DoCommand(JsonCommand cmd) {
			// sicherstellen, daß der User und der Token immer mitgeschickt werden
			cmd.SetParameter("user", this._Username);
			cmd.SetParameter("token", this._Token);
			JsonCommandRetValue retval = await MakeUrlRequest(cmd);
			// Wir haben ja jetzt einen Auth-Service eingebaut, damit nicht jeder Hinz und Kunz hier JsonCommands absetzen kann ;-)
			// Daher muss ich jetzt die 401-Meldungen abfangen
			if (retval.ReturnCode == 401) {
				// 401 = unauthorisiert, nun einfach einen login-command senden, und dann prüfen, ob dann der command durchgeht
				JsonCommand authCommand = new JsonCommand("", "Login");
				authCommand.SetParameter("user", _Username);
				authCommand.SetParameter("password", _Password);
				JsonCommandRetValue authCommandRetValue = await MakeUrlRequest(authCommand);
				if (authCommandRetValue.ReturnCode == 200 || authCommandRetValue.ReturnCode == 1) {
					// Login erfolgreich, also wieder den alten JsonCommand senden
					this._Token = authCommandRetValue.ReturnValue;
					cmd.SetParameter("token", this._Token);
					return await MakeUrlRequest(cmd);
				} else {
					// Login fehlgeschlagen, ich gebe den LoginRetValue zurück
					return authCommandRetValue;
				}
			} else {
				return retval;
			}
		}


		private async Task<JsonCommandRetValue> MakeUrlRequest(JsonCommand cmd) {
			try {
				using (var client = new HttpClient()) {
					var stringContent = new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");      // Utils.Security.Cryptography.EncryptStringAES(cmd.ToJson().ToString()));
					stringContent.Headers.Add("X-JsonCommand", cmd.CommandName);
					
					HttpResponseMessage response = await client.PostAsync(CommandUrl, stringContent);
					if (response.IsSuccessStatusCode) {
						if (response.Content != null) {
							string json = await response.Content.ReadAsStringAsync();
							JsonCommandRetValue retval = new JsonCommandRetValue(json);
							return retval;
						}
					} else {
						//Console.WriteLine("Failed to publish: HTTP " + response.StatusCode);
						if (response.Content != null) {
							string json = await response.Content.ReadAsStringAsync();
							JsonCommandRetValue retval = new JsonCommandRetValue(-1, "Failed to publish: HTTP " + response.StatusCode);
							retval.ReturnValue = json;
							return retval;
						}
					}

				}
			} catch (Exception ex) {
				string s = ex.Message + "\r\n" + ex.StackTrace;
				return new JsonCommandRetValue(-1, "Fehler bei JsonCommandClient.MakeUrlRequest: " + ex.ToString(true));
			}
			return new JsonCommandRetValue();
		}

		public async Task<JsonCommandRetValue> DoRawCommand(string cmdData) {
			using (var client = new HttpClient()) {
				//client.BaseAddress = new Uri(CommandUrl);
				//client.DefaultRequestHeaders.Accept.Clear();
				//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var stringContent = new StringContent(cmdData, Encoding.UTF8, "application/json");      // Utils.Security.Cryptography.EncryptStringAES(cmd.ToJson().ToString()));
																										//stringContent.Headers.Add("Content-Type", "application/json");
																										//stringContent.Headers.Add("X-JsonCommand", cmd.CommandName);
																										//var stream = File.OpenRead("B:\\test3.pdf");
																										//HttpResponseMessage response = await client.PostAsync(CommandUrl, new StreamContent(stream));
																										//stream.Close();
																										//Utils.IO.IOUtil.StringToFile("C:\\temp\\post.dat", await stringContent.ReadAsStringAsync());
				HttpResponseMessage response = await client.PostAsync(CommandUrl, stringContent);
				if (response.IsSuccessStatusCode) {
					if (response.Content != null) {
						string json = await response.Content.ReadAsStringAsync();
						//if (!string.IsNullOrWhiteSpace(json))
						//	json = Utils.Security.Cryptography.DecryptStringAES(json);
						JsonCommandRetValue retval = new JsonCommandRetValue(json);
						//retval.ReturnValue = json;
						return retval;
					}
				} else {
					//Console.WriteLine("Failed to publish: HTTP " + response.StatusCode);
					if (response.Content != null) {
						string json = await response.Content.ReadAsStringAsync();
						JsonCommandRetValue retval = new JsonCommandRetValue(-1, "Failed to publish: HTTP " + response.StatusCode);
						retval.ReturnValue = json;
						return retval;
					}
				}
			}
			return new JsonCommandRetValue();
		}
	}
}
