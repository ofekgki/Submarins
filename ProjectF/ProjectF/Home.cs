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
    public partial class Home : Form
    {
        Utilities u = new Utilities();
        private string messageR;

        private CommunicationHelper ch;
        int portNo = 500;
        //private string ipAddress = "192.168.1.23";
        TcpClient client = null;
        byte[] data;
        bool connectb = false;
        Thread connect;
         public Home()

        {
            InitializeComponent();
            connect = new Thread(new ThreadStart(IsConnected));
            this.ch = new CommunicationHelper(this);
        }

        
          private void button1_Click(object sender, EventArgs e)
        {//Create A First Connection To Server & Open A Login Form.
            ch.SendInfo("000");
            ch.client.GetStream().BeginRead(ch.data,
                                                0,
                                                System.Convert.ToInt32(ch.client.ReceiveBufferSize),
                                                ch.ReceiveInfo,
                                                null);
            this.Visible = false;
            Login l = new Login(this);
            ch.InitializeLoginForm(l);
            l.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void IsConnected()
        {
            //throw new NotImplementedException();
        }
        public void SendMessage(string message)
        {
            try
            {
                // send message to the server
                NetworkStream ns = client.GetStream();
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // send the text
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Asynchronously read data sent from the server in a seperate thread.
        /// Update the txtMessageHistory control using delegate.
        /// 
        /// Windows controls are not thread safed !
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                int bytesRead;

                // read the data from the server
                bytesRead = client.GetStream().EndRead(ar);

                if (bytesRead < 1)
                {
                    return;
                }
                else
                {
                    // invoke the delegate to display the recived data
                    object[] para = {
                                        System.Text.Encoding.ASCII.GetString(data,
                                                                             0,
                                                                             bytesRead)};
                    this.Invoke(new delUpdateMss(UpdateMss), para);
                }

                // continue reading
                client.GetStream().BeginRead(data,
                                         0,
                                         System.Convert.ToInt32(client.ReceiveBufferSize),
                                         ReceiveMessage,
                                         null);
                if (!connectb)
                {
                    //this.Invoke(new delIsConnected(IsConnected));
                    //Thread connect = new Thread(new ThreadStart(IsConnected));
                    connect.Start();
                    connectb = false;
                    //Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                // ignor the error... fired when the user loggs off
               
            }

        }
        public delegate void delUpdateMss(string str);
        public delegate void delIsConnected();

        public void UpdateMss(string str)
        {
            this.messageR = str;

    }

        void Disconnect()
        {
            try
            {
                // disconnect form the server
                client.GetStream().Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public CommunicationHelper getCh()
        {
            return this.ch;

        }
    }
}
