using com.eze.api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class TransactionDetails : Form
    {
        Form parent;
        public TransactionDetails(Form f,String t1,String t2,String t3,String t4,String t5,String t6)
        {
            InitializeComponent();
            this.Text = "EZETAP";
            this.parent = f;
            this.label7.Text = t1;
            this.label7.Show();
            this.label8.Text = t2;
            this.label8.Show();
            this.label9.Text = t3;
            this.label9.Show();
            this.label10.Text = t4;
            this.label10.Show();
            this.label11.Text = t5;
            this.label11.Show();
            this.label12.Text = t6;
            this.label12.Show();
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            EzeAPI.create().close();
            this.parent.Close();
            this.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.parent.Show();
        }

        private void TransactionDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
