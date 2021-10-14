using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;


namespace MovingHud
{
    public class WSClientController : MonoBehaviour
    {
        private string _url;
        private WebSocket _ws;

        private WSServerController _wsServerController;
        private static  Dictionary<string, string> _data;
        private string _oldData = "";

        public void Start()
        {

            _wsServerController = GetComponent<WSServerController>();
            _data = new Dictionary<string, string>();
        }

        public void Update()
        {
            if (_ws != null)
                SendWSMessage();
        }

        public void StartClient()
        {
            _url = _wsServerController.GetURL();
            _ws = new WebSocket(_url);
            _ws.Connect();
        }


        public void SendWSMessage()
        {

            string dataJSON = JsonConvert.SerializeObject(_data);
            if (dataJSON != _oldData)
            {
                _ws.Send(dataJSON);
                _oldData = dataJSON;
            }
        }

        static public void AddValueToDictionary(string key, string value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }

        public bool IsOpen()
        {
            return _ws.IsAlive;
        }
    }
}