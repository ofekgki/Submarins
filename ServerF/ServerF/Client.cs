using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerF
{
    public class Client
    {
        // Store list of all clients connecting to the server
        // the list is static so all memebers of the chat will be able to obtain list
        // of current connected client
        public static Hashtable AllClients = new Hashtable();
        static List<Player> playerlist = new List<Player>();
        static List<GameS> gamelist = new List<GameS>();
        static List<string> Connected = new List<string>();
        // information about the client
        private TcpClient client;
        private string clientIP;
        private string clientName;
        // used for sending and reciving data
        private byte[] data;
        Commands c = new Commands();
        Dal d = new Dal();
        string[] splitString;
        bool Check;
        bool Isready;
        string txtW;
        Player p;
        GameS g;
        string Reciver;
        string Sender;
        private string P2;
        private string P1;
        string serial;
        private static StreamWriter tw =  new StreamWriter(@"C:\Users\ofekg\Desktop\Ofek Project\Log.txt");


        /// <summary>
        /// When the client gets connected to the server the server will create an instance of the ChatClient and pass the TcpClient
        /// </summary>
        /// <param name="client"></param>

        public Client(TcpClient C)
        {
            client = C;

            // get the ip address of the client to register him with our client list
            clientIP = client.Client.RemoteEndPoint.ToString();
           
            // Add the new client to our clients collection
            AllClients.Add(clientIP, this);
            Console.WriteLine(clientIP + " Connected.");
            tw.WriteLine("New Client --" + "Ip:" + clientIP);
            tw.WriteLine("");
            tw.Flush();
             
           
            // Read data from the client async
            data = new byte[client.ReceiveBufferSize];

            // BeginRead will begin async read from the NetworkStream
            // This allows the server to remain responsive and continue accepting new connections from other clients
            // When reading complete control will be transfered to the ReviveMessage() function.
            client.GetStream().BeginRead(data,
                                          0,
                                          System.Convert.ToInt32(client.ReceiveBufferSize),
                                          ReceiveMessage,
                                          null);
        }

        /// <summary>
        /// allow the server to send message to the client.
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            try
            {
                System.Net.Sockets.NetworkStream ns;    

                // we use lock to present multiple threads from using the networkstream object
                // this is likely to occur when the server is connected to multiple clients all of 
                // them trying to access to the networkstram at the same time.
                lock (client.GetStream())
                {
                    ns = client.GetStream();
                }

                // Send data to the client
                byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes(message);
                ns.Write(bytesToSend, 0, bytesToSend.Length);
                tw.WriteLine("Server Send:  " + message);
                tw.WriteLine("");
                tw.Flush();
                ns.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ReceiveMessage(IAsyncResult ar)
        {
            int bytesRead;
            try
            {
                lock (client.GetStream())
                {
                    // call EndRead to handle the end of an async read.
                    bytesRead = client.GetStream().EndRead(ar);
                } 

                string messageReceived = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine(messageReceived);
                if (messageReceived.StartsWith("#"))
                {
                    splitString = messageReceived.Split('#');
                    d.AddUser(splitString[1], splitString[2], splitString[3], splitString[4], splitString[5], splitString[6], splitString[7]);
                    if (!d.IsUser(splitString[1]))
                    {
                        txtW = splitString[1] + "," + splitString[2] + "," + splitString[3] + "," + splitString[4] + "," + splitString[5] + "," + splitString[6] + "," + splitString[7] + "......";
                        Console.WriteLine("Success Registration - " + splitString[1]);
                        tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                        tw.WriteLine("Client" + clientIP + "  ,  Registration Information:  " + txtW);
                        tw.WriteLine("");
                        tw.Flush();
                        SendMessage("Success");
                    }
                    else
                    {
                        SendMessage("Taken");
                    }
                }
                if (messageReceived.StartsWith("/"))
                {
                    splitString = messageReceived.Split('/');
                    Check = d.CheckLogin(splitString[1], splitString[2]);
                    txtW = splitString[1] + ", " + splitString[2] + "......";
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client:  " + clientIP + "  ,  Login Information:  " + txtW );
                    tw.WriteLine("");
                    tw.Flush();
                    if (Check)
                    {
                        if (!c.Isconnected(splitString[1], Connected))
                        {
                            clientName = splitString[1];
                            Sender = splitString[1];
                            Connected.Add(splitString[1]);
                            p = new Player(clientName, false);
                            playerlist.Add(p);
                            SendMessage("Success");
                            tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                            tw.WriteLine("Login Successed .......");
                            tw.WriteLine("");
                            tw.Flush();
                            Console.WriteLine("Success Login - " + splitString[1]);
                        }
                        SendMessage("Already Connected");
                    }
                    else
                    {
                        SendMessage("Log in Failed");
                        tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                        tw.WriteLine("Login Fail ......");
                        tw.WriteLine("");
                        tw.Flush();
                        Console.WriteLine("Login Faild - " + splitString[1]);
                    }
                }
                if ( messageReceived.StartsWith("Email"))
                {
                    txtW = messageReceived;
                    txtW = txtW.Remove(0,5);
                    splitString = txtW.Split(',');
                    Check = d.CheckMail(splitString[0], splitString[1]);
                    if (Check)
                    {
                        SendMessage("Yes");
                        Console.WriteLine("Yes - Mail Confirmed " + splitString[1]);
                    }
                    else
                    {
                        SendMessage("No");
                        Console.WriteLine("No - Mail Is Wrong " + splitString[1]);
                    }
                }
                if (messageReceived.StartsWith("New Pass:"))
                {
                    messageReceived = messageReceived.Remove(0, 9);
                    Console.WriteLine(messageReceived);
                    splitString = messageReceived.Split('?');
                    d.UpdatePass(splitString[1], splitString[0]);
                    Console.WriteLine("Success Password Reset -" + splitString[1]);
                    txtW = splitString[0];
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client:  " + clientIP + "  ,  Password Change:  " + txtW + ",   Username:" + splitString[1] + "......");
                    tw.WriteLine("");
                    tw.Flush();
                    SendMessage("Success Reset" );
                }
                if (messageReceived.StartsWith("BChat"))
                {
                    txtW = messageReceived.Remove(0, 5);
                    //string s = txtW;
                    //int a = s.IndexOf('%');
                    //s = s.Remove(0, a + 1);
                    //if (s == serial)
                    //{
                        Spec(("BC" + txtW), P2);
                    // txtW = txtW.Remove(a);
                    //Spec(("BC" + txtW), P1);
                        SendMessage("BC" + txtW);
                    //}
                    Console.WriteLine("Board Chat -" + clientName);
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "  ,  Board Chat Writing:  " + txtW + "......");
                    tw.WriteLine("");
                    tw.Flush();
                }
                if (messageReceived.StartsWith("LChat"))
                {
                    txtW = messageReceived.Remove(0, 5);
                    Broadcast("LC" + txtW);
                    Console.WriteLine("Lobby Chat -" + clientName);
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "  ,  Lobby Chat Writing:  " + txtW + "......");
                    tw.WriteLine("");
                    tw.Flush();
                }



                if (messageReceived.StartsWith("GameS:"))
                {
                    txtW = messageReceived;
                    Console.WriteLine(txtW);
                    txtW = txtW.Remove(0, 7);
                    int a = txtW.IndexOf(',');
                    P1 = txtW.Remove(a);
                    P2 = txtW.Remove(0, a + 2);
                    string ip1 = "";
                    string ip2 = "";
                    Console.WriteLine(P1);
                    Console.WriteLine(P2);
                    foreach (DictionaryEntry c in AllClients )
                    {

                        if (((Client)(c.Value)).clientName == P1)
                            ip1 = ((Client)(c.Value)).clientIP;
                        if (((Client)(c.Value)).clientName == P2)
                            ip2 = ((Client)(c.Value)).clientIP;
                    }
                    //serial = c.SerialGenerate();
                    //Spec("%" + serial, P2);
                    //SendMessage("%" + serial);
                    playerlist.Remove(p);
                    Broadcast("NewL");
                    //g = new GameS(ip1, ip2, P1, P2 , serial);
                    //if (c.ifGameE(g, gamelist))
                    //{ gamelist.Add(g);
                    //    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    //    tw.WriteLine("Game  Created - Serial:   " + serial + "   Players:   " + Environment.NewLine + "1." + P1 +"...." + ip1 + Environment.NewLine + "2." + P2 + "...." + ip2 + "          ........");
                    //    tw.WriteLine("");
                    //    tw.Flush();
                    //}
                    //else
                    //    Console.WriteLine("Game Exist");
                }
                 if (messageReceived.StartsWith("EBoard:"))
                {
                    txtW = messageReceived.Remove(0, 7);
                    string s = txtW;
                    int a = s.IndexOf('%');
                    s = s.Remove(0, a + 1);
                    if (s == serial)
                    {
                        txtW = txtW.Remove(a);
                      
                    }
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "  ,  Enemy Board Update:  " + txtW + ".....");
                    tw.WriteLine("");
                    tw.Flush();
                }
                if (messageReceived.StartsWith("MBoard:"))
                {
                    txtW = messageReceived.Remove(0, 7);
                    string s = txtW;
                    int a = s.IndexOf('%');
                    s = s.Remove(0, a + 1);
                    if (s == serial)
                    {
                        txtW = txtW.Remove(a);

                    }
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "   ,  Self Board Update:   , Position: " + txtW + ".....");
                    tw.WriteLine("");
                    tw.Flush();
                }

                if (messageReceived.StartsWith("Ready"))
                {
                    Isready = true;
                    p.SetReady(Isready);

                    string s = "";
                    foreach (Player p in playerlist)
                    {
                        if (p.GetReady() == true)
                        {
                            s = s + p.GetName() + ", ";
                        }
                    }
                    Broadcast("List:  " + s);
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "  ,   " + "List Sended " + ".....");
                    tw.WriteLine("");
                    Console.WriteLine("List: " + s);

                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "Ready:   " + " , "+ Isready.ToString() +  ".....");
                    tw.WriteLine("");

                  }
                if (messageReceived.StartsWith("NewL"))
                {
                   
                    string s = "";
                    foreach (Player p in playerlist)
                    {
                        if (p.GetReady() == true)
                        {
                            s = s + p.GetName() + ", ";
                        }
                    }
                    Broadcast("List:  " + s);
                    tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                    tw.WriteLine("Client" + clientIP + "  ,   " + "List Sended " + ".....");
                    tw.WriteLine("");
                    Console.WriteLine("List: " + s);
                    
                }


                if (messageReceived.StartsWith("Ask"))
                {
                   
                    txtW = messageReceived;
                    int ind = txtW.IndexOf("From");
                    int na = clientName.Length;
                    string Reciver = txtW;
                    Reciver = Reciver.Remove(ind, 5 + na);
                    Reciver = Reciver.Remove(0, 4);
                    this.P1 = clientName;
                    this.P2 = Reciver;
                    Console.WriteLine(Reciver);
                    Spec("Ask" + clientName + " Has Requested To Play A Game With You. " , Reciver);
                                        
                }

                if (messageReceived.StartsWith("Yes"))
                {
                    messageReceived = messageReceived.Remove(0, 3);
                    Console.WriteLine(messageReceived);
                    Spec("Yes", messageReceived);
                    playerlist.Remove(p);
                    //SendMessage("Yes");
                }

                if (messageReceived.StartsWith("No"))
                {
                    messageReceived = messageReceived.Remove(0, 2);
                    Console.WriteLine(messageReceived);
                    Spec("No", messageReceived);
                    SendMessage("No");
                }

                lock (client.GetStream())
                {
                    // continue reading form the client
                    client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(client.ReceiveBufferSize), ReceiveMessage, null);
                }
            }
            catch (Exception ex)
            {
                AllClients.Remove(clientIP);
                playerlist.Remove(p);
                //Broadcast(clientIP + " has left the Program.");
                Console.WriteLine("Fail");
                tw.WriteLine(string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                tw.WriteLine("Client" + clientIP + "   ,  Program Crashed.....");
                tw.WriteLine("");
                tw.Flush();
            }
        }

        /// <summary>
        /// send message to all the clients that are stored in the allclients hashtable
        /// </summary>
        /// <param name="message"></param>
         public void Broadcast(string message)
        {
            //Console.WriteLine(message);
            foreach (DictionaryEntry c in AllClients)
            {
                ((Client)(c.Value)).SendMessage(message + Environment.NewLine);
            }
        }
        /// <summary>
        /// send message to Spesific client in the allclients hashtable
        /// </summary>
        /// <param name="message"></param>
        public void Spec(string message , string Reciver )
        {
            foreach (DictionaryEntry c in AllClients)
            {
                if (((Client)(c.Value)).clientName == Reciver)
                    ((Client)(c.Value)).SendMessage(message);
            }
        }
        /// <summary>
        /// send message to Two Connected clients that are stored in the allclients hashtable and playing together.
        /// </summary>
        /// <param name="message"></param>
        public void GameCast(string P1 , string P2 , string message)
        {
            foreach (DictionaryEntry c in AllClients)
            {
                if (((Client)(c.Value)).clientIP == P1)
                    ((Client)(c.Value)).SendMessage(message);
                if (((Client)(c.Value)).clientIP == P2)
                    ((Client)(c.Value)).SendMessage(message);
            }
        }
        public string SerialGenerate()
        {//Generating A Code With 4 Chars.
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";//Define The Chars Appers In Code
            var stringChars = new char[4];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }
    }
}
