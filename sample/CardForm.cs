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
    public partial class CardForm : Form
    {
        Form parent;
        Form login;
        public CardForm(Form f,Form login)
        {
            InitializeComponent();
          parent = f;
            this.login = login;
            this.Text = "EZETAP";
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            EzeAPI.create().close();
            this.parent.Close();
            this.Close();

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Reference_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //Home Button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            parent.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OptionalParams p = new OptionalParams();
            String mobileNumber = this.textBox4.Text;
            if (mobileNumber != null)
            {
                Customer cust = new Customer();
                cust.setMobileNumber(mobileNumber);
                p.setCustomer(cust);
            }
            EzeResult res = EzeAPI.create().cardTransaction(20.0,PaymentMode.SALE,p);
            Console.WriteLine("666666.... "+res.getStatus());
            if (res.getStatus() == Status.SUCCESS)
            {
                // f3 = new Form3(parent, "23");
                
                if (res.getResult() != null)
                {
                    com.eze.api.TransactionDetails td = res.getResult().getTransactionDetails();
                    Form3 f3 = new Form3(parent,td.getTxnId(),p.getCustomer(),parent);
                    f3.Show();
                }
            }
            else
            {
                MessageBox.Show(res.getError().getMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EzeAPI.create().close();
            this.Close();
            login.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EzeAPI.create().close();
            this.Close();
            this.login.Show();
           
        }
    }
}
