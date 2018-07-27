using System;
using System.Collections.Generic;
using System.Text;

namespace CrossUtils.UID
{
	public class UIDInfo : IComparable {
		public int TypeID { get; set; } = 0;
		public string ObjectID { get; set; } = "";
		public string Caption { get; set; } = "";
		public bool IsInvalid { get; private set; } = false;
		public UIDInfo() {

		}

		public UIDInfo(string uid) {
			string[] splitted = uid.Split(":".ToCharArray());
			if (splitted.Length != 2)
				IsInvalid = true;
			else {
				this.TypeID = Convert.ToInt32(splitted[0]);
				this.ObjectID = splitted[1].Trim();
			}
		}
		public UIDInfo(int typeID, string objectID) {
			TypeID = typeID;
			ObjectID = objectID;
		}
		public UIDInfo(int typeID, int objectID) {
			TypeID = typeID;
			ObjectID = objectID.ToString();
		}

		public UIDInfo(int typeID, string objectID, string caption) {
			TypeID = typeID;
			ObjectID = objectID;
			Caption = caption;
		}

		public UIDInfo(UIDType uidType, int objectID) {
			TypeID = (int)uidType;
			ObjectID = objectID.ToString();
		}

		public void Refresh(int typeID, string objectID, string caption) {
			TypeID = typeID;
			ObjectID = objectID;
			Caption = caption;
		}

		public static int GetTypeID(string uid) {
			string[] splitted = uid.Split(":".ToCharArray());
			return Convert.ToInt32(splitted[0]);
		}
		public static int GetObjectID(string uid) {
			string[] splitted = uid.Split(":".ToCharArray());
			return Convert.ToInt32(splitted[1]);
		}
		public string Uid {
			get { return TypeID + ":" + ObjectID; }
		}

		public int CompareTo(object obj) {
			UIDInfo c = (UIDInfo)obj;
			return this.Uid.CompareTo(c.Uid);
		}

		public override bool Equals(object obj22) {
			if (obj22 != null && obj22 is UIDInfo) {
				UIDInfo obj2 = obj22 as UIDInfo;
				return this.Uid == obj2.Uid;
			}
			return false;
		}

		public override int GetHashCode() {
			return this.Uid.GetHashCode();
		}

		public override string ToString() {
			return this.Caption;
		}
	}
}
