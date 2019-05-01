using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Globalization;
using System.Windows.Forms;
using Infotronix.TestApp;

namespace TestApp
{
    public partial class Form1 : Form
    {
        ReadDataFTP obj = new ReadDataFTP();
        public Form1()
        {
            InitializeComponent();
            this.Visible = false;
            this.ShowInTaskbar = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\downloadFile"))
            {
                DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\downloadFile");
                FileInfo[] Files = d.GetFiles(); 
                
                foreach (FileInfo file in Files)
                {
                    file.Delete();
                }
            }

            obj.ReadData();

            // check status for all device

            // objStatus.ReadStatus();

            Application.Exit();
        }
    }
}
