using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ra180
{
	public abstract class ChangeableBase
	{
		private int _suspendCount;
		private bool _hasPendingChange;

		public event EventHandler Changed;

		public bool IsSuspended
		{
			get { return _suspendCount > 0; }
		}

		public void SuspendLayout() {
			_suspendCount++;
		}

		public void ResumeLayout() {
			_suspendCount = Math.Max (_suspendCount - 1, 0);

			if (_suspendCount == 0 && _hasPendingChange) {
				_hasPendingChange = false;
				OnChanged (EventArgs.Empty);
			}
		}

		protected void SetHasPendingChange() {
			_hasPendingChange = true;
		}

		protected virtual void OnChanged(EventArgs e) {
			var handler = Changed;
			if (handler != null)
				handler (this, e);
		}
	}

}
