using System;

namespace Ra180.Programs
{
    public static class Ra180DisplayExtensions
    {
        public static void CenterText(this Ra180Display display, string text)
        {
            if (display == null) throw new ArgumentNullException("display");
            if (text == null) throw new ArgumentNullException("text");

            var totalWidth = display.Length;
            var width = totalWidth - text.Length;
            var marginLeft = (int) Math.Floor(width/2.0);

            var centeredText = text.PadLeft(marginLeft + text.Length).PadRight(totalWidth);
            display.SetText(centeredText);
        }
    }
}