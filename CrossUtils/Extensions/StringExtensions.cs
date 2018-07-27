using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CrossUtils.Extensions
{
    public static class StringExtensions
    {
		public static byte[] ToByteArrayFromBae64(this string data) {
			return Convert.FromBase64String(data);
		}

		public static Stream ToStream(this string s, Encoding encoding) {
			byte[] buf = encoding.GetBytes(s);		//.GetEncoding(1252).GetBytes(s);
			MemoryStream ms = new MemoryStream(buf);
			return (Stream)ms;
		}
	}
}
