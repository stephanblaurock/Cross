using CrossUtils.Json;
using Modules.Cross.Users.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Cross.Users {
	public class UserModule {
		public static UserModule Default;

		public UserModule() {
			Default = this;
		}

		public async Task<User> LoadCurrentUser() {
			JsonCommand cmd = UserServiceCommands.CreateGetUserCommand();
			JsonCommandRetValue retval = await ModulesClientEnvironment.Default.JsonCommandClient.DoCommand(cmd);
			if (retval.ReturnCode == 200) {
				_CurrentUser = JsonConvert.DeserializeObject<User>(retval.ReturnValue);
			}
			return _CurrentUser;
		}

		private User _CurrentUser = null;
		public User CurrentUser {
			get {
				return _CurrentUser;
			}
		}

	}
}
