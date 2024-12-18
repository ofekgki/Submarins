using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectF
{
    public class CommunicationHelper
    {
        int portNo = 500;
        private string ipAddress = "127.0.0.1";
        private System.Net.IPAddress Add = System.Net.IPAddress.Parse("192.168.1.23");
        public TcpClient client = null;
        public byte[] data;
        public bool connectb = false;
        //Thread connect;
        Home localclient;
        Board brd;
        Captcha cap;
        Forgot fgt;
        GameH gh;
        Lobby lb;
        Login lg;
        Registration rg;
        Restore rst;
        public string name;


        public CommunicationHelper(Home c)
        {
            localclient = c;
            client = new TcpClient();
            client.Connect(ipAddress, portNo);

            data = new byte[client.ReceiveBufferSize];
        }

        public void InitializeBoardForm(Board brd)
        {
            this.brd = brd;
        }

        public void InitializeCaptchaForm(Captcha cap)
        {
            this.cap = cap;
        }

        public void InitializeForgotForm(Forgot fgt)
        {
            this.fgt = fgt;
        }

        public void InitializeGameHForm(GameH gh)
        {
            this.gh = gh;
        }

        public void InitializeLobbyForm(Lobby lb)
        {
            this.lb = lb;
        }

        public void InitializeRegistrationForm(Registration rg)
        {
            this.rg = rg;
        }

        public void InitializeRestoreForm(Restore rst)
        {
            this.rst = rst;
        }

        public void InitializeLoginForm(Login lg)
        {
            this.lg = lg;
        }

        public void InitializeHomeForm(Home localclient)
        {
            this.localclient = localclient;
        }

        public void ReceiveInfo(IAsyncResult ar)
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
                    object[] para = { System.Text.Encoding.ASCII.GetString(data, 0, bytesRead) };
                    if ((localclient != null) && (localclient.Visible))
                        localclient.Invoke(new delHomeUpdateMss(localclient.UpdateMss), para);
                    if ((lg != null) && (lg.Visible))
                        lg.Invoke(new delLoginUpdateMss(lg.UpdateMss), para);
                    if ((lb != null) && (lb.Visible))
                        lb.Invoke(new delLobbyUpdateMss(lb.UpdateMss), para);
                    if ((brd != null) && (brd.Visible))
                        brd.Invoke(new delBoardUpdateMss(brd.UpdateMss), para);
                    if ((gh != null) && (gh.Visible))
                        gh.Invoke(new delGameHUpdateMss(gh.UpdateMss), para);
                    if ((cap != null) && (cap.Visible))
                        cap.Invoke(new delCaptchaUpdateMss(cap.UpdateMss), para);
                    if ((fgt != null) && (fgt.Visible))
                        fgt.Invoke(new delForgotUpdateMss(fgt.UpdateMss), para);
                    if ((rg != null) && (rg.Visible))
                        rg.Invoke(new delRegistrationUpdateMss(rg.UpdateMss), para);
                    if ((rst != null) && (rst.Visible))
                        rst.Invoke(new delRestoreUpdateMss(rst.UpdateMss), para);

                }

                // continue reading
                client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(client.ReceiveBufferSize), ReceiveInfo, null);

                //if (!connectb)
                //{
                //    connect.Start();
                //    connectb = false;
                //}

            }
            catch (Exception ex)
            {
                // ignor the error... fired when the user loggs off
            }
        }

        public void SendInfo(string info)
        {
            try
            {
                // send message to the server
                NetworkStream ns = client.GetStream();
                byte[] data = System.Text.Encoding.ASCII.GetBytes(info);

                // send the text
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public string get_name()
        {
            return name;
        }

        public void set_name(string s)
        {
            name = s;
        }

        public TcpClient get_client()
        {
            return client;
        }

        public byte[] get_data()
        {
            return data;
        }

        public delegate void delHomeUpdateMss(string str);
        public delegate void delBoardUpdateMss(string str);
        public delegate void delCaptchaUpdateMss(string str);
        public delegate void delForgotUpdateMss(string str);
        public delegate void delGameHUpdateMss(string str);
        public delegate void delLobbyUpdateMss(string str);
        public delegate void delLoginUpdateMss(string str);
        public delegate void delRegistrationUpdateMss(string str);
        public delegate void delRestoreUpdateMss(string str);
    }
}

