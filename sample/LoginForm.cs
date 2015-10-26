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
    public partial class LoginForm : Form
    {
        String userName = null;
        public LoginForm()
        {
            InitializeComponent();
            this.Height = 450;
            this.Width = 450;
            this.Text = "EZETAP";
            statusMessageLabel = this.label1;

            // from anywhere ->
           
        }
        static Label statusMessageLabel;
        public  string StatusText { set { statusMessageLabel.Text = value; } }


        void messageListener(String message, EventArgs args)
        {
            this.StatusText = message;
            Console.WriteLine("MessageListener: " + message);
           // this.textBox2.Text = message;
            //this.textBox2.Show();this.label1.Refresh();
           
            //this.label1.
            //    this.Refresh();
            //this.WriteLine("MessageListener: " + message);

        }
        private void button1_Click(object sender, EventArgs e)
        {

            if ((userName == null) && (userName.Length==0))
                MessageBox.Show("Invalid UserName", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
            else
            {
                EzeAPI api = EzeAPI.create();
                EzeResult res = null;
               
                EzeConfig config = new EzeConfig(LoginMode.APPKEY, "3175bf13-9ea7-454a-bfc5-1644588cb6b8", "test", "INR", false, ServerType.DEMO);
                if (api != null)
                {
                    api.setMessageHandler(messageListener);
                    res = api.initialize(config);
                    if ((res != null) && (res.getStatus() == Status.SUCCESS))
                    {
                        this.Visible = false;
                        new MenuForm(this).ShowDialog();
                        
                    }
                    else
                    {
                        MessageBox.Show(res.getError().getMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
                        
                    }
                }
                
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            userName = textBox1.Text;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {
           // this.eventLog1.WriteEvent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
