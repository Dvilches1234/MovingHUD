using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingHud
{
    public class ServersController : MonoBehaviour
    {
        public static bool ServersOn = false;
        public string HUDTag = "HUD";

        private HttpServerController _httpServer;
        private WSServerController _wsServer;
        private WSClientController _wsClient;
        private List<GameObject> _hudElements;
        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Start()
        {
            _httpServer = GetComponent<HttpServerController>();
            _wsServer = GetComponent<WSServerController>();
            _wsClient = GetComponent<WSClientController>();
            _hudElements = new List<GameObject>();
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
            HideHud();
        }

        public void StartServers()
        {
            ServersOn = true;
            _wsServer.StartServer();
            _httpServer.StartServer();
            _wsClient.StartClient();
        }

        public void StopServers()
        {
            _wsClient.StopClient();
            _httpServer.StopServer();
            _wsServer.StopServer();
            ServersOn = false;
        }
        // called first

        public void HideHud()
        {
            if (ServersOn)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(HUDTag);
                foreach (GameObject gameObject in gameObjects)
                {
                    _hudElements.Add(gameObject);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject gameObject in _hudElements)
                {
                    if (gameObject != null)
                    {
                        gameObject.SetActive(true);
                    }
                    
                }
            }
        }

        public void ShowHud()
        {
            
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(HUDTag);
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(true);
            }
        }

    }
}