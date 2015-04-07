using System;
using System.Drawing;
using Ra180.UI;
using Color = Ra180.UI.Color;
using Rectangle = Ra180.UI.Rectangle;

namespace Ra180.Client.WinForms
{
    public class WinFormsCanvas : ICanvas
    {
        private readonly Graphics _graphics;
        private readonly Rectangle _size;

        public WinFormsCanvas(Graphics graphics, System.Drawing.Rectangle size)
        {
            _graphics = graphics;
            _size = new Rectangle(size.Left, size.Top, size.Width, size.Height);
        }

        public Rectangle Size { get { return _size; } }

        void ICanvas.DrawBitmap(IBitmapDrawable bitmap, Rectangle rectangle)
        {
            if (bitmap == null) throw new ArgumentNullException("bitmap");
            
            var wfBitmap = bitmap as WinFormsBitmap;
            if (wfBitmap == null) throw new ArgumentException(@"bitmap must be an instance returned by WinFormsGraphic.GetDrawable(string)", "bitmap");

            DrawBitmap(wfBitmap, rectangle);
        }

        public void DrawBitmap(WinFormsBitmap bitmap, Rectangle rectangle)
        {
            DrawBitmap(bitmap.Image, ToDrawingRect(rectangle));
        }

        public void DrawBitmap(Image image, RectangleF rectangle)
        {
            _graphics.DrawImage(image, rectangle);
        }

        public void DrawText(string text, float x, float y, float size, Color color)
        {
            using (var brush = new SolidBrush(ToDrawingColor(color)))
            using (var font = new Font(FontFamily.GenericMonospace, size))
                _graphics.DrawString(text, font, brush, x, y - size);
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            using (var pen = new Pen(ToDrawingColor(color)))
                _graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private System.Drawing.Color ToDrawingColor(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.B, color.G);
        }

        private RectangleF ToDrawingRect(Rectangle rectangle)
        {
            return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}