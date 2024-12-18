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
    public partial class Lobby : Form
    {
        Utilities u = new Utilities();
        public CommunicationHelper ch;
        private string NickA;
        private string NickB;
        private string message;
        private string ask;
        private string messageR;
        #region בנאי
        public Lobby(Login l)
        {

            InitializeComponent();
            
            this.ch = l.GetCh();
            label4.Text = ch.get_name();
            NickA = ch.get_name();

        }
        public Lobby (Board b)
        {
            InitializeComponent();
            listBox1.Visible = false;
            label4.Text = ch.get_name();
            //timer1.Interval = 5000;
            this.ch = b.GetCh();
            NickA = ch.get_name();
        }
        public Lobby(GameH g)
        {

            InitializeComponent();
            listBox1.Visible = false;
            label4.Text = ch.get_name();
            //timer1.Interval = 5000;
            this.ch = g.GetCh();
            NickA = ch.get_name();

        }
        #endregion
        //private void button2_Click(object sender, EventArgs e)
        //{//Open A Game History Form.
        //    GameH g = new GameH(this);
        //    ch.InitializeGameHForm(g);
        //    g.Show();
        //    this.Visible = false;
        //}

        private void button5_Click(object sender, EventArgs e)
        {
            ch.SendInfo("Ready");
        }
        private delegate void delUpdateList(string str);
        /// <summary>
        /// Update The List.
        /// </summary>
        void UpdateList(string str)
        {
           
            listBox1.Items.Add(str);

        }
        void UpdateChat(string str)
        {
            this.Chatmsg.AppendText(str);
        }
        private delegate void delUpdateChat(string str);
        private void Ask(object sender, EventArgs e)
        {
            ask = listBox1.SelectedItem.ToString();
            NickB = listBox1.SelectedItem.ToString();
            message = "Ask" + ask + "From>"+ ch.get_name();
            ch.SendInfo(message);
           
            
        }   
        private void button7_Click(object sender, EventArgs e)
        {
          
            int a = label3.Text.IndexOf("Has");
            NickB = label3.Text.Remove(a - 1, 40);
            ch.SendInfo("Yes" + NickB);
            Board b = new Board(this, NickB);
            ch.InitializeBoardForm(b);
            b.Show();
            this.Visible = false;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            int a = label3.Text.IndexOf("Has");
            NickB = label3.Text.Remove(a - 1, 40);
            ch.SendInfo("No" + NickB);
            //MessageBox.Show("The Request Has Been Rejected");
            button7.Visible = false;
            button8.Visible = false;
            label3.Visible = false;
        }
        public CommunicationHelper GetCh()
        {
            return this.ch;
        }
        public void UpdateMss(string str)
        {
            if (str.StartsWith("LC"))
            {
                string m = str.Remove(0, 2);
                this.Invoke(new delUpdateChat(UpdateChat), m);
            }
            else
                if (str.StartsWith("List:"))
            {
                this.listBox1.Items.Clear();
                message = str;
                message = message.Remove(0, 6);
                string[] splitM = message.Split(',');
                foreach (string s in splitM)
                {
                    if( s != ch.get_name())
                    this.Invoke(new delUpdateList(UpdateList), s);
                }
            }
            else
                 if (str.StartsWith("Ask"))
            {
                message = str;
                message = message.Remove(0, 3);
                label3.Text = message;
                label3.Visible = true;
                button7.Visible = true;
                button8.Visible = true;
            }
            else
                if (str.StartsWith("No"))
            {
                label3.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                MessageBox.Show("The Request Has Been Rejected");
            }
            else
                if (str.StartsWith("Yes"))
            {
                ch.SendInfo("GameS: " + NickA + "," + NickB);
                Board b = new Board(this, NickB);
                ch.InitializeBoardForm(b);
                b.Show();
                this.Visible = false;
            }
            else
                if (str.StartsWith("NewL"))
                    ch.SendInfo("NewL"); 
                
            messageR = str;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                ch.SendInfo("LChat" + ch.get_name() + " > " + textBox1.Text);
                textBox1.Clear();
            }
            else
                MessageBox.Show("Can't Send A Empty Message");
        }
    }
}
