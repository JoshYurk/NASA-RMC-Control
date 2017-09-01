using System;
using System.IO;
using System.Runtime.InteropServices;
using Core;
using Renci.SshNet;

namespace Communication_Software
{
    internal class CommunicationProgram
    {
        private const int MfBycommand = 0x00000000;
        public const int ScClose = 0xF060;

        private static void Main(string[] args)
        {
            RemoveButtons();
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
                        var runCommand = sshClient.RunCommand("python server.py");

                        var streamReader = new StreamReader(runCommand.OutputStream);
                        Console.Write(streamReader.Read());

                        Console.WriteLine(runCommand.Error);
                        Console.WriteLine(runCommand.ExitStatus);
                        runCommand.Dispose();
                        streamReader.Close();
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

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void RemoveButtons()
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), ScClose, MfBycommand);
        }
    }
}
