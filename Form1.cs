using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIRC
{
    public partial class Form1 : Form
    {
        FIRCKiller fk = new FIRCKiller();
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_pid.Text))
            {
                return;
            }
            uint pid;
            if (uint.TryParse(tb_pid.Text, out pid))
            {
                if(fk.KillPID(pid))
                {
                    tb_pid.Clear();
                }
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fk.StartService();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            fk.StopService();
        }
    }
}
