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

namespace Control_Software
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
