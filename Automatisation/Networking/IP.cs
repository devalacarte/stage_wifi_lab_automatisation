/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Automatisation.Networking
{   
    public static class IP
    {
        /// <summary>
        /// Checks if an IP is valid.
        /// </summary>
        /// <param name="ip">IP address, either IPv4 or IPv6</param>
        /// <returns>Returns true when IP is valid, false if invalid.</returns>
        public static bool checkValidIP(string ip)
        {
            bool isIP = false;
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        // we have IPv4
                        isIP = true;
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        // we have IPv6
                        isIP = true;
                        break;
                    default:
                        // umm... yeah... I'm going to need to take your red packet and...
                        isIP = false;
                        break;
                }
            }
            return isIP;
        }

        /// <summary>
        /// Pings an ip and gives and returns an appropriate message, much like pinging in cmd.
        /// </summary>
        /// <param name="ip">Valid IP in string format: "192.168.1.2".</param>
        /// <returns>Executed ping information. If successful: bytes, time and TTL.</returns>
        public static string PingIP(string ip)
        {
            string message = String.Empty;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted. 
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 30;

            //for (int i = 0; i < 4; i++)
            //{
            try
            {
                //send the ping 4 times to the host and record the returned data.
                //The Send() method expects 4 items:
                //1) The IPAddress we are pinging
                //2) The timeout value
                //3) A buffer (our byte array)
                //4) PingOptions
                PingReply pingReply = null;
                try
                {
                    pingReply = pingSender.Send(ip, timeout, buffer, options);
                }
                catch (Exception e)
                {

                    return e.Message;
                }

                //make sure we dont have a null reply
                if (!(pingReply == null))
                {
                    switch (pingReply.Status)
                    {
                        case IPStatus.Success:
                            message = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}", pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime, pingReply.Options.Ttl);
                            break;
                        case IPStatus.TimedOut:
                            message = "Connection has timed out...";
                            break;
                        default:
                            message = string.Format("Ping failed: {0}", pingReply.Status.ToString());
                            break;
                    }
                }
                else
                    message = "Connection failed for an unknown reason...";
            }
            catch (PingException ex)
            {
                message = string.Format("Connection Error: {0}", ex.Message);
            }
            catch (SocketException ex)
            {
                message = string.Format("Connection Error: {0}", ex.Message);
            }
            //} //end for loop
            return message;
        }
    }
}
