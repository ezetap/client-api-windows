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
    public partial class ChequeForm : Form
    {
        Form parent;
        Form login;
        public ChequeForm(Form f,Form login)
        {
            InitializeComponent();
            this.Text = "EZETAP";
            parent = f;
            this.login = login;
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
            String mobileNumber = this.textBox5.Text;
            String emailId = this.textBox6.Text;
            Customer customer = new Customer();
            customer.setEmailId(emailId);
            customer.setMobileNumber(mobileNumber);
            String referenceId = this.textBox4.Text;
            //
            String chequeBankName = this.textBox2.Text;
            String chequeBankCode = this.textBox3.Text;
            String chequeDate = this.textBox4.Text;
            Cheque che = new Cheque();
            che.setBankName(chequeBankName);
            che.setBankCode(chequeBankCode);
            che.setChequeDate(chequeDate);
            che.setChequeNumber("100");
            Reference refe = new Reference();
            refe.setReference1(referenceId);
            OptionalParams op = new OptionalParams();
            op.setCustomer(customer);
            op.setReference(refe);
            double amount = double.Parse(this.textBox1.Text);
            EzeResult result = EzeAPI.create().chequeTransaction(amount,che,op);
            if (result.getStatus() == Status.SUCCESS)
            {
                if (result.getResult() != null)
                {
                    com.eze.api.TransactionDetails td = result.getResult().getTransactionDetails();
                    Form3 f3 = new Form3(parent, td.getTxnId(), op.getCustomer(), parent);
                    f3.Show();
                }
            }
            else
            {
                MessageBox.Show(result.getError().getMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EzeAPI.create().close();
            this.Close();
            login.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.parent.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
