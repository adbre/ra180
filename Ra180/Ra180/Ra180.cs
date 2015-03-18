using System;
using System.Collections.Generic;
using System.Linq;

namespace Ra180
{
    public class Ra180
    {
        internal const int SELFTEST_INTERVAL = 2000;
        internal const int SELFTEST = SELFTEST_INTERVAL * 4;

        private readonly ISynchronizationContext _synchronizationContext;
        private readonly LedDisplay _display = new LedDisplay(8);

        private Ra180Mod _mod;
        private Ra180Channel _channel;
        private Ra180Volume _volume;
        private bool _online;

        private Ra180Clock _clock;

        public Ra180(ISynchronizationContext synchronizationContext)
        {
            if (synchronizationContext == null) throw new ArgumentNullException("synchronizationContext");
            _synchronizationContext = synchronizationContext;
        }

        public LedDisplay Display { get { return _display; } }

        public Ra180Mod Mod
        {
            get { return _mod; }
            set { SendKey(Ra180Converter.ToKeyCode(value)); }
        }

        public Ra180Channel Channel
        {
            get { return _channel; }
            set { SendKey(Ra180Converter.ToKeyCode(value));}
        }

        public Ra180Volume Volume
        {
            get { return _volume; }
            set { SendKey(Ra180Converter.ToKeyCode(value)); }
        }

        internal Ra180Clock Clock
        {
            get { return _clock; }
        }

        public void SendKeys(params Ra180KeyCode[] keys)
        {
            foreach (var key in keys)
                SendKey(key);
        }

        public void SendKey(Ra180KeyCode key)
        {
            SendKey(new Ra180KeyEventArgs {KeyCode = key});
        }

        public void SendKey(Ra180KeyEventArgs args)
        {
            if (IsModKey(args.KeyCode))
            {
                SetMod(Ra180Converter.ToMod(args.KeyCode));
                return;
            }

            if (_mod == Ra180Mod.FR)
                return;

            if (args.KeyCode.HasFlag(Ra180KeyCode.Asterix | Ra180KeyCode.NumberSign))
            {
                Reset();
                return;
            }

            if (args.KeyCode == Ra180KeyCode.BEL)
            {
                _display.ChangeBrightness();
                return;
            }

            if (!_online)
                return;

            if (_currentMenus.Count > 0)
            {
                var currentMenu = _currentMenus.Peek();
                if (currentMenu != null)
                {
                    currentMenu.SendKey(args);
                    currentMenu.Display();
                    return;
                }
            }

            var newMenu = GetMenu(args.KeyCode);
            if (newMenu != null)
            {
                _currentMenus.Push(newMenu);
                newMenu.Closed += (sender, eventArgs) => _currentMenus.Pop();
                newMenu.Display();
            }
        }

        private Ra180MenuItem GetMenu(Ra180KeyCode keyCode)
        {
            switch (keyCode)
            {
                case Ra180KeyCode.TID:
                    return new Ra180TidMenu(this);

                default:
                    return null;
            }
        }

        private void Reset()
        {
            RunSelfTest();
        }

        private void SetMod(Ra180Mod newMod)
        {
            if (newMod == _mod)
                return;

            if (newMod == Ra180Mod.FR)
            {
                Shutdown();
                return;
            }

            if (_mod == Ra180Mod.FR)
                Start();

            _mod = newMod;
        }

        private void Start()
        {
            RunSelfTest();
        }

        private void Shutdown()
        {
            Display.Clear();
            _mod = Ra180Mod.FR;
            _online = false;
        }

        private bool IsModKey(Ra180KeyCode keyCode)
        {
            var modKeys = new[] {Ra180KeyCode.ModFR, Ra180KeyCode.ModKLAR, Ra180KeyCode.ModSKYDD, Ra180KeyCode.ModDRELÄ};
            return modKeys.Contains(keyCode);
        }

        private void RunSelfTest()
        {
            _online = false;
            Display.SetText("TEST");
            _synchronizationContext.Schedule(() =>
            {
                if (_mod == Ra180Mod.FR)
                    return;

                Display.SetText("TEST OK");
                _synchronizationContext.Schedule(() =>
                {
                    if (_mod == Ra180Mod.FR)
                        return;

                    Display.SetText("NOLLST");
                    _synchronizationContext.Schedule(() =>
                    {
                        Display.Clear();
                        
                        if (_clock == null)
                            _clock = new Ra180Clock(this, _synchronizationContext);

                        _online = true;
                    }, SELFTEST_INTERVAL);
                }, SELFTEST_INTERVAL);
            }, SELFTEST_INTERVAL);
        }

        private Stack<Ra180MenuItem> _currentMenus = new Stack<Ra180MenuItem>();

        public void RefreshDisplay()
        {
            if (_currentMenus.Count <= 0) return;
            var currentMenu = _currentMenus.Peek();
            if (currentMenu == null) return;
            currentMenu.Display();
        }
    }

    public class Ra180Clock
    {
        private readonly Ra180 _ra180;
        private readonly ISynchronizationContext _synchronizationContext;
        private readonly object _token;

