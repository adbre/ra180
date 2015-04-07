using System;

namespace Ra180.UI
{
    public interface IGraphic : IDisposable
    {
        IBitmapDrawable GetDrawable(string id);
        void Invalidate();
    }
}