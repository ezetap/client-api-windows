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


    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* CardForm card = new CardForm(this);
            card.Show();
            this.Visible = false;*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserControl1 c = new UserControl1();
            c.Visible = true;
            c.Show();
           // CashForm cash = new CashForm();
            //cash.Show();
        }

       

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void Card_Click(object sender, EventArgs e)
        {
          /*  Label title = new Label();
            title.Text = "card";
            //sp.Panel1.Controls.Add(title);
            CardForm card = new CardForm(this);
            //card.Show();
            //this.Visible = false;

            this.splitContainer1.Panel2.Controls.Add(card);*/
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
