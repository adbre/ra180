using System;
using System.Drawing;
using Ra180.UI;
using Rectangle = Ra180.UI.Rectangle;

namespace Ra180.Client.WinForms
{
    public class WinFormsBitmap : IBitmapDrawable
    {
        private readonly Image _bitmap;

        public WinFormsBitmap(Image bitmap)
        {
            _bitmap = bitmap;
        }

        public Rectangle Size
        {
            get
            {
                var size = _bitmap.Size;
                return new Rectangle(0, 0, size.Width, size.Height);
            }
        }

        public Image Image
        {
            get { return _bitmap; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _bitmap.Dispose();
        }
    }
}