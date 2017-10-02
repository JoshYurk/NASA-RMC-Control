﻿using System;

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
            return ToString(null);
        }

        /// <summary>
        /// Minimize network infomation by removing data that didn't change
        /// </summary>
        /// <param name="current">Last ComData sent to robot</param>
        /// <returns></returns>
        public string ToString(ComData current) {
            string output = "";
            for (int i = 0; i < dataArray.Length; i++) {
                if (current == null || current.dataArray[i] != dataArray[i]) {
                    output += i.ToString() + ':' + dataArray[i].ToString() + '|';
                }
            }
            return output;
        }

    }
}
