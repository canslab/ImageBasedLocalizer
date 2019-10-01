using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageLocalizer
{
    public partial class VideoLocalizationDialog : Form
    {
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        public VideoLocalizationDialog()
        {
            InitializeComponent();
        }

        private void VideoLocalizationDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            int width, height;
            bool parseOkay = false;

            parseOkay = int.TryParse(widthTextBox.Text, out width);
            if (parseOkay == false)
            {
                MessageBox.Show("Invalid width");
                return;
            }
            int.TryParse(heightTextBox.Text, out height);
            if (parseOkay == false)
            {
                MessageBox.Show("Invalid height");
                return;
            }

            MapWidth = width;
            MapHeight = height;
            Close();
        }
    }
}
