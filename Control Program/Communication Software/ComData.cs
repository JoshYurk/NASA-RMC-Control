﻿using System;

namespace Communication_Software {

    /// <summary>
    /// Add and modify the data in here to denote the integer data needed to be comminicated with the robot. The order of values must match on robot's program
    /// </summary>
    public enum dataId {
        BSMotor = 0,
        BPMotor = 1,
        SSMotor = 2,
        SPMotor = 3,
        ClawServo = 4,
        YawAct = 5,
        PitchAct = 6,
        ExtentionAct = 7,
        Error = 8
    }

    /// <summary>
    /// Stores and parses data to send and received from robot
    /// </summary>
    class ComData : EventArgs {

        

        /// <summary>
        /// Example filter, points that controler has authority over
        /// </summary>
        public static Filter ControlerAuth { get; private set; }  =  new Filter(new dataId[] { dataId.BPMotor, dataId.BSMotor, dataId.SPMotor, dataId.SSMotor, dataId.ClawServo, dataId.YawAct, dataId.PitchAct, dataId.ExtentionAct });

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
                if (!int.TryParse(input[i], out index) || !int.TryParse(input[i + 1], out value) || !(index < dataArray.Length)) continue;
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
        public int this[int id] {
            get {
                return dataArray[id];
            }
            set {
                dataArray[id] = value;
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

       

    }

    /// <summary>
    /// A bitset that can be used to filter data, default all false
    /// </summary>
    public class Filter {
        private bool[] bitSet = new bool[Enum.GetNames(typeof(dataId)).Length];

        public Filter() { }

        public bool this[dataId point] {
            get {
                return bitSet[(int)point];
            }
            set {
                bitSet[(int)point] = value;
            }
        }
        public bool this[int point] {
            get {
                return bitSet[point];
            }
            set {
                bitSet[point] = value;
            }
        }

        public Filter(dataId[] dataPoint) {
            foreach (dataId point in dataPoint) {
                bitSet[(int)point] = true;
            }
        }
        public Filter(int[] dataPoint) {
            foreach (int point in dataPoint) {
                bitSet[point] = true;
            }
        }

    }
}
