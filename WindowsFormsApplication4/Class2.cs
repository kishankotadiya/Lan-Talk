using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security;
namespace KishanKotadiya
{
    class Class2
    {
        Int32 port;
        UdpClient udpClient;
       static Thread thread;
        String status;
        static int count = 0;
        String sourceIP;
        public static void main()
        {
            thread = new Thread(new ThreadStart(ReceiveMessage));
            thread.IsBackground = true;
            thread.Start();
        }

        private static void ReceiveMessage()
        {
            try
            {
                udpClient = new UdpClient(port);
            }
            catch (Exception)
            {
                MessageBox.Show("Port is already in use please restart the application and use any unsued port for communication.");

            }
            while (true)
            {
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, port);
                byte[] content = udpClient.Receive(ref remoteIPEndPoint);
                sourceIP = remoteIPEndPoint.Address.ToString();

                string message = Encoding.ASCII.GetString(content);
                Console.WriteLine(message);
                Console.WriteLine("Connetcted to " + sourceIP + " on port " + port + ".");

            }
        }


    }


}
