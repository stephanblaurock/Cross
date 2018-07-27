using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.Json
{
	public delegate void JsonCommandEventHandler(object sender, JsonCommandEventArgs e);
	public class JsonCommandEventArgs {
		public JsonCommand JsonCommand;
		public JsonCommandEventArgs(JsonCommand command) {
			JsonCommand = command;
		}
	}
}
