using System;
using System.Net.Sockets;
using System.Text;

namespace ClientSide
{
    public class Client
    { 
        private TcpClient _client; 
        private NetworkStream _ns;
        public bool Connected { get; set; }
        public void Connect(string ip, int port) 
        { 
            try 
            { 
                ClientSide.Client client = new ClientSide.Client(); 
                _client = new TcpClient(ip, port);
                _ns = _client.GetStream();
                _ns.ReadTimeout = 10000;
                _ns.WriteTimeout = 10000;
                Connected = true;
            }
            catch (Exception e)
            {
                throw new Exception("couldn't connect with server");
            }
        }

        public void Disconnect()
        { 
            try
            { 
                _ns.Close();
                _client.Close();
            }
            catch (Exception e)
            {
                throw new Exception("couldn't disconnect with server");
            }

        }

        public string Read()
        {
            string retval;
            byte[] bytes = new byte[1024]; 
            int bytesRead; 
            try 
            { 
                bytesRead = _ns.Read(bytes, 0, bytes.Length);
            }
            catch (TimeoutException e) 
            {
                throw e;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("time"))
                {
                    throw new TimeoutException(e.Message);
                }
                throw e;
            }

            if (bytesRead == 0)
            {
                throw new Exception("couldn't read from server");
            }
            retval = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            return retval;
        }

        public void Write(string command)
        {
            byte[] bytes;
            bytes = Encoding.ASCII.GetBytes(command);
            try
            {
                _ns.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("time"))
                {
                    throw new TimeoutException(e.Message);
                } 
                throw e;
            }
        }
    }
}
