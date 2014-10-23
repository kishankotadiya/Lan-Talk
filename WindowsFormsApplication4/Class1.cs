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
    class Class1
    {
        

        public String ping(String ip)
        {
            String status;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            options.DontFragment = true;

            string data = "";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            try
            {
                PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                status = reply.Status.ToString();
            }
            catch (Exception)
            {

                status = "Host Unreachable";
            }
            return (status);


        }

        public void SendBroadcast(String ip,int port, string message)
        {
            UdpClient client = new UdpClient();
            byte[] packet = Encoding.ASCII.GetBytes(message);
            
            
            try
            {
                client.Send(packet, packet.Length,ip, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       
        
    }
}
