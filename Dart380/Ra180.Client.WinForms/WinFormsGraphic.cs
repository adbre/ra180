using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ra180.Client.WinForms.Properties;
using Ra180.UI;

namespace Ra180.Client.WinForms
{
    public class WinFormsGraphic : IGraphic
    {
        private readonly Control _control;

        private readonly Dictionary<string, Func<Bitmap>> _bitmapFactories = new Dictionary<string, Func<Bitmap>>
            {
                {Drawables.Dart380BackgroundImage, () => Resources.Dart380_1363x1018},
                {Drawables.Vred1, () => Resources.Vred_1},
                {Drawables.Vred2, () => Resources.Vred_2},
                {Drawables.Vred3, () => Resources.Vred_3},
                {Drawables.Vred4, () => Resources.Vred_4},
                {Drawables.Vred5, () => Resources.Vred_5},
                {Drawables.Vred6, () => Resources.Vred_6},
                {Drawables.Vred7, () => Resources.Vred_7},
                {Drawables.Vred8, () => Resources.Vred_8},
            };

        public WinFormsGraphic(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
        }

        public IBitmapDrawable GetDrawable(string id)
        {
            Func<Bitmap> bitmapFactory;
            if (!_bitmapFactories.TryGetValue(id, out bitmapFactory))
                return null;

            Bitmap bitmap = null;
            try
            {
                bitmap = bitmapFactory();
                return new WinFormsBitmap(bitmap);
            }
            catch
            {
                if (bitmap != null)
                    bitmap.Dispose();

                throw;
            }
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