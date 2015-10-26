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
    public partial class Form3 : Form
    {
        Form parent = null;
        Customer customer = null;
        Form superParent = null;
        public Form3(Form f, String id,Customer info,Form superParent)
        {
            
            InitializeComponent();
                        this.label2.Text = id;
            this.label2.Show();
            customer = info;
            this.superParent = superParent;
            this.Text = "EZETAP";
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
           /* if ((info==null) || ((info!=null) && ((info.getMobileNumber()==null) || (info.getEmailId() == null)))){
                this.button2.Hide();
            }*/


        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            EzeAPI.create().close();
            this.parent.Close();
            this.Close();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //Done
        private void button1_Click(object sender, EventArgs e)
        {
                      
            this.Close();
            this.superParent.Show();

        }
        //Send Receipt
        private void button2_Click(object sender, EventArgs e)
        {
           String tid =  this.label2.Text;
            if (customer != null)
            {
                EzeResult res = null;
                res = EzeAPI.create().sendReceipt(tid, customer.getMobileNumber(), "tk.bhargav@gmail.com");
                Console.WriteLine("send receipt status: " + res.getStatus());
                if (res.getStatus() == Status.SUCCESS)
                {
                    this.Close();
                    this.superParent.Show();
                }
                    
            }

        }
    }
}
