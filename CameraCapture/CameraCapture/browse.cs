using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CameraCapture
{
    public partial class detail : Form
    {
        public detail()
        {
            InitializeComponent();
            string user_name = Environment.UserName;
            string machine_name = Environment.MachineName;
            string processor_name = Environment.ProcessorCount.ToString();
            bool is64bit = Environment.Is64BitOperatingSystem;
            string operatingsystem;
            if (is64bit)
            {
                operatingsystem = "64-bit Operatring system";
            }
            else
            {
                operatingsystem = "32-bit Operatring system";
            }
            string osversion_platform = System.Environment.OSVersion.Platform.ToString();
            string osversion_servicepack = System.Environment.OSVersion.ServicePack;
            string osversion = System.Environment.OSVersion.ToString();
            string[] system_data = { user_name, machine_name, processor_name, operatingsystem, osversion_platform, osversion_servicepack, osversion };
            
            label1.Text = "User Name : " + user_name;
            label2.Text = "Machine Name : " + machine_name;
            label3.Text = "Processor : " +  processor_name;
            label4.Text = "Operating System : " + operatingsystem;
            label5.Text = "OS Version Platform : "+ osversion_platform;
            label6.Text = "OS Version : " + osversion;
        }

        private void detail_Load(object sender, EventArgs e)
        {
            
        }
    }
}
