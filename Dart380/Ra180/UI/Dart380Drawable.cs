using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ra180.Devices.Dart380;

namespace Ra180.UI
{
    public class Dart380Drawable : IDisposable
    {
        private readonly IDart380 _dart380;
        private readonly IGraphic _graphic;
        private readonly IBitmapDrawable _bgImage;
        private readonly List<HotArea> _hotAreas = new List<HotArea>();

        private bool _hasMeasurements;
        private Rectangle _dartRect;
        private Dart380Control _largeDisplay;
        private Dart380Control _smallDisplay;
        private bool _blinkNow;

        public Dart380Drawable(IGraphic graphic, IDart380 dart380)
        {
            _graphic = graphic;
            _dart380 = dart380;

            _bgImage = graphic.GetDrawable(Drawables.Dart380BackgroundImage);

            _largeDisplay = new Dart380Control(491, 95, 379, 29);
            _smallDisplay = new Dart380Control(911, 95, 294, 29);

            _hotAreas.AddRange(GetButtonHotAreas());
        }

        public void OnDraw(ICanvas canvas)
        {
            if (!_hasMeasurements)
                ConfigureMeasurements(canvas.Size.Width, canvas.Size.Height);

            canvas.DrawBitmap(_bgImage, _dartRect);

            var fgColor = new Color(255, 200, 244, 0);

            foreach (var btn in _hotAreas.OfType<Dart380Button>().Where(btn => btn.IsPressed)) {
                canvas.DrawRectangle(btn.Rectangle, fgColor);
            }

            foreach (var btn in _hotAreas.OfType<Dart380Button>())
            {
                canvas.DrawRectangle(btn.Rectangle, fgColor);
            }

            DrawDisplay(_dart380.LargeDisplay, _largeDisplay.Rectangle, fgColor, canvas);
            DrawDisplay(_dart380.SmallDisplay, _smallDisplay.Rectangle, fgColor, canvas);
        }

        public void OnTouchEvent(MotionEventArgs e)
        {
            foreach (var btn in _hotAreas)
                btn.OnTouchEvent(e);
        }

        public void SizeChanged(SizeChangedEventArgs e)
        {
            ConfigureMeasurements(e.Width, e.Height);
        }

        public void ToggleBlinking()
        {
            _blinkNow = !_blinkNow;
            _graphic.Invalidate();
        }

        public void OnKeyPress(char keyChar)
        {
            var key = keyChar.ToString().ToUpper();

            foreach (var btn in _hotAreas.OfType<Dart380Button>())
            {
                if (btn.PrimaryKey == key || btn.SecondaryKey == key)
                    _dart380.SendKey(key);
            }
        }

        private void ConfigureMeasurements(float width, float height)
        {
            _dartRect = _bgImage.Size.ScaleToFit(width, height);
            var scale = _bgImage.Size.GetScale(width, height);

            _largeDisplay.Scale(scale, _dartRect);
            _smallDisplay.Scale(scale, _dartRect);

            foreach (var ha in _hotAreas) {
                ha.Scale(scale, _dartRect);
            }

            _hasMeasurements = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _graphic.Dispose();
            _bgImage.Dispose();
        }

        private void DrawDisplay(Ra180Display display, Rectangle rect, Color fgColor, ICanvas canvas)
        {
            var text = new StringBuilder();
            var underscore = new StringBuilder();

            foreach (var c in display.Characters)
            {
                if (c.IsBlinking && _blinkNow)
                {
                    text.Append(' ');
                    underscore.Append(' ');
                    continue;
                }

                text.Append(c.IsBlinking && _blinkNow ? ' ' : c.Char);
                underscore.Append(c.HasUnderscore ? '-' : ' ');
            }

            var textSize = _largeDisplay.Rectangle.Height;
            canvas.DrawText(text.ToString(), rect.Left, rect.Bottom, textSize, fgColor);
            canvas.DrawText(underscore.ToString(), rect.Left, rect.Bottom + rect.Height / 2, textSize, fgColor);
        }

        private IEnumerable<HotArea> GetButtonHotAreas()
        {
            return GetButtons()
                .Select(btn =>
                {
                    btn.Callback = () => OnButtonDown(btn);
                    return btn;
                });
        }

        private void OnButtonDown(Dart380Button button)
        {
            var shiftKey = _hotAreas
                .OfType<Dart380Button>()
                .Single(btn => btn.PrimaryKey == "SHIFT");

            if (button == shiftKey)
            {
                return;
            }

            var key = button.PrimaryKey;
            if (shiftKey.IsPressed)
            {
                if (!string.IsNullOrEmpty(button.SecondaryKey))
                {
                    key = button.SecondaryKey;
                }
                shiftKey.IsPressed = false;
            }

            _dart380.SendKey(key);
        }

