using System;
using System.Collections.Generic;

namespace Ra180
{
    internal abstract class Ra180MenuProgram : Ra180Program
    {
        private readonly List<Ra180Program> _children = new List<Ra180Program>();
        private bool _initialized;
        private int _currentMenuItemIndex = -1;
        private Ra180Program _currentChild;

        protected Ra180MenuProgram(Ra180 ra180, Ra180Display display) : base(ra180, display)
        {
        }

        public IList<Ra180Program> Children { get { return _children; } }

        protected int CurrentChildIndex
        {
            get { return _currentMenuItemIndex; }
            set { _currentMenuItemIndex = value; }
        }

        protected Ra180Program CurrentChild
        {
            get
            {
                if (CurrentChildIndex >= _children.Count)
                    return null;

                return _children[CurrentChildIndex];
            }
            set
            {
                var index = _children.IndexOf(value);
                if (index < 0)
                    throw new ArgumentException("program is not a child of mine", "value");

                _currentMenuItemIndex = index;
            }
        }

        public void AddChild(Ra180Program child)
        {
            child.Ra180 = Ra180;
            child.Display = Display;
            _children.Add(child);
        }

        public override bool SendKey(string key)
        {
            if (_currentMenuItemIndex < _children.Count)
            {
                var currentMenuItem = _children[_currentMenuItemIndex];
                if (currentMenuItem.SendKey(key))
                    return true;
            }

            if (key == Ra180Key.ENT)
            {
                NextChild();
                return true;
            }

            if (key == Ra180Key.SLT)
            {
                Close();
                return true;
            }

            return base.SendKey(key);
        }

        private void NextChild()
        {
            _currentMenuItemIndex++;
            if (_currentMenuItemIndex > _children.Count)
            {
                Close();
                return;
            }

            if (_currentMenuItemIndex == _children.Count)
                return;

            var child = _children[_currentMenuItemIndex];
            if (child.Disabled)
                NextChild();
        }

        public override void UpdateDisplay()
        {
            if (!_initialized)
            {
                NextChild();
                _initialized = true;
            }

            if (!IsClosed && CurrentChild != null)
            {
                CurrentChild.UpdateDisplay();
                return;
            }

            base.UpdateDisplay();
        }
    }
}