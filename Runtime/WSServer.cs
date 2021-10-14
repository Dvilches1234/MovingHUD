using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;

namespace MovingHud
{
    public class WSServer : WebSocketBehavior
    {


        // Start is called before the first frame update
        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(e.Data);
        }

    }
}
