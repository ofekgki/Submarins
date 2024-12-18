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
    public partial class GameH : Form
    {
        public CommunicationHelper ch;
        private string messageR;
        public GameH(Lobby l)
        {
            InitializeComponent();
            this.ch = l.GetCh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lobby l = new Lobby(this);
            l.Show();
            this.Visible = false;
        }
        public void UpdateMss(string str)
        {
            this.messageR = str;
        }
        public CommunicationHelper GetCh()
        {
            return this.ch;
        }
    }
}
