using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.Json
{
	public class JsonCommand {
		public string ModuleName { get; set; }
		public string CommandName { get; set; }
		public JObject Parameter { get; set; } = new JObject();
		//public string Result { get; set; } = "";

		public JsonCommand(string data) {
			if (string.IsNullOrEmpty(data))
				return;
			JObject obj = JObject.Parse(data);
			ModuleName = JsonUtil.GetValue<string>(obj, "ModuleName", "");
			CommandName = JsonUtil.GetValue<string>(obj, "CommandName", "");
			string paraString = JsonUtil.GetValue<string>(obj, "Parameter", "");
			if (!string.IsNullOrEmpty(paraString)) {
				Parameter = JObject.Parse(paraString);
			}
		}

		public JsonCommand(string moduleName, string commandName) {
			ModuleName = moduleName;
			CommandName = commandName;
		}

		public JsonCommand(string moduleName, string commandName, JObject parameter) {
			ModuleName = moduleName;
			CommandName = commandName;
			if (parameter != null)
				Parameter = parameter;
		}

		public void SetParameter(string name, object value) {
			JsonUtil.SetValue(Parameter, name, value);
		}

		public T GetParameter<T>(string name, T defaultValue) {
			return JsonUtil.GetValue<T>(Parameter, name, defaultValue);
		}

		public Dictionary<string, string> GetParameterList() {
			Dictionary<string, string> retval = new Dictionary<string, string>();
			foreach (JProperty property in this.Parameter.Properties()) {
				retval.Add(property.Name, property.Value.ToString());
			}
			return retval;
		}
	}
}
