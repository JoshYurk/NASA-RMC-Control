using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;

namespace Control_Software
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var button = new Button
            {
                Text = @"Start Communication Software"
            };
            button.Click += Button_Click;
            button.AutoSize = true;

            Controls.Add(button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            StartCommunicationSoftware();
        }

        protected void StartCommunicationSoftware()
        {
            var baseDirectory = GetBaseDirectory(Directory.GetCurrentDirectory());
            var communicationUrl = baseDirectory + "\\Communication Software\\bin\\Debug\\Communication Software.exe";

            System.Diagnostics.Process.Start(communicationUrl);
        }

        private static string GetBaseDirectory(string directory)
        {
            var baseDirectory = Directory.GetParent(directory).FullName;
            var strings = baseDirectory.Split('\\');

            if (!strings.Last().Contains(Constants.ProgramBaseFolderName))
            {
                return GetBaseDirectory(baseDirectory);
            }
            return baseDirectory;
        }
            
    }
}
