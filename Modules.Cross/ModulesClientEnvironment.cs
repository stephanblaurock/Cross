using CrossUtils.Json;
using Modules.Cross.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Cross {
	public class ModulesClientEnvironment {
		public static ModulesClientEnvironment Default;
		public JsonCommandClient JsonCommandClient;
		public string JsonCommandServerUrl { get; private set; }

		public ModulesClientEnvironment(string jsonCommandUrl, string login, string password) {
			Default = this;
			new UserModule();
			JsonCommandServerUrl = jsonCommandUrl;
			this.JsonCommandClient = new JsonCommandClient(JsonCommandServerUrl, login, password);
		}

	}
}
