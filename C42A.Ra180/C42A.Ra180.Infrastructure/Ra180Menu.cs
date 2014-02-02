using System;
using System.Collections.Generic;

namespace C42A.Ra180.Infrastructure
{
    internal abstract class Ra180Menu
    {
        private readonly Ra180Unit _unit;
        private string _input;
        private int _submenu;

        protected Ra180Menu(Ra180Unit unit)
        {
            if (unit == null) throw new ArgumentNullException("unit");
            _unit = unit;

            OnSubmenuChanged(0);
        }

        protected virtual string Title
        {
            get { return "????????"; }
        }

        protected virtual int Submenus
        {
            get { return 0; }
        }

        protected Ra180Unit Unit { get { return _unit; } }

        protected int Submenu
        {
            get { return _submenu; }
            set
            {
                _submenu = value > Submenus ? 0 : value;
                OnSubmenuChanged(_submenu);
            }
        }

        protected string Input
        {
            get { return _input; }
            private set
            {
                _input = value; 
                OnInputChanged(_input);
            }
        }

        protected virtual void OnInputChanged(string input)
        {
        }

        public virtual void HandleKeys(Ra180Knapp knapp)
        {
            switch (knapp)
            {
                case Ra180Knapp.ÄND:
                    OnÄND();
                    break;
                case Ra180Knapp.ENT:
                    OnRETURN();
                    break;
                case Ra180Knapp.SLT:
                    OnSLT();
                    break;
                default:
                    OnNumpadKey(knapp);
                    break;
            }
        }

        protected virtual void OnÄND()
        {
            StartCaptureInput();
        }

        protected virtual void OnRETURN()
        {
            var input = Input;
            if (input != null)
                ConfirmInput();
            else
                NextSubmodule();
        }

        protected virtual void OnSLT()
        {
            var input = Input;
            if (input == null)
            {
                CloseMenu();
                return;
            }

            Input = null;
            OnSubmenuChanged(Submenu);
        }

        protected virtual void NextSubmodule()
        {
            var submodule = Submenu + 1;
            if (submodule > Submenus)
            {
                CloseMenu();
                return;
            }

            Submenu++;
        }

        protected virtual void CloseMenu()
        {
            Unit.CloseMenu();
        }

        protected virtual void ConfirmInput()
        {
            var success = TrySubmitInput(Input);
            if (success) 
                _input = null;
            else
                Input = "";
        }

        protected virtual bool TrySubmitInput(string input)
        {
            return true;
        }

        protected virtual void StartCaptureInput()
        {
            Input = "";
        }

        protected virtual void OnNumpadKey(Ra180Knapp knapp)
        {
            var numpad = new Dictionary<Ra180Knapp, char>
            {
                {Ra180Knapp.Knapp0, '0'},
                {Ra180Knapp.Knapp1, '1'},
                {Ra180Knapp.Knapp2, '2'},
                {Ra180Knapp.Knapp3, '3'},
                {Ra180Knapp.Knapp4, '4'},
                {Ra180Knapp.Knapp5, '5'},
                {Ra180Knapp.Knapp6, '6'},
                {Ra180Knapp.Knapp7, '7'},
                {Ra180Knapp.Knapp8, '8'},
                {Ra180Knapp.Knapp9, '9'},
            };

            foreach (var keymap in numpad)
            {
                if (keymap.Key != knapp) continue;
                OnNumpadKey(keymap.Value);
            }
        }

        protected virtual void OnNumpadKey(char key)
        {
            var input = Input;
            if (input == null) return;
            input += key;
            Input = input;
        }

        protected virtual void OnSubmenuChanged(int submenu)
        {
            if (Input != null) return;
            var displayText = FormatDisplay(submenu);
            Unit.SetDisplay(displayText);
        }

        protected virtual string FormatDisplay(int submenu)
        {
            if (submenu == Submenus)
                return string.Format("  ({0}) ", Title);
            else
                return null;
        }
    }
}