using System;
using Core;
using Core.Enums;
using Renci.SshNet;

namespace Communication_Software
{
    /// <summary>
    /// This file should not be used in the release
    /// </summary>
    internal class CommunicationProgram
    {
        const bool DEBUG = true;
        static RobotConnection conn;

        private static void Main(string[] args)
        {
            Console.WriteLine("Kent State Robotics NASA Communication Software");
            //StartSshConnection();
            //DefaultConnection();
            if (DEBUG) {
                TestComData();
                Console.WriteLine("Enter m to input the robot's address and port manualy, c to use the defaults");
                string mode = Console.ReadLine();
                if (mode.ToLower() == "m") {
                    ManualConnect();
                }
                else {
                    DefaultConnection();
                }
                Console.WriteLine("Connection established");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static void DefaultConnection() {
            try {
                Console.WriteLine("Connecting to robot");
                conn = new RobotConnection(Constants.RobotIpAddress, Constants.RobotPort);
                if(DEBUG) DebugStartChat();
            } catch (Exception e) {
                Console.WriteLine("Failed to automaticly connect to robot");
                ManualConnect();
            }
            
        }

        private static void ManualConnect() {
            try {
                string Ip = Console.ReadLine();
                Console.WriteLine("Enter Port: ");
                int Port;
                while (!int.TryParse(Console.ReadLine(), out Port)) {
                    Console.WriteLine("Enter Port: ");
                }
                conn = new RobotConnection(Ip, Port);
                if (DEBUG) DebugStartChat();
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
                ManualConnect();
                Console.WriteLine("Failed to connect to robot");
            }
        }

        private static void DebugStartChat() {
            conn.received += DebugReceive;
            conn.disconnect += DebugDisconnect;
            while (true) {
                string input = Console.ReadLine();
                if (input == "e") break;
                conn.SendData(input);
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
            Console.WriteLine("SSHing to bot to launch server");
            using (var sshClient = new SshClient(Constants.RobotIpAddress, Constants.RobotUsername, Constants.RobotPassword))
            {
                sshClient.Connect();
                if (sshClient.IsConnected)
                {
                    if (Constants.RobotOperatingSystem == RobotOperatingSystems.Linux.OperatingSystem)
                    {
                        sshClient.RunCommand(Constants.RobotLauchFile);
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

        private static void TestComData() {
            ComData Data = new ComData();

            try {
                Data = new ComData();
                TestHelp("0", Data[0].ToString());
                Data = new ComData("0:7809134");
                TestHelp("7809134", Data[0].ToString());
                Data = new ComData("0:7809134|1:44|4:5677|2:456");
                TestHelp("44", Data[(dataId)1].ToString());
                Data = new ComData("0:7809134|hiu|aefu:ui:|olafe1:44|4:5677|2:456");
                TestHelp("5677", Data[(dataId)4].ToString());

                Data = new ComData();
                Data[0] = 45678;
                TestHelp("45678", Data[0].ToString());
                Data = new ComData();
                TestHelp("0", Data[0].ToString());

                Data = new ComData("0:7809134|1:44|4:5677|2:456");
                Console.WriteLine(Data.ToString());
                TestHelp("0:7809134|", Data.ToString(null, new Filter(new dataId[] { 0 })));
                Filter Filter = new Filter(new dataId[] { (dataId)1, (dataId)2 });
                Filter[2] = false;
                Filter[(dataId)3] = true;
                TestHelp("1:44|3:0|", Data.ToString(null, Filter));
                ComData PrevTest = new ComData("0:7809134|2:456");
                TestHelp("1:44|4:5677|", Data.ToString(PrevTest, null));
                TestHelp("1:44|", Data.ToString(PrevTest, new Filter(new dataId[] { (dataId)3, (dataId)1 })));
            } catch(Exception e) {
                Console.WriteLine("TEST CRASHED-------------------------------------------------------------------------------");
                Console.WriteLine(e);
            }
        }

        private static void TestHelp(string check, string value) {
            if(check != value) {
                Console.WriteLine("FAILED---------------------------------------------------------------------------------");
            }
            Console.WriteLine(check + " == " + value + " " + (check == value).ToString());
        }
    }
}
