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
    public partial class TransactionFetchForm : Form
    {
        Form parent;
        public TransactionFetchForm(Form f)
        {
            InitializeComponent();
            this.parent = f;
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
            if ((this.textBox1.Text==null) && (this.textBox1.Text.Length==0))
            {
                MessageBox.Show("Invalid TransactionId", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
            }
            else
            {
                EzeResult result = EzeAPI.create().getTransaction(this.textBox1.Text);
                if (result.getStatus() == Status.SUCCESS)
                {
                    com.eze.api.TransactionDetails details = result.getResult().getTransactionDetails();
                    TransactionDetails td = new TransactionDetails(parent,details.getTxnId(),""+details.GetType(),""+details.getAmount(),"2015-10-23",result.getResult().getMerchant().getMerchantName(),result.getResult().getCustomer().getMobileNumber());
                    td.Show();
                }
                else
                {
                    MessageBox.Show(result.getError().getMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
                }

            }
        }
    }
}
