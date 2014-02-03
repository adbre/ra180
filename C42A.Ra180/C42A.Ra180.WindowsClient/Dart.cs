using System.Windows.Forms;
using C42A.Ra180.Infrastructure;

namespace C42A.Ra180.WindowsClient
{
    public partial class Dart : Form
    {
        public Dart()
        {
            InitializeComponent();
        }

        public Ra180Unit Ra180 { get; set; }
    }
}
