using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingHud
{
    public class ServersController : MonoBehaviour
    {
        public static bool ServersOn = false;


        private HttpServerController _httpServer;
        private WSServerController _wsServer;
        private WSClientController _wsClient;

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Start()
        {
            _httpServer = GetComponent<HttpServerController>();
            _wsServer = GetComponent<WSServerController>();
            _wsClient = GetComponent<WSClientController>();
        }

        public void Update()
        {
            if (ServersOn)
            {
                if (!_wsServer.isOpen())
                    _wsServer.StartServer();
                if (!_httpServer.isOpen())
                    _httpServer.StartServer();
                if (!_wsClient.IsOpen())
                    _wsClient.StartClient();
            }

        }

        public void StartServers()
        {
            ServersOn = true;
            _wsServer.StartServer();
            _httpServer.StartServer();
            _wsClient.StartClient();

        }

    }
}