using System;

namespace Ra180.UI
{
    public static class RectangleExtensions
    {
        public static Rectangle PlaceInside(this Rectangle src, Rectangle container)
        {
            return new Rectangle(
                container.Left + src.Left,
                container.Top + src.Top,
                container.Left + src.Right,
                container.Top + src.Bottom
                );
        }

        public static Rectangle Scale(this Rectangle src, float scale)
        {
            return new Rectangle(src.Left * scale, src.Top * scale, src.Right * scale, src.Bottom * scale);
        }

        public static Rectangle ScaleToFit(this Rectangle src, float dstWidth, float dstHeight)
        {
            return ScaleToFit(src, new Rectangle(0, 0, dstWidth, dstHeight));
        }

        public static Rectangle ScaleToFit(this Rectangle src, Rectangle dst)
        {
            var scale = GetScale(src, dst);
            var width = src.Width * scale;
            var height = src.Height * scale;
            var ymargin = (dst.Height - height) / 2;
            var xmargin = (dst.Width - width) / 2;
            return new Rectangle(xmargin, ymargin, width + xmargin, height + ymargin);
        }

        public static float GetScale(this Rectangle src, float dstWidth, float dstHeight)
        {
            return GetScale(src, new Rectangle(0, 0, dstWidth, dstHeight));
        }

        public static float GetScale(this Rectangle src, Rectangle dst)
        {
            return Math.Min(dst.Width / src.Width, dst.Height / src.Height);
        }

        public static bool Contains(this Rectangle rectangle, float x, float y)
        {
            if (x < rectangle.X) return false;
            if (y < rectangle.Y) return false;
            if (x > rectangle.Right) return false;
            if (y > rectangle.Bottom) return false;
            return true;
        }
    }
}