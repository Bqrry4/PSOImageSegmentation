using PSOImageSegmentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pso = new PSOimage();
            var psoed = pso.runPSO(new Bitmap(Image.FromFile("./toji.jpg")));
            pictureBox1.Image = psoed;
        }
    }
}
