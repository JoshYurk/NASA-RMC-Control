using System;

namespace Communication_Software {
    /// <summary>
    /// Stores and parses data to send and received from robot
    /// </summary>
    class DataToSend {

        /// <summary>
        /// Add and modify the data in here to denote the integer data needed to be comminicated with the robot. The order of values must match on robot's program
        /// </summary>
        public enum dataId {
            Moter1,
            Moter2,
            Moter3,
            Moter4,
            Error1,
            Sensor1
        }

        public const dataId[] ControlAuthority = {Moter1, Moter2, Moter3, Moter4};
        public const dataId[] Errors = {Error1};
        public const dataId[] RobotAuthority = {Sensor1};

        int[] dataArray = new int[Enum.GetNames(typeof(dataId)).Length];

        public DataToSend() {}
        /// <summary>
        /// Take string received from robot and make a DataToSend object out of it
        /// </summary>
        /// <param name="data">Data received from robot</param>
        public DataToSend(string data) {
            string[] input = data.Split(new char[]{ '|',':'});
            for(int i = 0; i < input.Length; i += 2) {
                int index, value= 0;
                if (int.TryParse(input[i], out index) && int.TryParse(input[i + 1], out value) && index < dataArray.Length) break;
                dataArray[index] = value;
            }
        }

        public int GetData(dataId idenifier) {
            return dataArray[(int)idenifier];
        }

        public void SetData(dataId idenifier, int data) {
            dataArray[(int)idenifier] = data;
        }

        /// <summary>
        /// Turns object into a string to send to robot
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string output = "";
            foreach(dataId id in ControlAuthority){
                output += (int)id + ':' + dataArray[id] + '|';
            }
            foreach(dataId id in Errors){
                output += (int)id + ':' + dataArray[id] + '|';
            }
            return output;
        }
        /// <summary>
        /// Minimize network infomation by removing data that didn't change
        /// </summary>
        /// <param name="current">Last DataToSend sent to robot</param>
        /// <returns></returns>
        public string ToString(DataToSend current) {
            string output = "";
            foreach(dataId id in ControlAuthority){
                if (current.dataArray[id] != dataArray[id]) {
                    output += (int)id + ':' + dataArray[id] + '|';
                }
            }
            foreach(dataId id in Errors){
                if (current.dataArray[i] != dataArray[i]) {
                    output += (int)id + ':' + dataArray[id] + '|';
                }
            }
            return output;
        }

    }
}
