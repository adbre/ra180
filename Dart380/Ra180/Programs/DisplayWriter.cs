using System;

namespace Ra180.Programs
{
    public class DisplayWriter
    {
        private readonly Ra180Display _display;
        private string _current = "";

        public DisplayWriter(Ra180Display display)
        {
            _display = display;
        }

        public void Clear()
        {
            _current = "";
            _display.Clear();
        }

        public void Append(string s)
        {
            _current += s;
            Refresh();
        }

        public void SetText(string s)
        {
            _current = s;
            Refresh();
        }

        public void Refresh()
        {
            _display.SetText(_current);
        }

        public void CenterText(string text)
        {
            var totalWidth = _display.Length;
            var width = totalWidth - text.Length;
            var marginLeft = (int)Math.Floor(width/2.0);

            var centeredText = text.PadLeft(marginLeft + text.Length).PadRight(totalWidth);
            SetText(centeredText);
        }
    }
}