using System;
using Ra180.Programs;

namespace Ra180
{
    public class Ra180ComboBoxEditMenuItem : Ra180EditMenuItem
    {
        public Ra180ComboBoxEditMenuItem(params string[] args) : this()
        {
            GetValues = () => args;
        }

        public Ra180ComboBoxEditMenuItem()
        {
            CanEdit = () =>
            {
                var values = Safe(GetValues);
                return values != null && values.Length > 1;
            };

            OnSelectedItem = (value, index) =>
            {
                var onSelectedIndex = OnSelectedIndex;
                if (onSelectedIndex != null)
                    onSelectedIndex(index);

                var onSelectedValue = OnSelectedValue;
                if (onSelectedValue != null)
                    onSelectedValue(value);
            };

            GetSelectedIndex = () =>
            {
                var selectedValue = Safe(GetSelectedValue);
                var values = Safe(GetValues);
                if (values == null || values.Length < 1 || selectedValue == null)
                    return -1;

                for (var i = 0; i < values.Length; i++)
                {
                    if (values[i] == selectedValue)
                        return i;
                }

                return -1;
            };

            IsChangeKey = key => key == Ra180Key.ÄND;

            base.OnKey = key =>
            {
                if (!IsChangeKey(key))
                    return false;

                SelectNextValue();
                return true;
            };

            GetValue = () =>
            {
                var selectedIndex = Safe(GetSelectedIndex);

                var values = Safe(GetValues);
                if (values == null || values.Length < 1)
                    return null;

                if (selectedIndex < 0 || selectedIndex >= values.Length)
                    return null;

                return values[selectedIndex];
            };
        }

        public Action<string, int> OnSelectedItem;
        public Action<int> OnSelectedIndex;
        public Action<string> OnSelectedValue;
        public Func<string[]> GetValues;
        public Func<int> GetSelectedIndex;
        public Func<string> GetSelectedValue;
        public new Func<string, bool> OnKey;
        public Func<string, bool> IsChangeKey;

        protected void SelectNextValue()
        {
            var values = Safe(GetValues);
            if (Safe(CanEdit) && values != null && values.Length > 0)
            {
                var selectedIndex = Safe(GetSelectedIndex);
                selectedIndex++;

                if (selectedIndex >= values.Length)
                    selectedIndex = 0;

                var selectedValue = values[selectedIndex];

                var onSelectedItem = OnSelectedItem;
                if (onSelectedItem != null)
                    onSelectedItem(selectedValue, selectedIndex);
            }
        }

        private TResult Safe<TResult>(Func<TResult> func)
        {
            return (func ?? (() => default(TResult)))();
        }
    }
}