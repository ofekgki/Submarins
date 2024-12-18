using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerF
{
    class Program
    {
        const int portNo = 500;
        private const string ipAddress = "127.0.0.1";
        private const string add = "192.168.1.34";
        

        static void Main(string[] args)
        {
            System.Net.IPAddress localAdd = System.Net.IPAddress.Parse(ipAddress);
            //System.Net.IPAddress localAdd = System.Net.IPAddress.Parse(add);
            //TcpListener listener = new TcpListener(System.Net.IPAddress.Any, portNo);
            TcpListener listener = new TcpListener(localAdd, portNo);

            Console.WriteLine("Experts4D - Simple TCP Server");
            Console.WriteLine("Listening to ip {0} port: {1}", ipAddress, portNo);
            Console.WriteLine("Server is ready.");

            // Start listen to incoming connection requests
            listener.Start();

            // infinit loop.
            while (true)
            {
                // AcceptTcpClient - Blocking call
                // Execute will not continue until a connection is established

                // We create an instance of ChatClient so the server will be able to 
                // server multiple client at the same time.
                Client user = new Client(listener.AcceptTcpClient());

            }
        }
    }
}
