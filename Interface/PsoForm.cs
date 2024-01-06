using PSOImageSegmentation;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Interface
{
    public partial class PsoForm : Form
    {
        public PsoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pso = new PSOimage();
            var psoed = pso.runPSO(new Bitmap(Image.FromFile("./toji.jpg")));
            resultPictureBox.Image = psoed;
        }
    }
}
