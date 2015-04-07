using System;
using System.Windows.Forms;
using Ra180.Client.WinForms.Properties;
using Ra180.UI;

namespace Ra180.Client.WinForms
{
    public class WinFormsGraphic : IGraphic
    {
        private readonly Control _control;

        public WinFormsGraphic(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
        }

        public IBitmapDrawable GetDrawable(string id)
        {
            if (id == Drawables.Dart380BackgroundImage)
            {
                return new WinFormsBitmap(Resources.Dart380_1363x1018);
            }

            return null;
        }

        public void Invalidate()
        {
            _control.Invalidate();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}