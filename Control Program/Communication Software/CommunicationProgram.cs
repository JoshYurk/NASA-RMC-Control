using System;
using Core;
using Core.Enums;
using Renci.SshNet;

namespace Communication_Software
{
    internal class CommunicationProgram
    {
        const bool DEBUG = true;
        static RobotConnection conn;

        private static void Main(string[] args)
        {
            Console.WriteLine("Kent State Robotics NASA Communication Software");
            //StartSshConnection();
            Console.WriteLine("Enter m to input the robot's address and port manualy, c to use the defaults");
            string mode = Console.ReadLine();
            if (mode.ToLower() == "m") {
                ManualConnect();
            }
            else {
                DefaultConnection();
            }
            Console.WriteLine("Connection established");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static void DefaultConnection() {
            try {
                conn = new RobotConnection(Constants.RobotIpAddress, Constants.RobotPort);
                DebugStartChat();
            } catch (Exception e) {
                ManualConnect();
            }
            
        }

        private static void ManualConnect() {
            try {
                Console.WriteLine("Could not connect to robot, enter robot address manualy: ");
                string Ip = Console.ReadLine();
                Console.WriteLine("Enter Port: ");
                int Port;
                while (!int.TryParse(Console.ReadLine(), out Port)) {
                    Console.WriteLine("Enter Port: ");
                }
                conn = new RobotConnection(Ip, Port);
                DebugStartChat();
            }
            catch(Exception e) {
                ManualConnect();
            }
        }

        private static void DebugStartChat() {
            if (DEBUG) {
                conn.received += DebugReceive;
                conn.disconnect += DebugDisconnect;
                while (true) {
                    string input = Console.ReadLine();
                    if (input == "e") break;
                    conn.SendData(input);
                }
            }
        }

        private static void DebugDisconnect() {
            Console.WriteLine("Disconnected");
            Console.ReadLine();
            conn.Reconnect();
        }

        private static void DebugReceive(object source, ComData data) {
            Console.WriteLine(data.ToString());
        }

        private static void StartSshConnection()
        {
            using (var sshClient = new SshClient(Constants.RobotIpAddress, Constants.RobotUsername, Constants.RobotPassword))
            {
                sshClient.Connect();
                if (sshClient.IsConnected)
                {
                    if (Constants.RobotOperatingSystem == RobotOperatingSystems.Linux.OperatingSystem)
                    {
                        sshClient.RunCommand("ls");
                    }
                    else if (Constants.RobotOperatingSystem == RobotOperatingSystems.Windows.OperatingSystem)
                    {
                        sshClient.RunCommand("dir");
                    }
                }
                else
                {
                    Console.WriteLine("Failure To Connect");
                }
            }
        }
    }
}
