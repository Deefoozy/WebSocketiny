using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocketTest.Responses
{
    class Message
    {
        static public byte[] GenerateMessage(string message)
        {
            byte[] resp = Encoding.UTF8.GetBytes("  " + message);
            resp[0] = 129;
            resp[1] = (byte)(resp.Length - 2);
            return resp;
        }
    }
}
