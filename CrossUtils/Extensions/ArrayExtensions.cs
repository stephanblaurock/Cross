using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.Extensions
{
    public static class ArrayExtensions
    {
		public static string ToBase64String(this byte[] data) {
			return Convert.ToBase64String(data);
		}

	
    }
}
