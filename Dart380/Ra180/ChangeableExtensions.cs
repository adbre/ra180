using System;

namespace Ra180
{
	public static class ChangeableExtensions
	{
		public static void Update<T>(this T changeable, Action<T> callback)
			where T : ChangeableBase
		{
			changeable.Update(() => callback(changeable));
		}

		public static void Update(this ChangeableBase changeable, Action callback) {
		    try
		    {
		        changeable.SuspendLayout();
		        callback();
		    }
		    finally
		    {
                changeable.ResumeLayout();
		    }
		}
	}

}
