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
    public partial class Cash : Form
    {
        Form parent;
        Form login;
        public Cash(Form f,Form login)
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

        private void button1_Click(object sender, EventArgs e)
        {
            double amount = double.Parse(this.textBox1.Text);
            String refernceId = this.textBox2.Text;
            String mobileNumber = this.textBox3.Text;
            Customer cust = new Customer();
            cust.setMobileNumber(mobileNumber);
            OptionalParams op = new OptionalParams();
            op.setCustomer(cust);
            
           // ref.se
          //  op.setReference(ref);
            EzeResult result = EzeAPI.create().cashTransaction(amount,op);
            if (result.getStatus() == Status.SUCCESS)
            {
                Form3 f = new Form3(this,result.getResult().getTransactionDetails().getTxnId(),cust,parent);
                f.Show();

            }
            else
            {
                MessageBox.Show(result.getError().getMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
            }
        }
        //Home
        private void button2_Click(object sender, EventArgs e)
        {
            parent.Show();
            this.Close();
        }
        //logout
        private void button3_Click(object sender, EventArgs e)
        {
            EzeAPI.create().close();
            this.Close();
            login.Show();

        }
    }
}
