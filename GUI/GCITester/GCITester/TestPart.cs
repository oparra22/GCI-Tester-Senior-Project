using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    public class TestPartResult
    {
        public int PinID;
        public List<Double> VoltageReadings;

        public TestPartResult()
        {
            PinID = 0;
            VoltageReadings = new List<double>();
        }

        public TestPartResult(int PinID)
        {
            this.PinID = PinID;
            VoltageReadings = new List<double>();
        }

        public TestPartResult(int PinID, double Voltage)
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
                return Math.Round(Sum / Total,5);
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

    class TestPart
    {
        public int TotalIterations;
        public List<int> PinsToTest;
        public Dictionary<int, Dictionary<int,TestPartResult>> TestResults;
        public int CurrentIteration;
        public int CurrentPinNumber;

        public TestPart()
        {
            TotalIterations = 0;
            PinsToTest = new List<int>();
            TestResults = new Dictionary<int, Dictionary<int, TestPartResult>>();
            CurrentIteration = 0;
            CurrentPinNumber = 0;
        }

        public TestPart(int TotalIterations)
        {
            this.TotalIterations = TotalIterations;
            PinsToTest = new List<int>();
            TestResults = new Dictionary<int, Dictionary<int, TestPartResult>>();
            CurrentIteration = 0;
            CurrentPinNumber = 0;
        }

        public void ResetTest()
        {
            CurrentIteration = 0;
            CurrentPinNumber = 0;
            TestResults = new Dictionary<int, Dictionary<int, TestPartResult>>();
        }

        public int GetNextPin()
        {
            if (CurrentIteration < TotalIterations)
            {
                int Result = PinsToTest[CurrentPinNumber];
                if (CurrentPinNumber < PinsToTest.Count-1)
                {
                    CurrentPinNumber++;
                }
                else
                {
                    CurrentPinNumber = 0;
                    CurrentIteration++;
                }
                return Result;
            }
            /*else
            {
                CurrentIteration = 0;
                CurrentPinNumber = 0;
            }*/
            return 0;
        }

        public void AddResult(int SlotID, int Pin, double Voltage)
        {
            if (TestResults.ContainsKey(SlotID) == true)
            {
                if (TestResults[SlotID].ContainsKey(Pin) == false)
                {
                    TestResults[SlotID].Add(Pin, new TestPartResult(Pin, Voltage));
                }
                else
                {
                    TestResults[SlotID][Pin].VoltageReadings.Add(Voltage);
                }
            }
            else
            {
                TestResults.Add(SlotID, new Dictionary<int, TestPartResult>());
                TestResults[SlotID].Add(Pin, new TestPartResult(Pin, Voltage));
            }
        }
    }
}
