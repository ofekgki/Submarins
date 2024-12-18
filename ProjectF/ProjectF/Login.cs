using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectF
{


    public partial class Login : Form
    {
        Utilities u = new Utilities();
        public CommunicationHelper ch;
        private string messageR;

        public Login(Home h)
        {

            InitializeComponent();
            this.ch = h.getCh();
            
        }
        public Login(Registration r)
        {

            InitializeComponent();
            this.ch = r.GetCh();

        }
        public Login(Restore rs)
        {

            InitializeComponent();
            this.ch = rs.GetCh();

        }

        public CommunicationHelper GetCh()
        {
            return this.ch;
        }

        private void button2_Click(object sender, EventArgs e)
        {//Open A Registration Form.
            Registration reg = new Registration(this);
            ch.InitializeRegistrationForm(reg);
            reg.Show();
            this.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {//Create A String With The Login Information And Send It To Server.
         //If The Server Sent "Log In Successed" , Define Client Name & Message Pop Up "Log In Successed".
         //Opens A Lobby Form.
         //If Log-In Failed , Message Pop Up "Log In Failed".
            string s = "/" + textBox1.Text + "/" + textBox2.Text + "/";
            ch.SendInfo(s);
            //Thread.Sleep(1500);
            //if (messageR == "Success")
            //{
            //    ch.set_name(textBox1.Text);
            //    MessageBox.Show("Log in Successed");
            //    Lobby l = new Lobby(this);
            //    ch.InitializeLobbyForm(l);
            //    l.Show();
            //    this.Visible = false;
            //}
            //else
            //    MessageBox.Show("Log in Failed");
        }
        
         private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//Open A Captcha Form.
            if (checkBox1.Checked == true)
            {
                Captcha c = new Captcha();
                c.Show();
                checkBox1.Visible = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {//Open A Forgot Password Form.
            Forgot f = new Forgot(this);
            ch.InitializeForgotForm(f);
            f.Show();
            Visible = false;
        }

        public void UpdateMss(string str)
        {
            if (str  == "Success")
            {
                ch.set_name(textBox1.Text);
                MessageBox.Show("Log in Successed");
                Lobby l = new Lobby(this);
                ch.InitializeLobbyForm(l);
                l.Show();
                this.Visible = false;
            }
            if (str == "Already Connected")
            {
                MessageBox.Show("Already Connected");
            }
            else
                MessageBox.Show("Log in Failed");
            this.messageR = str;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
