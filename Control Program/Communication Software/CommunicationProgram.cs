using System;
using Core;
using Renci.SshNet;

namespace Communication_Software
{
    internal class CommunicationProgram
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Kent State Robotics NASA Communication Software");
            StartSshConnection();
            Console.ReadLine();
        }

        private static void StartSshConnection()
        {
            using (SshClient sshClient = new SshClient(Constants.RobotIpAddress, Constants.RobotUsername, Constants.RobotPassword))
            {
                sshClient.Connect();
                if (sshClient.IsConnected)
                {
                    sshClient.RunCommand("ls");
                }
                else
                {
                    Console.WriteLine("Failure To Connect");
                }
            }
        }
    }
}
