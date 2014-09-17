using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADNtestApp
{
    public partial class LockCode : Form
    {
        public LockCode()
        {
            InitializeComponent();
        }

        public bool bUpdateLic = false;

        private void LockCode_Load(object sender, EventArgs e)
        {
            if (bUpdateLic == false)
            {
                button2.Visible = false;
            }
            else
            {
                button2.Visible = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
