using CrossUtils.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Cross.Users
{
    public class UserServiceCommands
    {
		public static JsonCommand CreateGetUserCommand(int userID = 0) {
			JsonCommand retval = new JsonCommand("Modules.Users.Service.UserService", "GetUserViewModel");
			if (userID > 0)
				retval.SetParameter("UserID", userID);
			return retval;
		}
	}
}
