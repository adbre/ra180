using System;
using System.Windows.Forms;

namespace Ra180.Client.WinForms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var serverAddress = textBox1.Text;
            Uri uri;
            if (!Uri.TryCreate(serverAddress, UriKind.Absolute, out uri))
            {
                MessageBox.Show(this, @"Serveradress måste vara en giltig URI");
                return;
            }

            Properties.Settings.Default.Server = serverAddress;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
