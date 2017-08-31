using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Control_Software.Getters;

namespace Control_Software
{
    public sealed partial class Form1 : Form
    {
        private static bool _isCommsStarted = false;
        private static Process _process;


        public Form1()
        {
            InitializeComponent();
            Text = @"NASA Control Program";
            WindowState = FormWindowState.Maximized;

            var button = new Button
            {
                Text = @"Start Communication Software"
            };
            button.Click += Button_Click;
            button.AutoSize = true;

            Controls.Add(button);
        }

        private static void Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (_isCommsStarted == false)
            {
                StartCommunicationSoftware();
                button.Text = @"Stop Communication Software";
            }
            else
            {
                EndCommunicationSoftware();
                button.Text = @"Start Communication Software";
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
            _isCommsStarted = false;
            _process.CloseMainWindow();
            _process.Close();
        }
    }
}
