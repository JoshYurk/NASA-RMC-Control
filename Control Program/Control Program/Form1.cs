using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Renci.SshNet;

namespace Control_Program
{
    public partial class Form1 : Form
    {
        readonly PictureBox _robotImage = new PictureBox();
        readonly Button _connectButton = new Button();
        readonly Button _shutdownButton = new Button();
        readonly Label _connectionStatus = new Label();
        readonly Label _connectionInformation = new Label();
        readonly Label _motor1Current = new Label();
        readonly Label _motor2Current = new Label();
        readonly Label _motor3Current = new Label();
        readonly Label _motor4Current = new Label();
        readonly Label _motor1Label = new Label();
        readonly Label _motor2Label = new Label();
        readonly Label _motor3Label = new Label();
        readonly Label _motor4Label = new Label();
        readonly Label _leftSideSpeed = new Label();
        readonly Label _rightSideSpeed = new Label();

        protected string RobotIpAddress = "192.168.1.134";
        protected string RobotUserName = "pi";
        protected string RobotPassword = "raspberry";

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            InitializeUi();
        }

        public void InitializeUi()
        {
            var rectangle = Screen.FromControl(this).Bounds;
            SetUpRobotInfo(rectangle);
            SetUpConnectionInfo(rectangle);

            _shutdownButton.Left = _connectButton.Left;
            _shutdownButton.Top = rectangle.Bottom - 100;
            _shutdownButton.Text = @"Emergency Shutdown";
            _shutdownButton.AutoSize = true;
            _shutdownButton.TabStop = false;

            Controls.Add(_shutdownButton);
        }

        private void SetUpRobotInfo(Rectangle rectangle)
        {
            _robotImage.Left = 10;
            _robotImage.Image = Image.FromFile(@"C:\Users\jyurk\OneDrive\Pictures\sadpsycho.jpg");
            _robotImage.AutoSize = true;
            _robotImage.Top = 5;
            _robotImage.SendToBack();
            Controls.Add(_robotImage);

            _motor1Label.Text = @"Motor 1:";
            _motor2Label.Text = @"Motor 2:";
            _motor3Label.Text = @"Motor 3:";
            _motor4Label.Text = @"Motor 4:";
            _motor1Label.Top = _robotImage.Bottom + 5;
            _motor2Label.Top = _motor1Label.Bottom + 5;
            _motor3Label.Top = _motor2Label.Bottom + 5;
            _motor4Label.Top = _motor3Label.Bottom + 5;
            _motor1Label.Left = _robotImage.Left;
            _motor2Label.Left = _robotImage.Left;
            _motor3Label.Left = _robotImage.Left;
            _motor4Label.Left = _robotImage.Left;

            _motor1Current.Left = _motor1Label.Right;
            _motor2Current.Left = _motor2Label.Right;
            _motor3Current.Left = _motor3Label.Right;
            _motor4Current.Left = _motor4Label.Right;
            _motor1Current.Top = _motor1Label.Top;
            _motor2Current.Top = _motor2Label.Top;
            _motor3Current.Top = _motor3Label.Top;
            _motor4Current.Top = _motor4Label.Top;

            _motor1Current.Text = @"0 mAh";
            _motor2Current.Text = @"0 mAh";
            _motor3Current.Text = @"0 mAh";
            _motor4Current.Text = @"0 mAh";


            var motorLabels = new List<Label>
            {
                _motor1Label,
                _motor1Current,
                _motor2Label,
                _motor2Current,
                _motor3Label,
                _motor3Current,
                _motor4Label,
                _motor4Current
            };

            foreach (var motorLabel in motorLabels)
            {
                Controls.Add(motorLabel);
            }

            _leftSideSpeed.Text = @"Left Side Speed: 0";
            _leftSideSpeed.Top = (rectangle.Bottom + _motor4Label.Bottom) / 2;
            _leftSideSpeed.Left = _robotImage.Left;
            _leftSideSpeed.AutoSize = true;

            _rightSideSpeed.Text = @"Right Side Speed: 0";
            _rightSideSpeed.Left = _leftSideSpeed.Left;
            _rightSideSpeed.Top = _leftSideSpeed.Bottom + 5;
            _rightSideSpeed.AutoSize = true;

            Controls.Add(_leftSideSpeed);
            Controls.Add(_rightSideSpeed);
        }

        private void SetUpConnectionInfo(Rectangle rectangle)
        {
            _connectButton.Left = rectangle.Right - 150;
            _connectButton.Top = 50;
            _connectButton.Text = @"Connect To Robot";
            _connectButton.AutoSize = true;
            _connectButton.TabStop = false;
            _connectButton.Click += _connectButton_Click;
            _connectionStatus.Text = @"Not Connected To Robot";
            _connectionStatus.AutoSize = true;
            _connectionStatus.Left = _connectButton.Left;
            _connectionStatus.Top = _connectButton.Top - 20;

            _connectionInformation.Text = @"Connected To: ";
            _connectionInformation.Top = _connectButton.Bottom + 5;
            _connectionInformation.Left = _connectButton.Left;
            _connectionInformation.AutoSize = true;

            Controls.Add(_connectButton);
            Controls.Add(_connectionStatus);
            Controls.Add(_connectionInformation);
        }

        private void _connectButton_Click(object sender, EventArgs e)
        {
            if (_connectionStatus.Text == @"Not Connected To Robot")
            {
                _connectionStatus.Text = @"Connected To Robot";
                _connectButton.Text = @"Disconnect From Robot";
                var connectionInformationText = _connectionInformation.Text;
                _connectionInformation.Text = connectionInformationText + RobotIpAddress;
            }
            else
            {
                _connectionStatus.Text = @"Not Connected To Robot";
                _connectButton.Text = @"Connect To Robot";
                _connectionInformation.Text = @"Connected To: ";
            }
        }
    }
}
