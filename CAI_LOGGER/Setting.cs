using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAI_LOGGER
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cdTBX.Enabled==true && lpTBX.Enabled==true)
            {
                Form1.CDmax = Convert.ToInt32(cdTBX.Value);
                Form1.LPmax = Convert.ToInt32(lpTBX.Value);
                Form1 form = new Form1();
                form.Show();
                this.Hide();
            }
            else
            {
                if(passTBX.Text == "syazwi")
                {
                    cdTBX.Enabled = true;
                    lpTBX.Enabled = true;
                }
            }
        }
    }
}
