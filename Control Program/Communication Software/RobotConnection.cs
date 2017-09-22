﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Communication_Software {
    internal class RobotConnection {
        Socket Conn;
        int ReconnectAtempts = 0;

        RobotConnection(string Ip, int Port) {
            Conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Conn.BeginConnect(Ip, Port, CallBeginConnect, null);
        }

        private void CallBeginConnect(IAsyncResult res) {
            try {
                Conn.EndConnect(res);

            } catch(SocketException) {
                ReconnectAtempts++;
               // Conn.BeginConnect
            }
        }

        public void SendData(DataToSend Data) {
            byte[] Buffer = Encoding.ASCII.GetBytes(Data.ToString());
            try {
                Conn.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, EndSendCall, null);
            } catch(SocketException e) {

            }
        }

        private void EndSendCall(IAsyncResult res) {
            Conn.EndSend(res);

        }

        private void Reconnect() {
            Conn.Disconnect(true);
           // Conn.BeginConnect(Ip, Port, CallBeginConnect, null);
        }
        //TODO Receive
        //TODO Throw exception if cannot reconnect




    }
}
