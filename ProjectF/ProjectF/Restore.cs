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
    public partial class Restore : Form
    {
        string str = "";
        string us = "" ;
        private string messageR;
        Utilities u = new Utilities();
        public CommunicationHelper ch;
        public Restore(string s , string user , Forgot f)
        {//open the form while saving the generated code and the username.
            
            InitializeComponent();
            this.ch = f.GetCh();
            str = s;
            us = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {//Check If The Code Is Correct And The Password Are Match.
         //Yes - Sending To Server The New Password And Update It. And Open A Login Form.
         //No - Pop Up Message With The Problem Occurred.
            if (checkBox1.Checked == true)
            {
                if ((str.Equals(textBox1.Text)) && (textBox2.Text.Equals(textBox3.Text)))//&& (u.IsValidPass(textBox2.Text)) 
                {
                    string mes = "New Pass:" + textBox2.Text + "?" + us;
                    ch.SendInfo(mes);
                    
                }
                else
                {
                    if (!str.Equals(textBox1.Text))
                    {
                        MessageBox.Show("Code Is Wrong, Try Again.");
                    }
                    //if (!u.IsValidPass(textBox2.Text))
                    //    MessageBox.Show("Password Unacceptable , Try Again.");
                    if (!textBox2.Text.Equals(textBox3.Text))
                    {
                        MessageBox.Show("Password Dosen't Match, Try Again.");
                    }
                }
            }
            else
                MessageBox.Show("Please Confirm You Are Not A Robot.");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {//Revel The Password.
            textBox2.PasswordChar = '\0';
            textBox3.PasswordChar = '\0';
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {//Hide The Password.
            textBox2.PasswordChar = '*';
            textBox3.PasswordChar = '*';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        { //Open A Captcha Form.
            Captcha c = new Captcha();
            c.Visible = true;
            checkBox1.Visible = false;
        }
        public CommunicationHelper GetCh()
        {
            return this.ch;
        }
        public void UpdateMss(string str)
        {
            if (str.StartsWith("Success Reset"))
            {
                Login l = new Login(this);
                ch.InitializeLoginForm(l);
                this.Visible = false;
                l.Show();
            }
            this.messageR = str;
        }
    }
}
