using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectF
{
    public partial class Captcha : Form
    {
        Utilities u = new Utilities();
        int x;
        private string messageR;
        public Captcha()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            this.ControlBox = false;//Disable The Option To Close This Form
            this.Text = " Captcha";
        }

        private void Captcha_Load(object sender, EventArgs e)
        {//Randomize What Captch The User Will Need To Complete.
            Random rnd = new Random();
            x = rnd.Next(1, 3);
            
            if (x == 1)
            {
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox2.Visible = true;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {//Check If The User Complete The Captch Correctly
         //Yes - Message Pop Up "Correct", No - Message Pop Up "Try Again".
            string s = "";
            string str = "";
            
            if(pictureBox2.Visible == true)
            {
                s = "Website";
                str = "website";
            }

            if(pictureBox1.Visible == true)
            {
                s = "Foodie";
                str = "foodie";
            }

            if(textBox1.Text.Equals(s)||textBox1.Text.Equals(str))
            {
                MessageBox.Show("Correct");
                
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Try Again");
                
            }
        }
        public void UpdateMss(string str)
        {
            this.messageR = str;
        }

    }
}