        private static IEnumerable<Dart380Button> GetButtons()
        {
            // The coordinates assumes the Dart380 image is 1363x1018 pixels in size.
            return new List<Dart380Button> {
				new Dart380Button("F1", 143,353, 61,59),
				new Dart380Button("F2", 224,353, 61,59),
				new Dart380Button("F3", 306,353, 61,59),
				new Dart380Button("F4", 388,353, 61,59),
				new Dart380Button("FMT", 470,353, 61,59),
				new Dart380Button("REP", 143,435, 61,59),
				new Dart380Button("RAD", 224,435, 61,59),
				new Dart380Button("KVI", 306,435, 61,59),
				new Dart380Button("SKR", 388,435, 61,59),
				new Dart380Button("DDA", 470,435, 61,59),
				new Dart380Button("SND", 143,516, 61,59),
				new Dart380Button("EKV", 224,516, 61,59),
				new Dart380Button("MOT", 306,516, 61,59),
				new Dart380Button("AVS", 388,516, 61,59),
				new Dart380Button("ISK", 470,516, 61,59),
				new Dart380Button("OPM", 604,435, 61,59),
				new Dart380Button("EFF", 686,435, 61,59),
				new Dart380Button("ÄND", 767,435, 61,59),
				new Dart380Button("BEL", 604,516, 61,59),
				new Dart380Button("SLT", 686,516, 61,59),
				new Dart380Button("ENT", 767,516, 61,59),
				new Dart380Button("1", 879,272, 61,59),
				new Dart380Button("2", 961,272, 61,59),
				new Dart380Button("3", 1042,272, 61,59),
				new Dart380Button("4", 879,353, 61,59),
				new Dart380Button("5", 961,353, 61,59),
				new Dart380Button("6", 1042,353, 61,59),
				new Dart380Button("7", 879,435, 61,59),
				new Dart380Button("8", 961,435, 61,59),
				new Dart380Button("9", 1042,435, 61,59),
				new Dart380Button("*", 879,516, 61,59),
				new Dart380Button("0", 961,516, 61,59),
				new Dart380Button("#", 1042,516, 61,59),
				new Dart380Button("Q", "!", 143,639, 61,59),
				new Dart380Button("W", "\"", 224,639, 61,59),
				new Dart380Button("E", "#", 306,639, 61,59),
				new Dart380Button("R", "@", 388,639, 61,59),
				new Dart380Button("T", "%", 470,639, 61,59),
				new Dart380Button("Y", "&", 551,639, 61,59),
				new Dart380Button("U", "/", 632,639, 61,59),
				new Dart380Button("I", "(", 714,639, 61,59),
				new Dart380Button("O", ")", 796,639, 61,59),
				new Dart380Button("P", "=", 877,639, 61,59),
				new Dart380Button("Å", "+", 958,639, 61,59),
				new Dart380Button("DEL", 1040,639, 61,59),
				new Dart380Button("A", 184,721, 61,59),
				new Dart380Button("S", 266,721, 61,59),
				new Dart380Button("D", 347,721, 61,59),
				new Dart380Button("F", 429,721, 61,59),
				new Dart380Button("G", "´", 510,721, 61,59),
				new Dart380Button("H", "^", 592,721, 61,59),
				new Dart380Button("J", "$", 673,721, 61,59),
				new Dart380Button("K", "<", 755,721, 61,59),
				new Dart380Button("L", ">", 837,721, 61,59),
				new Dart380Button("Ö", "*", 918,721, 61,59),
				new Dart380Button("Ä", "?", 1000,721, 61,59),
				new Dart380Button("SHIFT", 143,802, 61,59) { Style = Dart380Button.ButtonStyle.Toggle },
				new Dart380Button("Z", 224,802, 61,59),
				new Dart380Button("X", 306,802, 61,59),
				new Dart380Button("C", 388,802, 61,59),
				new Dart380Button("V", 470,802, 61,59),
				new Dart380Button("B", 551,802, 61,59),
				new Dart380Button("N", 632,802, 61,59),
				new Dart380Button("M", 714,802, 61,59),
				new Dart380Button(",", ";", 796,802, 61,59),
				new Dart380Button(".", ":", 877,802, 61,59),
				new Dart380Button("-", "_", 958,802, 61,59),
				new Dart380Button("ENT", 1040,802, 61,59),
				new Dart380Button("LARROW", 224,884, 61,59),
				new Dart380Button("RARROW", 306,884, 61,59),
				new Dart380Button("UARROW", 388,884, 61,59),
				new Dart380Button("DARROW", 470,884, 61,59),
				new Dart380Button("SPACE", 551,884, 307,59),
				new Dart380Button("PGUP", 877,884, 61,59),
				new Dart380Button("PGDOWN", 958,884, 61,59),
			};
        }
    }
}
