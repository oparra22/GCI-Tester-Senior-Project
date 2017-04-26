using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    public class LearnControl
    {
        public int TotalIterations;
        public List<Byte> PinsToTest;

        public int CurrentIteration;
        public int CurrentPinNumber;

        public LearnControl()
        {
            TotalIterations = 0;
            PinsToTest = new List<Byte>();
            CurrentIteration = 0;
            CurrentPinNumber = 0;
        }

        public LearnControl(int TotalIterations)
        {
            this.TotalIterations = TotalIterations;
            PinsToTest = new List<Byte>();
            CurrentIteration = 0;
            CurrentPinNumber = 0;
        }

        public void ResetTest()
        {
            CurrentIteration = 0;
            CurrentPinNumber = 0;
        }

        public Byte GetNextPin()
        {
            if (CurrentIteration < TotalIterations)
            {
                Byte Result = PinsToTest[CurrentPinNumber];
                if (CurrentPinNumber < PinsToTest.Count - 1)
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
            else
            {
                CurrentIteration = 0;
                CurrentPinNumber = 0;
            }
            return 0;
        }
    }
}
