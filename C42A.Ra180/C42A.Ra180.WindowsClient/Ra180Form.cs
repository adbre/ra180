using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using C42A.Ra180.Infrastructure;

namespace C42A.Ra180.WindowsClient
{
    public partial class Ra180Form : Form
    {
        private Ra180Unit _ra180;
        private readonly SynchronizationContext _synchronizationContext;

        public Ra180Form()
        {
            _synchronizationContext = SynchronizationContext.Current;
            InitializeComponent();

            Bind(new Ra180Unit());
        }

        private void Bind(Ra180Unit unit)
        {
            if (unit == null) throw new ArgumentNullException("unit");
            _ra180 = unit;
            _ra180.DisplayChanged += (sender, args) => _synchronizationContext.Send(state => Display.Text = _ra180.Display, null);
            Display.Text = _ra180.Display;
            ResetHardwareKeysToRa180Values();
        }

        private void ResetHardwareKeysToRa180Values()
        {
            SelectHardwareKey(_ra180.Kanal, new []{Kanal1, Kanal2, Kanal3, Kanal4, Kanal5, Kanal6, Kanal7, Kanal8});
            SelectHardwareKey(_ra180.Volym, new []{Volym1, Volym2, Volym3, Volym4, Volym5, Volym6, Volym7, Volym8});
            SelectHardwareKey(((int)_ra180.Mod) + 1, new []{ModFrån, ModKlar, ModSkydd, ModDRelä});
        }

        private void SelectHardwareKey(int value, IList<RadioButton> radioButtons)
        {
            var index = value - 1;
            for (var i = 0; i < radioButtons.Count; i++)
            {
                var radioButton = radioButtons[i];
                radioButton.Checked = index == i;
            }
        }

        private void Nollställn_Click(object sender, EventArgs e)
        {
            _ra180.SendKeys(Ra180Knapp.KnappAsterix|Ra180Knapp.KnappHashtag);
        }

        private void Knapp_Click(object sender, EventArgs e)
        {
            var senderControl = sender as Control;
            if (senderControl == null) return;

            var senderName = senderControl.Name;
            if (string.IsNullOrWhiteSpace(senderName)) return;

            Ra180Knapp knapp;
            if (!Enum.TryParse(senderName, true, out knapp))
                return;

            _ra180.SendKeys(knapp);
        }
    }
}
