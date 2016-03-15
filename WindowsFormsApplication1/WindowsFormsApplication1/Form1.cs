using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Loader.Load();
            //MessageBox.Show("App Started","CleanAndTrack Dialog",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Environment.Exit(-1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loader.CleanUp();
            //MessageBox.Show("Desktop Cleaned", "CleanAndTrack Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(-1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Loader.StartTrack();
            //MessageBox.Show("Information Logged", "CleanAndTrack Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
