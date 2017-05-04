using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    class LearnResult
    {
        public int PinID;
        public int PinID2;
        public List<Double> VoltageReadings;

        public LearnResult()
        {
            PinID = 0;
            VoltageReadings = new List<double>();
        }

        public LearnResult(int PinID)
        {
            this.PinID = PinID;
            VoltageReadings = new List<double>();
        }

        public LearnResult(int PinID, double Voltage)
        {
            this.PinID = PinID;
            VoltageReadings = new List<double>();
            VoltageReadings.Add(Voltage);
        }
        public LearnResult(int PinID,int PinID2, double Voltage)
        {
            this.PinID = PinID;
            this.PinID2 = PinID2;
            VoltageReadings = new List<double>();
            VoltageReadings.Add(Voltage);
        }

        public double GetVoltageAverage()
        {
            if (VoltageReadings != null)
            {
                int Total = VoltageReadings.Count;
                double Sum = 0;
                foreach (double reading in VoltageReadings)
                {
                    Sum += reading;
                }
                return Math.Round(Sum / Total, 5);
            }
            return 0;
        }

        public double GetStandardDeviation()
        {
            double temp = 0;
            double Average = GetVoltageAverage();

            if (VoltageReadings != null)
            {
                int Total = VoltageReadings.Count;
                if (Total == 1)
                    return 0;

                foreach (double reading in VoltageReadings)
                {
                    temp += Math.Pow(reading - Average, 2);
                }
                temp /= Total - 1;
            }
            return Math.Round(Math.Sqrt(temp), 5);
        }
    }
}
