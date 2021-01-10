using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ppvis2_wf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.StartSimulation();
            label3.Text = Program.PrintBases();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.simulator.Simlate(1);
        }

        private void label1_Click(object sender, EventArgs e)
        {
           label3.Text= Program.PrintBases();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label3.Text = Program.PrintCars();
        }
    }
}
