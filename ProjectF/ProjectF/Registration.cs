using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectF
{
    public partial class Registration : Form
    {
        Utilities u = new Utilities();
        public CommunicationHelper ch;
        private string messageR;

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        public Registration(Login l)
        {
            InitializeComponent();
            this.ch = l.GetCh();
        }

        private string Check()
        {//Check Input.

            string s = "";
            if (checkBox1.Checked == false)
                s += "Please Confirm You Are Not A Robot. ";
         //   if (!u.IsValidMail(Mail.Text))
            //    s += "Mail Unacceptable , Try Again. ";
            if (u.IsValidPass(textBox1.Text))
               s += "Password Unacceptable , Try Again. /n Password Cannot Contain Sign As (# , $ , / , %) And It's Length Must Be At Least 6 Characters ";
            return s;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {//Show The Password.
            textBox1.PasswordChar ='\0';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//Create A Captcha Form.
            
            if (checkBox1.Checked == true)
            {
                Captcha c = new Captcha();
                c.Visible = true;
                checkBox1.Visible = false;
            }
            
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {//Hide The Password.
            textBox1.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {//If Check Input Fine. Sending Registration Information To Server.
         //If Server Sent " Registerd Successfully " Open A Login Form.
         //Else Show The Problem In The Input From The Check Function.
            if (Check() == "")
            {
                string str = "#" + textBox3.Text + "#" + textBox1.Text + "#" + Mail.Text + "#" + textBox5.Text + "#" + textBox6.Text + "#" + comboBox1.Text + "#" + textBox4.Text + "#";
                ch.SendInfo(str);
                Login l = new Login(this);
                ch.InitializeLoginForm(l);
                l.Show();
                Visible = false;

               
            }
            else
            {
                MessageBox.Show(Check());

            }
        }

        public void UpdateMss(string str)
        {
            if (str == "Success")
            {
                MessageBox.Show("Registration Successed ");
                Login l = new Login(this);
                ch.InitializeLoginForm(l);
                l.Show();
                this.Visible = false;
            }
            if(str == "Taken")
            {
                MessageBox.Show("Username Already Taken. " + Environment.NewLine + "try Again With Different Username. ");
            }
            else
                this.messageR = str;
        }

        public CommunicationHelper GetCh()
        {
            return this.ch;
        }

       
    }
}

