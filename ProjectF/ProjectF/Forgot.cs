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
    public partial class Forgot : Form
    {
        Utilities u = new Utilities();
        string s = "";
        private string messageR;
        public CommunicationHelper ch;
        public Forgot(Login l)
        {
            InitializeComponent();
            this.ch = l.GetCh();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //send the informatiob to the server to check if email connected with username
            ch.SendInfo("Email" + textBox1.Text + "," + textBox2.Text);

        }
        public CommunicationHelper GetCh()
        {
            return this.ch;
        }
        public void UpdateMss(string str)
        {
            if (str == "Yes")
            {//Sending A Mail With A Generated Code And Open A Resotre Password Form.
             //The Recived Mail Address Is By The Inserted Address Of The Client.
             //Open A Restore Form With The Code Generated And The Username.
                s = u.CodeGenerate();
                u.sendMail(textBox1.Text, s, "Forgot Password");
                Restore r = new Restore(s, textBox2.Text, this);
                ch.InitializeRestoreForm(r);
                r.Show();
                this.Visible = false;
            }
            if (str == "No")
            {
                MessageBox.Show("The User Name And The Mail Are Not Connected , Try Again");
            }
            this.messageR = str;
        }
    }
}
