using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.UID
{
    public class UIDHelper
    {
		public static List<string> GetObjectIDs(List<UIDInfo> infos, int typeFilter) {
			List<string> retval = new List<string>();
			if (infos != null && infos.Count > 0) {
				infos.ForEach(x => {
					if (typeFilter <= 0 || x.TypeID == typeFilter)
						retval.Add(x.ObjectID);
				});
			}
			return retval;
		}
		public static List<string> GetUIDs(List<UIDInfo> infos, int typeFilter) {
			List<string> retval = new List<string>();
			if (infos != null && infos.Count > 0) {
				infos.ForEach(x => {
					if (typeFilter <= 0 || x.TypeID == typeFilter)
						retval.Add(x.Uid);
				});
			}
			return retval;
		}
	}
}
