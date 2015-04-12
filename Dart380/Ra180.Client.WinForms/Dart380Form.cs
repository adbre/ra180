using System.Windows.Forms;

namespace Ra180.Client.WinForms
{
    public partial class Dart380Form : Form
    {
        public Dart380Form()
        {
            InitializeComponent();
        }

        public IDart380 Dart380
        {
            get { return dart380Control1.Dart380; }
            set { dart380Control1.Dart380 = value; }
        }

        private void avslutaToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void omRa180Dart380SimuleringToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var form = new AboutBox1())
                form.ShowDialog(this);
        }

        private void ra180ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void dart380ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void inställningarToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var form = new SettingsForm())
            {
                form.ShowDialog(this);
            }
        }
    }
}
