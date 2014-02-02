using System;
using System.Collections.Generic;
using System.Drawing;
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

            var taskFactory = new TaskFactory();
            var ra180 = new Ra180Unit(taskFactory);
            Bind(ra180);
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
            var knapp = GetRa180Knapp(sender);
            if (knapp == null) return;
            _ra180.SendKeys(knapp.Value);
        }

        private static Ra180Knapp? GetRa180Knapp(object control)
        {
            var senderControl = control as Control;
            if (senderControl == null) return null;

            var senderName = senderControl.Name;
            if (string.IsNullOrWhiteSpace(senderName)) return null;

            Ra180Knapp knapp;
            if (!Enum.TryParse(senderName, true, out knapp))
                return null;

            return knapp;
        }

        private void Knapp1_Paint(object sender, PaintEventArgs e)
        {
            var title = GetButtonTitle(sender);
            if (title == null) return;

            var control = sender as Control;
            if (control == null) return;

            const float padding = 3f;
            var width = control.Width - (padding * 2);
            var height = control.Height / 2;
            var rectangle = new RectangleF(padding, padding, width, height);

            using (var font = new Font(control.Font.FontFamily, control.Font.SizeInPoints * 0.75f))
            using (var brush = new SolidBrush(control.ForeColor))
            using (var format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(title, font, brush, rectangle, format);
            }
        }

        private string GetButtonTitle(object sender)
        {
            var knapp = GetRa180Knapp(sender);
            if (knapp == null) return null;

            switch (knapp)
            {
                case Ra180Knapp.Knapp1:
                    return "TID";
                case Ra180Knapp.Knapp2:
                    return "RDA";
                case Ra180Knapp.Knapp3:
                    return "DTM";
                case Ra180Knapp.Knapp4:
                    return "KDA";
                case Ra180Knapp.Knapp5:
                    return "NIV";
                case Ra180Knapp.Knapp6:
                    return "RAP";
                case Ra180Knapp.Knapp7:
                    return "NYK";
                case Ra180Knapp.Knapp9:
                    return "TJK";

                default:
                    return null;
            }
        }
    }
}
