namespace Core.Enums
{
    public class RobotOperatingSystems
    {
        public static RobotOperatingSystems Windows = new RobotOperatingSystems("Windows");
        public static RobotOperatingSystems Linux = new RobotOperatingSystems("Linux");
        public static RobotOperatingSystems Mac = new RobotOperatingSystems("Mac");


        public string OperatingSystem { get; private set; }
        private RobotOperatingSystems(string operatingSystem)
        {
            OperatingSystem = operatingSystem;
        }
    }
}
