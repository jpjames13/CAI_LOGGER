using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Configuration;

namespace CAI_LOGGER
{
    public partial class Form1 : Form
    {
        public static int LPmax = Convert.ToInt32(Setting.GetSetting("LPMAX"));
        public static int CDmax = Convert.ToInt32(Setting.GetSetting("CDMAX"));

        public static int LPcounter2 { get; set; }
        public static int CDcounter2 { get; set; }

        #region I hope I don't use these variables ever in my life
        public static int LPcounter { get; set; }
        public static int CDcounter { get; set; }
        #endregion

        public static int LPCreated;
        public static int CDCreated;
        public static int oldLPCreated;
        public static int oldCDCreated;


        FileSystemWatcher CDdirWatcher = new FileSystemWatcher();
        FileSystemWatcher LPdirWatcher = new FileSystemWatcher();

        public Form1()
        {
            InitializeComponent();
             
            watch();
            ////Implementing Threads asynchronously
            //Thread oThreadone = new Thread(() =>
            //{
            //    //Do what u wanna……
            //    watch();
            //});

            //Thread oThreadtwo = new Thread(() =>
            //{
            //    //Do what u wanna……
            //    checker();
            //});

            ////Calling thread workers
            //oThreadone.Start();
            //oThreadone.IsBackground = true;
            //oThreadtwo.Start();
            //oThreadtwo.IsBackground = true;
            checker();

            this.FormClosing += Form1_FormClosing;
        } 
        //Ignore
        #region
        //watch method should run in the background as checker
        public void watch()
        { 
            CDdirWatcher.Path = @"C:\Data\LotData\CD";
            CDdirWatcher.Filter = "EM*";
            CDdirWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.Attributes;
            CDdirWatcher.Created += CDdirWatcher_Created;
            CDdirWatcher.EnableRaisingEvents = true;
            
            LPdirWatcher.Path = @"C:\Data\LotData\LP";
            LPdirWatcher.Filter = "EM*";
            LPdirWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.Attributes;
            LPdirWatcher.Created += LPdirWatcher_Created;
            LPdirWatcher.EnableRaisingEvents = true;
            Thread.Sleep(50);
        }
        private static void CDdirWatcher_Created(object sender, FileSystemEventArgs e)
        { 
            CDCreated += 1;

            //if (CDCreated/2 > CDmax)
            //{
            //    popupbx();
            //    //Dispatcher.CurrentDispatcher.BeginInvoke((MethodInvoker)popupbx);
            //}
        }
        private static void LPdirWatcher_Created(object sender, FileSystemEventArgs e)
        { 
            LPCreated += 1;

            //if (LPCreated/2 > LPmax)
            //{
            //    popupbx();
            //    //Dispatcher.CurrentDispatcher.BeginInvoke((MethodInvoker)popupbx);
            //}
        }
        public static void popupbx()
        {
            popup form = new popup();
            form.TopMost = true;    
            form.Show(); 
        }

        #region     Handling Form close event
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (string.Equals((sender as Button).Name, @"CloseButton"))
        //        // Do something proper to CloseButton.
        //        e.Cancel = true;
        //    else
        //        // Then assume that X has been clicked and act accordingly.
        //        MessageBox.Show("Anda Pasti Anda Nak Close CAI LOGGER");
        //}

        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                // Prompt user to save his data
                //MessageBox.Show("Anda Pasti Anda Nak Close CAI LOGGER");
                switch (MessageBox.Show(this, "Anda Pasti Nak Tutup?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //Stay on this form
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            if (e.CloseReason == CloseReason.WindowsShutDown)
                MessageBox.Show("Anda Pasti Anda Nak Close CAI LOGGER");
                // Prompt user to save his data
                //MessageBox.Show("Anda Pasti Anda Nak Close CAI LOGGER");
                switch (MessageBox.Show(this, "Anda Pasti Nak Tutup?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //Stay on this form
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
        }


        //private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        //{
        //    //In case windows is trying to shut down, don't hold the process up
        //    if (e.CloseReason == CloseReason.WindowsShutDown) return;

        //    if (this.DialogResult == DialogResult.Cancel)
        //    {
        //        // Assume that X has been clicked and act accordingly.
        //        // Confirm user wants to close
        //        switch (MessageBox.Show(this, "Are you sure?", "Do you still want ... ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        //        {
        //            //Stay on this form
        //            case DialogResult.No:
        //                e.Cancel = true;
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        #endregion

        #region THE CHECKER METHOD THAT BECAME ASYNC AWAIT SAVIOR
        public async static void checker()
        { 
            while(true)
            { 
                if (CDCreated == CDmax)
                {   
                    popupbx();      
                    CDCreated = 0; 
                    //break;
                }
                if (LPCreated == LPmax)
                { 
                    popupbx();
                    LPCreated = 0;
                    //break;
                }
                await Task.Delay(4000);
            }
        }
        #endregion   

        private void OnChanged(object source, FileSystemEventArgs e)
        {

        }
        #endregion

        private void OnDirectoryChanged()
        {
            try
            {
                CDdirWatcher.EnableRaisingEvents = false; 
            }

            finally
            {
                CDdirWatcher.EnableRaisingEvents = true;
            }
        }

        #region WASTED 15 MINUTES OF MY LIFE ON THIS USELESS REGION
        //public void CDCounted()
        //{
        //    int retCD1 = retCDCount();
        //    int retCD2 = retCDCount();

        //    CDCreated = retCD2 - retCD1;

        //    CDCounted();
        //}
        //public void LPCounted()
        //{
        //    int retLP1 = retLPCount();
        //    int retLP2 = retLPCount();

        //    LPCreated = retLP2 - retLP1;

        //    LPCounted();
        //}
        #endregion


        public int retCDCount()
        {
            //Location of Files 
            var File = Directory.GetDirectories(@"C:\Data\LotData\CD", "EM*", SearchOption.AllDirectories); 
            CDcounter = File.Length;

            return CDcounter;
        }
        public int retLPCount()
        {
            //Location of Files 
            var File = Directory.GetDirectories(@"C:\Data\LotData\LP", "EM*", SearchOption.AllDirectories);
            LPcounter = File.Length;

            return LPcounter;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Setting form = new Setting();
            form.Show();
            this.Hide();
        }
    }
}
