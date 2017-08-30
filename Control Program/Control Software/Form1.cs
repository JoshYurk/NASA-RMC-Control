using System;
using System.IO;
using System.Windows.Forms;
using Control_Software.Getters;

namespace Control_Software
{
    public sealed partial class Form1 : Form
    {
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
            StartCommunicationSoftware();
        }

        private static void StartCommunicationSoftware()
        {
            var baseDirectory = DirectoryGetter.GetBaseDirectory(Directory.GetCurrentDirectory());
            var communicationUrl = baseDirectory + "\\Communication Software\\bin\\Debug\\Communication Software.exe";

            System.Diagnostics.Process.Start(communicationUrl);
        }
    }
}
