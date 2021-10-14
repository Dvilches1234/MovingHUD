using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using WebSocketSharp;
using WebSocketSharp.Server;
using QRCoder;
using QRCoder.Unity;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace MovingHud
{
    public class WSServerController : MonoBehaviour
    {
        public string port = "8088";
        public string path = "HUD";
        public GameObject qrHolder;
        public HttpServerController httpServer;

        private Image _qrSprite;
        private WebSocketServer _wsServer;
        private string _url;
        private string _ip;

        public void Start()
        {
            _qrSprite = qrHolder.GetComponent<Image>();
        }

        public void StartServer()
        {
            _ip = HttpServerController.GetLocalIPAddress().ToString();
            _url = "ws://" + _ip + ":" + port;
            _wsServer = new WebSocketServer(_url);
            _wsServer.AddWebSocketService<WSServer>("/" + path);
            _wsServer.Start();

            ChangeSprite();
        }

        public bool isOpen()
        {
            return _wsServer.IsListening;
        }

        public string GetURL()
        {
            return _url + "/" + path;
        }

        public string getHttpURL()
        {
            return "http://" + _ip + ":" + httpServer.GetHTTPPort();
        }

        private Texture2D GenerateBarcode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(getHttpURL(), QRCodeGenerator.ECCLevel.Q);
            UnityQRCode qrCode = new UnityQRCode(qrCodeData);
            Texture2D qrCodeTexture = qrCode.GetGraphic(32);
            return qrCodeTexture;
        }

        public void ChangeSprite()
        {
            if (!qrHolder.activeSelf)
                qrHolder.SetActive(true);

            Texture2D texture = GenerateBarcode();
            Rect rectangle = new Rect(0, 0, texture.width, texture.height);
            _qrSprite.sprite = Sprite.Create(texture, rectangle, new Vector2(0.5f, 0.5f), 128);
        }



    }
}