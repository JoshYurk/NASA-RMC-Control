using System;
using System.Text;
using System.Net.Sockets;

namespace Communication_Software {
    class RobotConnection {
        public bool ready { private set; get; } = false;
        private Socket Conn;
        private int ReconnectAtempts = 0;
        private const int MAX_RECONNECT_ATEMPTS = 5;
        private byte[] Buffer = new byte[1024];
        private string Ip;
        private int Port;

        /// <summary>
        /// Create new connection to robot
        /// </summary>
        /// <param name="Ip">Ip address of robot</param>
        /// <param name="Port">TCP Port used to communicate with robot</param>
        public RobotConnection(string Ip, int Port) {
            this.Ip = Ip;
            this.Port = Port;
            Conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Conn.BeginConnect(Ip, Port, EndBeginConnect, null);
        }

        /// <summary>
        /// Delegate for the received event
        /// </summary>
        /// <param name="connection">The RobotConnection that received data from the robot</param>
        /// <param name="data">The data received from the robot</param>
        public delegate void receivedEventHandler(object connection, ComData data);
        /// <summary>
        /// Event that is fired when data is received from the robot
        /// </summary>
        public event receivedEventHandler received;

        public delegate void disconnectEventHandler();
        public event disconnectEventHandler disconnect;

        /// <summary>
        /// Sends data to robot, will do nothing if conn not yet established
        /// </summary>
        /// <param name="Data"></param>
        public void SendData(string Data) {
            if (ready) {
                byte[] Buffer = Encoding.ASCII.GetBytes(Data);
                try {
                    Conn.BeginSend(Buffer, 0, Data.Length, SocketFlags.None, EndSendCall, null);
                }
                catch (SocketException e) {
                    Reconnect();
                }
            }
        }

        public void Reconnect() {
            ready = false;
            Conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ReconnectAtempts = 0;
            Conn.BeginConnect(Ip, Port, EndBeginConnect, null);
        }

        #region privateMethods
        private void EndBeginConnect(IAsyncResult res) {
            try {
                Conn.EndConnect(res);
                Conn.BeginReceive(Buffer, 0, 1024, SocketFlags.None, EndReceive, null);
                ready = true;
            } catch(SocketException e) {
                ReconnectAtempts++;
                if(ReconnectAtempts >= MAX_RECONNECT_ATEMPTS){
                    Conn.BeginConnect(Ip, Port, EndBeginConnect, null);
                } else {
                    ready = false;
                    OnDisconnect();
                }
            }
        }

        private void EndSendCall(IAsyncResult res) {
            Conn.EndSend(res);
        }

        private void EndReceive(IAsyncResult res){
            try {
                int Size = Conn.EndReceive(res);
                string dataString = Encoding.ASCII.GetString(Buffer, 0, Size);
                ComData data = new ComData(dataString);
                OnReceived(data);
                Conn.BeginReceive(Buffer, 0, 1024, SocketFlags.None, EndReceive, null);
            } catch(SocketException e) {
                    Reconnect();
            }
        }

        protected virtual void OnReceived(ComData data) {
            received?.Invoke(this, data);
        }

        protected virtual void OnDisconnect() {
            disconnect?.Invoke();
        }
        #endregion

    }
}
