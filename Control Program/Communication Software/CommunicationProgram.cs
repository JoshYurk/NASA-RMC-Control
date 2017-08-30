using System;
using Core;
using Core.Enums;
using Renci.SshNet;

namespace Communication_Software
{
    internal class CommunicationProgram
    {
        private static void Main(string[] args)
        {
            Console.Title = "Communication Software";
            Console.WriteLine("Kent State Robotics NASA Communication Software");
            StartSshConnection();
            Console.ReadLine();
        }

        private static void StartSshConnection()
        {
            using (var sshClient = new SshClient(Constants.RobotIpAddress, Constants.RobotUsername, Constants.RobotPassword))
            {
                try
                {
                    Console.WriteLine("Connecting To: " + Constants.RobotIpAddress);
                    sshClient.Connect();
                    if (sshClient.IsConnected)
                    {
                        Console.WriteLine("Connected");
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
                catch (Exception e)
                {
                    Console.WriteLine("Error Connecting");
                }
            }
        }
    }
}
