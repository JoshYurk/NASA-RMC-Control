using Core;
using Renci.SshNet;

namespace Communication_Software
{
    public static class CommandSender
    {
        public static void ShutdownServerProcess()
        {
            using (var sshClient = new SshClient(Constants.RobotIpAddress, Constants.RobotUsername, Constants.RobotPassword))
            {
                sshClient.Connect();
                if (sshClient.IsConnected)
                {
                    var runCommand = sshClient.RunCommand("ps -C 'python server.py'");
                    var runCommandResult = runCommand.Result;
                    var strings = runCommandResult.Split(' ');

                    sshClient.RunCommand("kill -9 " + strings[15]);
                }
            }
        }
    }
}
