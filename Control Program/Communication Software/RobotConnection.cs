using System;
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
        const int MaxReconnectAtempts = 5;
        byte[] Buffer = new byte[1024];

        RobotConnection(string Ip, int Port) {
            Conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Conn.BeginConnect(Ip, Port, CallBeginConnect, null);
        }

        private void CallBeginConnect(IAsyncResult res) {
            try {
                Conn.EndConnect(res);
                Conn.BeginReceive(Buffer, 0, 1024, SocketFlags.None, EndReceive, null);
            } catch(SocketException e) {
                ReconnectAtempts++;
                if(ReconnectAtempts >= MaxReconnectAtempts){
                    Conn.BeginConnect(Ip, Port, CallBeginConnect, null);
                } else {
                    throw e;
                }
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
            ReconnectAtempts = 0;
            Conn.BeginConnect(Ip, Port, CallBeginConnect, null);
        }
        //TODO Receive
        private void EndReceive(IAsyncResult res){
            int Size = Conn.EndReceive(res);
            string dataString = Encoding.ASCII.GetString(Buffer, 0, Size);
            DataToSend data = new DataToSend(dataString);
            dataReceive(data);
            Conn.BeginReceive(Buffer, 0, 1024, SocketFlags.None, EndReceive, null);
        }

        public event DataToSend dataReceive;



    }
}
