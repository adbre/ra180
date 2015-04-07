using System;

namespace Ra180.UI
{
    public interface ICanvas : IDisposable
    {
        Rectangle Size { get; }
        void DrawBitmap(IBitmapDrawable bitmap, Rectangle rectangle);
        void DrawText(string text, float x, float y, float size, Color color);
        void DrawRectangle(Rectangle rectangle, Color color);
    }
}