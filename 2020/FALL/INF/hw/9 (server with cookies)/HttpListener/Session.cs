using System;
using System.Collections.Generic;

namespace HttpListener
{
	[Serializable]
	public class Session
	{
		public DateTime LastVisitTime;
		public readonly Dictionary<string, object> Data;

		public Session()
		{
			Data = new Dictionary<string, object>();
			LastVisitTime = DateTime.Now;
		}
	}
}