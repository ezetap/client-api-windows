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
    public partial class MenuForm : Form
    {
        Form  parent;
        Form login;
        public MenuForm(Form f)
        {
            InitializeComponent();
            this.parent = f;
            this.Text = "EZETAP";
            this.login = f;
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           EzeAPI.create().close();
            this.parent.Show();
         this.Close();

        }

        //Card
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            CardForm card = new CardForm(this,login);
            card.Show();
        }
        //Cash
        private void button2_Click(object sender, EventArgs e)
        {

            Cash cash = new Cash(this,login);
            cash.Show();
            this.Hide();
        }

        //Get Transaction Details
        private void button4_Click(object sender, EventArgs e)
        {

            TransactionFetchForm form = new TransactionFetchForm(this);
            this.Hide();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EzeAPI.create().close();
            this.Close();
            parent.Show();
            
        }
        //Cheque
        private void button3_Click(object sender, EventArgs e)
        {
            ChequeForm form = new ChequeForm(this,login);
            form.Show();
            this.Hide();
        }
    }
}
