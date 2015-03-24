using System;

namespace Ra180
{
    internal class Ra180EditMenuItem : Ra180Program
    {
        private string _editValue;

        public Ra180EditMenuItem()
        {
            Prefix = () => string.Empty;

            MaxInputTextLength = () =>
            {
                var prefix = Prefix() ?? string.Empty;
                return Ra180.Display.Count - prefix.Length - 1;
            };

            CanEdit = () => false;
            SaveInput = text => false;
            GetValue = () => null;
            AcceptInput = (text, key) => true;
            IsDisabled = () => false;
        }

        public Func<string> Prefix { get; set; }
        public Func<int> MaxInputTextLength { get; set; }
        public Func<bool> CanEdit { get; set; }
        public Func<string, bool> SaveInput { get; set; }
        public Func<string> GetValue { get; set; }
        public Func<string, string, bool> AcceptInput { get; set; }
        public Func<string, bool> OnKey { get; set; }
        public Func<bool> IsDisabled { get; set; }

        public bool IsEditing { get; set; }

        public override bool Disabled
        {
            get { return IsDisabled(); }
        }

        public string EditValue
        {
            get { return _editValue ?? string.Empty; }
            set { _editValue = value; }
        }

        public override bool SendKey(string key)
        {
            var keyHandler = OnKey;
            if (keyHandler != null && keyHandler(key))
                return true;

            if (base.SendKey(key))
                return true;

            if (IsEditing)
            {
                if (key == Ra180Key.SLT)
                {
                    IsEditing = false;
                    return true;
                }

                if (key == Ra180Key.ÄND)
                {
                    if (EditValue.Length > 0)
                        EditValue = EditValue.Substring(0, EditValue.Length - 1);
                    return true;
                }

                if (key == Ra180Key.ENT)
                {
                    if (SaveInput(EditValue))
                        IsEditing = false;
                    else
                        EditValue = null;

                    return true;
                }

                if (AcceptInput(EditValue, key) && EditValue.Length < MaxInputTextLength())
                {
                    EditValue += key;
                    return true;
                }
            }
            else if (CanEdit() && key == Ra180Key.ÄND)
            {
                EditValue = null;
                IsEditing = true;
                return true;
            }

            return false;
        }

        public override void Display()
        {
            if (CanEdit())
            {
                if (IsEditing)
                {
                    Ra180.Display.SetText(Prefix() + ":" + EditValue);
                }
                else
                {
                    Ra180.Display.SetText(Prefix() + ":" + GetValue());
                }
            }
            else
            {
                Ra180.Display.SetText(Prefix() + "=" + GetValue());
            }
        }
    }
}