        public Ra180Clock(Ra180 ra180, ISynchronizationContext synchronizationContext)
        {
            if (synchronizationContext == null) throw new ArgumentNullException("synchronizationContext");
            _ra180 = ra180;
            _synchronizationContext = synchronizationContext;
            _token = _synchronizationContext.Repeat(Tick, 1000);

            Day = 1;
            Month = 1;
        }

        ~Ra180Clock()
        {
            _synchronizationContext.Cancel(_token);
        }

        public int Second { get; private set; }
        public int Minute { get; private set; }
        public int Hour { get; private set; }
        public int Day { get; private set; }
        public int Month { get; private set; }

        private void Tick()
        {
            Second += 1;

            if (Second >= 60)
            {
                Second = 0;
                Minute++;
            }

            if (Minute >= 60)
            {
                Minute = 0;
                Hour++;
            }

            if (Hour >= 24)
            {
                Hour = 0;
                Day++;
            }

            _ra180.RefreshDisplay();
        }

        public bool TrySetTime(string text)
        {
            return false;
        }

        public bool TrySetDate(string text)
        {
            throw new NotImplementedException();
        }

        public string GetTime()
        {
            return string.Format("{0:00}{1:00}{2:00}", Hour, Minute, Second);
        }

        public string GetDate()
        {
            return string.Format("{0:00}{1:00}", Month, Day);
        }
    }

    internal class Ra180TidMenu : Ra180MenuItemContainer
    {
        public Ra180TidMenu(Ra180 ra180) : base(ra180)
        {
            Title = "TID";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "T",
                MaxInputTextLength = () => 6,
                CanEdit = () => true,
                SaveInput = text =>
                {
                    if (text.Length == 6 && ra180.Clock.TrySetTime(text))
                        return true;

                    return false;
                },
                GetValue = () => ra180.Clock.GetTime()
            });

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "DAT",
                MaxInputTextLength = () => 4,
                CanEdit = () => true,
                SaveInput = text =>
                {
                    if (text.Length == 4 && ra180.Clock.TrySetDate(text))
                        return true;

                    return false;
                },
                GetValue = () => ra180.Clock.GetDate()
            });
        }
    }

    internal class Ra180EditMenuItem : Ra180MenuItem
    {
        public Ra180EditMenuItem()
        {
            Prefix = () => string.Empty;

            MaxInputTextLength = () =>
            {
                var prefix = Prefix() ?? string.Empty;
                return Ra180.Display.Count - prefix.Length - 1;
            };

            CanEdit = () => false;
            SaveInput = text => true;
            GetValue = () => null;
        }

        public Func<string> Prefix { get; set; }
        public Func<int> MaxInputTextLength { get; set; }
        public Func<bool> CanEdit { get; set; }
        public Func<string, bool> SaveInput { get; set; }
        public Func<string> GetValue { get; set; }

        public override void Display()
        {
            if (CanEdit())
            {
                Ra180.Display.SetText(Prefix() + ":" + GetValue());
            }
            else
            {
                Ra180.Display.SetText(Prefix() + "=" + GetValue());
            }
        }
    }

    internal abstract class Ra180MenuItemContainer : Ra180MenuItem
    {
        private readonly List<Ra180MenuItem> _children = new List<Ra180MenuItem>();
        private int _currentMenuItemIndex = 0;

        protected Ra180MenuItemContainer(Ra180 ra180) : base(ra180)
        {
        }

        public string Title { get; set; }

        public IList<Ra180MenuItem> Children { get { return _children; } }

        public void AddChild(Ra180MenuItem child)
        {
            child.Ra180 = Ra180;
            _children.Add(child);
        }

        public override bool SendKey(Ra180KeyEventArgs e)
        {
            if (e.KeyCode == Ra180KeyCode.ENT)
            {
                _currentMenuItemIndex++;
                if (_currentMenuItemIndex > _children.Count)
                    Close();

                return true;
            }

            if (_currentMenuItemIndex < _children.Count)
            {
                var currentMenuItem = _children[_currentMenuItemIndex];
                return currentMenuItem.SendKey(e);
            }

            return base.SendKey(e);
        }

        public override void Display()
        {
            if (_currentMenuItemIndex < _children.Count)
            {
                var currentMenuItem = _children[_currentMenuItemIndex];
                currentMenuItem.Display();
                return;
            }

            Ra180.Display.SetText(string.Format("({0})", Title));
        }
    }

    internal abstract class Ra180MenuItem
    {

        protected Ra180MenuItem(Ra180 ra180)
        {
            Ra180 = ra180;
        }

        protected Ra180MenuItem()
        {
        }

        public Ra180 Ra180 { get; set; }

        public virtual bool SendKey(Ra180KeyEventArgs e)
        {
            if (e.KeyCode == Ra180KeyCode.SLT)
            {
                Close();
                return true;
            }

            return false;
        }

        public event EventHandler Closed;

        public virtual void Display()
        {
        }

        public virtual void Close()
        {
            OnClosed();
        }

        protected virtual void OnClosed()
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    public class Ra180KeyEventArgs
    {
        public Ra180KeyCode KeyCode { get; set; }
    }

    public enum Ra180Mod
    {
        FR,
        KLAR,
        SKYDD,
        DRELÄ
    }
}