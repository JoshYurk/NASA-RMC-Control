using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Communication_Software;
using Control_Software.Getters;

namespace Control_Software
{
    public sealed partial class Form1 : Form
    {
        private static bool _isCommsStarted;
        private static Process _process;

        private static readonly Button CommunicationButton = new Button();

        public Form1()
        {
            InitializeComponent();
            Text = @"NASA Control Program";
            WindowState = FormWindowState.Maximized;

            CommunicationButton.Text = @"Start Communication Software";
            CommunicationButton.Click += Button_Click;
            CommunicationButton.AutoSize = true;

            Controls.Add(CommunicationButton);
        }

        private static void Button_Click(object sender, EventArgs e)
        {
            if (_isCommsStarted == false)
            {
                StartCommunicationSoftware();
                CommunicationButton.Text = @"Stop Communication Software";
            }
            else
            {
                EndCommunicationSoftware();
                CommunicationButton.Text = @"Start Communication Software";
            }
        }

        private static void StartCommunicationSoftware()
        {
            _isCommsStarted = true;
            var baseDirectory = DirectoryGetter.GetBaseDirectory(Directory.GetCurrentDirectory());
            var communicationUrl = baseDirectory + "\\Communication Software\\bin\\Debug\\Communication Software.exe";

            _process = Process.Start(communicationUrl);
        }

        private static void EndCommunicationSoftware()
        {
            CommandSender.ShutdownServerProcess();
            _isCommsStarted = false;
            _process.CloseMainWindow();
            _process.Close();
        }
    }
}
