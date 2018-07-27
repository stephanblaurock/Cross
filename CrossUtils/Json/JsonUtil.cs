using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using CrossUtils.Extensions;
using Newtonsoft.Json;

namespace CrossUtils.Json
{
	public class JsonUtil {
		public static T GetValue<T>(JObject obj, string propertyName, T defaultValue) {
			if (obj == null || obj[propertyName] == null)
				return defaultValue;
			try {
				JToken jtoken = null;
				if (obj.TryGetValue(propertyName, out jtoken)) {
					if (typeof(T) == typeof(JToken))
						return (T)(object)jtoken;
					else if (typeof(T) == typeof(int))
						return (T)(object)jtoken.Value<int>();
					//return (T)(object)Int32.Parse(jtoken.ToString());   //.Trim("\"".ToCharArray()));
					else if (typeof(T) == typeof(long))
						return (T)(object)jtoken.Value<long>();
					//return (T)(object)Int64.Parse(jtoken.ToString());
					else if (typeof(T) == typeof(decimal)) {
						return (T)(object)jtoken.Value<decimal>();
						//if (jtoken.Type == JTokenType.Integer || jtoken.Type == JTokenType.Float)
						//	return (T)(object)jtoken.Value<decimal>();
						//else
						//	return (T)(object)Decimal.Parse(jtoken.ToString(), CultureInfo.InvariantCulture);
					} else if (typeof(T) == typeof(float))
						return (T)(object)jtoken.Value<float>();
					//return (T)(object)Single.Parse(jtoken.ToString(), CultureInfo.InvariantCulture);
					else if (typeof(T) == typeof(string))
						return (T)(object)jtoken.ToString();
					else if (typeof(T) == typeof(bool))
						return (T)(object)Boolean.Parse(jtoken.ToString());
					else if (typeof(T) == typeof(Guid))
						return (T)(object)Guid.Parse(jtoken.ToString());
					else if (typeof(T) == typeof(DateTime))
						return (T)(object)DateTime.Parse(jtoken.ToString());
					else if (typeof(T) == typeof(TimeSpan))
						return (T)(object)TimeSpan.Parse(jtoken.ToString());
					else if (typeof(T) == typeof(byte[]))
						return (T)(object)jtoken.ToString().ToByteArrayFromBae64();	// Utils.StringUtil.ByteArrayFromBase64(jtoken.ToString());
				}
			} catch { }
			return defaultValue;
		}

		public static void SetValue(JObject obj, string propertyName, object value) {
			if (obj == null)
				throw new ArgumentNullException();
			obj.Remove(propertyName);
			if (value != null && value.GetType() == typeof(byte[]))      // Byte-Array beim Schreiben zuerst in String umwandeln
				value = ((byte[])value).ToBase64String();		// Utils.StringUtil.ByteArrayToBase64((byte[])value);
			obj.Add(propertyName, new JValue(value));
		}

		public static void SetObject<T>(JObject obj, string propertyName, T jobject) {
			if (obj == null)
				throw new Exception("Es wurde kein gültiges Objekt als Speicher übergeben");
			//obj.Add(propertyName, jobject.ToJson());
			obj.Add(propertyName, JsonConvert.SerializeObject(jobject));
		}

		public static T GetObject<T>(JObject obj, string propertyName, T defaultValue) {
			if (obj == null)
				return defaultValue;
			JToken tok = null;
			if (obj.TryGetValue(propertyName, out tok)) {
				T myobj = (T)Activator.CreateInstance(typeof(T));
				JsonConvert.PopulateObject(tok.ToString(), myobj);
				//myobj.FromJson(tok.ToString());
				return myobj;
			}
			return defaultValue;
		}
		public static void SetToken(JObject obj, string propertyName, JToken token) {
			obj.Add(propertyName, token);
		}
		public static JToken GetToken(JObject obj, string propertyName) {
			JToken tok = null;
			obj.TryGetValue(propertyName, out tok);
			return tok;
		}

		//public static void SetArray<T>(JObject obj, string propertyName, List<T> objects) where T : IJsonExchange {
		//	if (obj == null)
		//		throw new Exception("Es wurde kein gültiges Objekt als Speicher übergeben");
		//	if (objects == null || objects.Count == 0)
		//		return;
		//	JArray arr = new JArray();
		//	foreach (IJsonExchange jobj in objects) {
		//		arr.Add(jobj.ToJson());
		//	}
		//	obj.Add(propertyName, arr);
		//}

		//public static List<T> GetArray<T>(JObject obj, string propertyName) where T : IJsonExchange {
		//	List<T> retval = new List<T>();
		//	JToken arrtok = null;
		//	if (obj.TryGetValue(propertyName, out arrtok)) {
		//		JArray arr = (JArray)arrtok;
		//		foreach (JToken tok in arr) {
		//			T myobj = (T)Activator.CreateInstance(typeof(T));
		//			myobj.FromJson(tok.ToString());
		//			retval.Add(myobj);
		//		}
		//	}
		//	return retval;
		//}
	}
}
