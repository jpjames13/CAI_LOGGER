using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

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



        #region     Configurations
        public static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key]; 
        }

        public static void SetSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion



        private void button1_Click(object sender, EventArgs e)
        {
            if(cdTBX.Enabled==true && lpTBX.Enabled==true)
            {
                SetSetting("CDMAX", Convert.ToString(cdTBX.Value));
                SetSetting("LPMAX", Convert.ToString(lpTBX.Value));
                //Form1.CDmax = Convert.ToInt32(cdTBX.Value);
                //Form1.LPmax = Convert.ToInt32(lpTBX.Value);
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
