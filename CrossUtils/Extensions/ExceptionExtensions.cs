using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.Extensions
{
    public static class ExceptionExtensions { 
		public static string ToString(this Exception ex, bool includeStackTrace) {
		if (ex == null)
			return "";
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("Ausnahme aufgetreten: " + ex.GetType().FullName);
		sb.AppendLine(ex.Message);
		if (includeStackTrace)
			sb.AppendLine(ex.StackTrace);
		while (ex.InnerException != null) {
			sb.AppendLine(ex.InnerException.Message);
			if (includeStackTrace)
				sb.AppendLine(ex.InnerException.StackTrace);
			ex = ex.InnerException;
		}
		return sb.ToString();
	}
    }
}
