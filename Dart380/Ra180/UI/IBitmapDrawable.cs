using System;

namespace Ra180.UI
{
    public interface IBitmapDrawable : IDisposable
    {
        Rectangle Size { get; }
    }
}