using System;

namespace Communication_Software {
    /// <summary>
    /// Stores and parses data to send and received from robot
    /// </summary>
    class ComData : EventArgs {

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

        /// <summary>
        /// Example filter, points that controler has authority over
        /// </summary>
        public static Filter ControlerAuth { get; private set; }  =  new Filter(new dataId[] { dataId.Moter1, dataId.Moter2, dataId.Moter3, dataId.Moter4 });

        private int[] dataArray = new int[Enum.GetNames(typeof(dataId)).Length];

        public ComData() { }

        /// <summary>
        /// Take string received from robot and make a ComData object out of it
        /// </summary>
        /// <param name="data">Data received from robot</param>
        public ComData(string data) {
            string[] input = data.Split(new char[] { '|', ':' });
            for (int i = 0; i < input.Length - 1; i += 2) {
                int index, value = 0;
                if (!int.TryParse(input[i], out index) || !int.TryParse(input[i + 1], out value) || !(index < dataArray.Length)) break;
                dataArray[index] = value;
            }
        }

        public int this[dataId id] {
            get {
                return dataArray[(int)id];
            } set {
                dataArray[(int)id] = value;
            }
        }

        /// <summary>
        /// Turns object into a string to send to robot
        /// </summary>
        /// <returns>String to send to robot</returns>
        public override string ToString() {
            return ToString(null, null);
        }

        /// <summary>
        /// Minimize network infomation by filtering out unnessecary info
        /// </summary>
        /// <param name="current">Last ComData sent to robot, will filter data that hasn't changed. Is Nullable</param>
        ///<param name="filter">Remove all data points except these. Is Nullable</param>
        /// <returns>A string that can be sent to the robot</returns>
        public string ToString(ComData current, Filter filter) {
            string output = "";
            for (int i = 0; i < dataArray.Length; i++) {
                if ((current == null || current.dataArray[i] != dataArray[i]) && (filter == null || filter[(dataId)i])) {
                    output += i.ToString() + ':' + dataArray[i].ToString() + '|';
                }
            }
            return output;
        }

        /// <summary>
        /// A bitset that can be used to filter data, default all false
        /// </summary>
        public class Filter {
            private bool[] bitSet = new bool[Enum.GetNames(typeof(dataId)).Length];

            public Filter() {}

            public bool this[dataId point] {
                get {
                    return bitSet[(int)point];
                }
                set {
                    bitSet[(int)point] = value;
                }
            }

            public Filter(dataId[] dataPoint) {
                foreach(dataId point in dataPoint) {
                    bitSet[(int)point] = true;
                }
            }

        }

    }
}
