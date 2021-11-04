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
        public  int serverPort = 80;
        public IPAddress IP { get; set; }
        private Socket _httpServer;

        private Thread _thread;
        private string header = "";
        

        private void Update()
        {

        }

        public void StartServer()
        {
            IP = GetLocalIPAddress();
            WEB_DIR = Application.dataPath + WEB_DIR;
            //Debug.Log((WEB_DIR));
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
                StartListeningForConnection();
            }
            catch (Exception ex)
            {
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
                string file;
                if (dir == "/")
                    file = WEB_DIR + "/index.html";
                else
                    file = WEB_DIR + dir;

                FileInfo fileInfo = new FileInfo(file);
                //Debug.Log(file);
                FileInfo headerInfo = new FileInfo(WEB_DIR + "/Header");
                try
                {
                    header = getContentType(file);
                    FileStream fs = fileInfo.OpenRead();
                    BinaryReader reader = new BinaryReader(fs);
                    byte[] fileBytes = new byte[fs.Length];
                     /*
                    FileStream hs = headerInfo.OpenRead();
                    BinaryReader hreader = new BinaryReader(hs);
                    byte[] byteHeader = new byte[hs.Length];
                    hreader.Read(byteHeader, 0, byteHeader.Length);
                     */
                    byte[] byteHeader = Encoding.ASCII.GetBytes(header);
                     
                    
                    
                    reader.Read(fileBytes, 0, fileBytes.Length);
                    
                    client.SendTo(byteHeader, client.RemoteEndPoint);
                    Debug.Log(header);
                    Debug.Log("------------");
                    client.SendTo(fileBytes, client.RemoteEndPoint);
                }
                catch (Exception ex)
                {
                    header = getContentType(".html");
                    byte[] byteHeader = Encoding.ASCII.GetBytes(header);
                    byte[] message = Encoding.ASCII.GetBytes(ex.Message);
                    client.SendTo(byteHeader, client.RemoteEndPoint);
                    client.SendTo(message, client.RemoteEndPoint);
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

        public void StopServer()
        {
            if (_thread != null)
                _thread.Abort();
        }

        public string getContentType(string file)
        {
            
            string fileName = file.ToLower();
            Debug.Log(fileName);
            //string content = "HTTP/1.1 200 Everything is Fine\nServer: Hud_server\n";
            ///*
            string content = "";
            if (fileName.Contains(".html"))
            {
                content = "HTTP/1.1 200 Everything is Fine\nServer: Hud_server\nContent-Type: text/html\n \n ";
            }
            else if (fileName.Contains(".js"))
            {
                content = "HTTP/1.1 200 Everything is Fine\nServer: Hud_server\nContent-Type: text/javascript\n \n";
            }
            else if (fileName.Contains(".png"))
            {
                content = "HTTP/1.1 200 Everything is Fine\nServer: Hud_server\nContent-Type: image/* \n \n";
            }
            else if (fileName.Contains(".jpg") || fileName.Contains(".jpeg"))
            {
                content = "HTTP/1.1 200 Everything is Fine\nServer: Hud_server\nContent-Type: image/jpeg\n \n";
            }
            //*/
            //Debug.Log(content);
            return content;
        }
    }
}