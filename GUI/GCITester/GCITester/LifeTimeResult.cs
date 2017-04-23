using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    class LifetimeResult
    {
        public Byte PinID;
        public List<Double> VoltageReadings;

        public LifetimeResult()
        {
            PinID = 0;
            VoltageReadings = new List<double>();
        }

        public LifetimeResult(byte PinID)
        {
            this.PinID = PinID;
            VoltageReadings = new List<double>();
        }

        public LifetimeResult(byte PinID, double Voltage)
        {
            this.PinID = PinID;
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
