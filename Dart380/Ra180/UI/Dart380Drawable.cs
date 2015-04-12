using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ra180.Programs;

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

        private HotArea _vredKanal;
        private HotArea _vredMod;
        private HotArea _vredVolym;

        private Ra180Channel _channel = Ra180Channel.Channel1;
        private Dart380Mod _mod = Dart380Mod.FR;
        private Ra180Volume _volume = Ra180Volume.Volume4;

        private readonly Dictionary<Ra180Channel, string> _channelVredResourceMap = new Dictionary<Ra180Channel, string>
        {
            {Ra180Channel.Channel1, Drawables.Vred1},
            {Ra180Channel.Channel2, Drawables.Vred2},
            {Ra180Channel.Channel3, Drawables.Vred3},
            {Ra180Channel.Channel4, Drawables.Vred4},
            {Ra180Channel.Channel5, Drawables.Vred5},
            {Ra180Channel.Channel6, Drawables.Vred6},
            {Ra180Channel.Channel7, Drawables.Vred7},
            {Ra180Channel.Channel8, Drawables.Vred8},
        };

        private readonly Dictionary<Dart380Mod, string> _modVredResourceMap = new Dictionary<Dart380Mod, string>
        {
            {Dart380Mod.FR, Drawables.Vred1},
            {Dart380Mod.TE, Drawables.Vred2},
            {Dart380Mod.KLAR, Drawables.Vred3},
            {Dart380Mod.SKYDD, Drawables.Vred4},
            {Dart380Mod.DRELÄ, Drawables.Vred5},
            {Dart380Mod.TD, Drawables.Vred6},
            {Dart380Mod.NG, Drawables.Vred7},
            {Dart380Mod.FmP, Drawables.Vred8},
        };

        private readonly Dictionary<Ra180Volume, string> _volumeVredResourceMap = new Dictionary<Ra180Volume, string>
        {
            {Ra180Volume.Volume1, Drawables.Vred1},
            {Ra180Volume.Volume2, Drawables.Vred2},
            {Ra180Volume.Volume3, Drawables.Vred3},
            {Ra180Volume.Volume4, Drawables.Vred4},
            {Ra180Volume.Volume5, Drawables.Vred5},
            {Ra180Volume.Volume6, Drawables.Vred6},
            {Ra180Volume.Volume7, Drawables.Vred7},
            {Ra180Volume.Volume8, Drawables.Vred8},
        };

        public Dart380Drawable(IGraphic graphic, IDart380 dart380)
        {
            _graphic = graphic;
            _dart380 = dart380;

            _bgImage = graphic.GetDrawable(Drawables.Dart380BackgroundImage);

            _largeDisplay = new Dart380Control(491, 95, 379, 29);
            _smallDisplay = new Dart380Control(911, 95, 294, 29);

            _vredKanal = new HotArea(1229, 400, 94, 94, () =>
            {
                _channel = Next(_channel);
                _dart380.SendKey(Ra180Key.From(_channel));
            });
            _vredMod = new HotArea(1229, 610, 94, 94, () =>
            {
                _mod = Next(_mod);
                _dart380.SendKey(Dart380Key.From(_mod));
            });
            _vredVolym = new HotArea(1229, 820, 94, 94, () =>
            {
                _volume = Next(_volume);
                _dart380.SendKey(Ra180Key.From(_volume));
            });

            _hotAreas.Add(new Dart380Button(Ra180Key.Channel1, 1219, 893, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel2, 1208, 855, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel3, 1220, 819, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel4, 1252, 796, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel5, 1291, 796, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel6, 1323, 819, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel7, 1335, 857, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Channel8, 1323, 895, 12, 26));

            _hotAreas.Add(new Dart380Button(Dart380Key.ModFRÅN, 1207, 683, 41, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModTE,   1196, 645, 24, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModKLAR, 1189, 599, 62, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModSKYDD,1209, 577, 74, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModDRELÄ,1287, 577, 66, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModTD,   1323, 609, 34, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModNG,   1333, 647, 30, 26));
            _hotAreas.Add(new Dart380Button(Dart380Key.ModFmP,  1323, 685, 40, 26));

            _hotAreas.Add(new Dart380Button(Ra180Key.Volume1, 1219, 473, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume2, 1208, 435, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume3, 1220, 398, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume4, 1252, 376, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume5, 1291, 376, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume6, 1323, 399, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume7, 1335, 437, 12, 26));
            _hotAreas.Add(new Dart380Button(Ra180Key.Volume8, 1323, 475, 12, 26));

            _hotAreas.Add(_vredKanal);
            _hotAreas.Add(_vredMod);
            _hotAreas.Add(_vredVolym);

            _hotAreas.AddRange(GetButtons());

            foreach (var btn in _hotAreas.OfType<Dart380Button>())
            {
                var btnCache = btn;
                btn.Clicked += (sender, args) => OnButtonDown(btnCache);
            }
        }

        public bool HighlightHotAreas { get; set; }

        public void OnDraw(ICanvas canvas)
        {
            if (!_hasMeasurements)
                ConfigureMeasurements(canvas.Size.Width, canvas.Size.Height);

            canvas.DrawBitmap(_bgImage, _dartRect);

            var fgColor = new Color(255, 200, 244, 0);

            foreach (var btn in _hotAreas.OfType<Dart380Button>().Where(btn => btn.IsPressed)) {
                canvas.DrawRectangle(btn.Rectangle, fgColor);
            }

            if (HighlightHotAreas)
            {
                foreach (var btn in _hotAreas)
                    canvas.DrawRectangle(btn.Rectangle, fgColor);
            }

            DrawDisplay(_dart380.LargeDisplay, _largeDisplay.Rectangle, fgColor, canvas);
            DrawDisplay(_dart380.SmallDisplay, _smallDisplay.Rectangle, fgColor, canvas);

            DrawBitmap(_channelVredResourceMap[_channel], _vredKanal.Rectangle, canvas);
            DrawBitmap(_modVredResourceMap[_mod], _vredMod.Rectangle, canvas);
            DrawBitmap(_volumeVredResourceMap[_volume], _vredVolym.Rectangle, canvas);
        }

        private void DrawBitmap(string id, Rectangle rectangle, ICanvas canvas)
        {
            using (var bitmap = _graphic.GetDrawable(id))
                canvas.DrawBitmap(bitmap, rectangle);
        }

        public void OnTouchEvent(MotionEventArgs e)
        {
            var handledDownEvent = false;
            foreach (var btn in _hotAreas)
            {
                if (e.Action == MotionEventActions.Down && !handledDownEvent && btn.Rectangle.Contains(e.X, e.Y))
                {
                    btn.OnTouchEventDown();
                    handledDownEvent = true;
                }
                else
                {
                    btn.OnTouchEventUp();
                }
            }
        }

        public void SizeChanged(SizeChangedEventArgs e)
        {
            ConfigureMeasurements(e.Width, e.Height);
        }

        public void Tick()
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

            Ra180Channel channel;
            if (Ra180Key.TryParseChannel(key, out channel))
                _channel = channel;

            Dart380Mod mod;
            if (Dart380Key.TryParseMod(key, out mod))
                _mod = mod;

            Ra180Volume volume;
            if (Ra180Key.TryParseVolume(key, out volume))
                _volume = volume;

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

        private static Ra180Channel Next(Ra180Channel channel)
        {
            switch (channel)
            {
                case Ra180Channel.Channel1: return Ra180Channel.Channel2;
                case Ra180Channel.Channel2: return Ra180Channel.Channel3;
                case Ra180Channel.Channel3: return Ra180Channel.Channel4;
                case Ra180Channel.Channel4: return Ra180Channel.Channel5;
                case Ra180Channel.Channel5: return Ra180Channel.Channel6;
                case Ra180Channel.Channel6: return Ra180Channel.Channel7;
                case Ra180Channel.Channel7: return Ra180Channel.Channel8;
                case Ra180Channel.Channel8: return Ra180Channel.Channel1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Dart380Mod Next(Dart380Mod channel)
        {
            switch (channel)
            {
                case Dart380Mod.FR: return Dart380Mod.TE;
                case Dart380Mod.TE: return Dart380Mod.KLAR;
                case Dart380Mod.KLAR: return Dart380Mod.SKYDD;
                case Dart380Mod.SKYDD: return Dart380Mod.DRELÄ;
                case Dart380Mod.DRELÄ: return Dart380Mod.TD;
                case Dart380Mod.TD: return Dart380Mod.NG;
                case Dart380Mod.NG: return Dart380Mod.FmP;
                case Dart380Mod.FmP: return Dart380Mod.FR;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Ra180Volume Next(Ra180Volume channel)
        {
            switch (channel)
            {
                case Ra180Volume.Volume1: return Ra180Volume.Volume2;
                case Ra180Volume.Volume2: return Ra180Volume.Volume3;
                case Ra180Volume.Volume3: return Ra180Volume.Volume4;
                case Ra180Volume.Volume4: return Ra180Volume.Volume5;
                case Ra180Volume.Volume5: return Ra180Volume.Volume6;
                case Ra180Volume.Volume6: return Ra180Volume.Volume7;
                case Ra180Volume.Volume7: return Ra180Volume.Volume8;
                case Ra180Volume.Volume8: return Ra180Volume.Volume1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
