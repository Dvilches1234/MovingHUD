using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using UnityEngine;


namespace MovingHud
{

    public class HttpServerController : MonoBehaviour
    {
        public string WEB_DIR = "/root/web";
        private int serverPort = 80;
        public IPAddress IP { get; set; }
        private Socket _httpServer;

        private Thread _thread;

        

        private void Update()
        {

        }

        public void StartServer()
        {
            IP = GetLocalIPAddress();
            Console.WriteLine("");
            WEB_DIR = Application.dataPath + WEB_DIR;
            Debug.Log((WEB_DIR));
            try
            {
                _httpServer = new Socket(SocketType.Stream, ProtocolType.Tcp);
                _thread = new Thread(new ThreadStart(this.ConnectionThreadMethod));
                _thread.Start();

            }
            catch (Exception ex)
            {
                Debug.Log("Error While starting server");
                Debug.Log(ex.Message);
            }


            if (isOpen())
                Debug.Log("server started in " + IP.ToString() + ":" + serverPort.ToString());
        }

        private void ConnectionThreadMethod()
        {

            try
            {
                IPEndPoint endpoint = new IPEndPoint(IP, serverPort);
                _httpServer.Bind(endpoint);
                _httpServer.Listen(10);
                Debug.Log(endpoint.Address);
                Debug.Log(endpoint.Port);
                StartListeningForConnection();
            }
            catch (Exception ex)
            {
                Debug.Log("Coudnt start");
                Debug.Log(ex.Message);
            }
        }

        private void StartListeningForConnection()
        {
            while (true)
            {
                string data = "";
                string[] dataArray;
                byte[] bytes = new byte[2048];
                Socket client = _httpServer.Accept();
                string dir;
                while (true)
                {
                    int numBytes = client.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, numBytes);
                    dataArray = data.Split(' ');
                    dir = dataArray[1];
                    if (data.IndexOf("\r\n") > -1)
                        break;
                }

                Debug.Log("---" + dir);
                string file;
                if (dir == "/")
                    file = WEB_DIR + "/index.html";
                else
                    file = WEB_DIR + dir;

                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Exists)
                    Debug.Log("Pagw  found");
                else
                {
                    Debug.Log("Pag not found");
                }

                try
                {
                    FileStream fs = fileInfo.OpenRead();
                    BinaryReader reader = new BinaryReader(fs);
                    byte[] fileBytes = new byte[fs.Length];

                    reader.Read(fileBytes, 0, fileBytes.Length);
                    client.SendTo(fileBytes, client.RemoteEndPoint);
                }
                catch (Exception ex)
                {
                    file = WEB_DIR + "/archivo2.html";
                    fileInfo = new FileInfo(file);
                    FileStream fs = fileInfo.OpenRead();
                    BinaryReader reader = new BinaryReader(fs);
                    byte[] fileBytes = new byte[fs.Length];

                    reader.Read(fileBytes, 0, fileBytes.Length);
                    client.SendTo(fileBytes, client.RemoteEndPoint);
                }

                client.Close();
            }
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public bool isOpen()
        {
            return _thread.IsAlive;
        }

        private void OnApplicationQuit()
        {
            if (_thread != null)
                _thread.Abort();
        }

        public int GetHTTPPort()
        {
            return serverPort;
        }
    }
}