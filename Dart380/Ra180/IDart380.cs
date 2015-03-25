using System;
using System.Collections.Generic;

namespace Ra180
{
	public interface IDart380
	{
		Ra180Display LargeDisplay { get; }
		Ra180Display SmallDisplay { get; }
		void SendKey (string key);
	}
}
