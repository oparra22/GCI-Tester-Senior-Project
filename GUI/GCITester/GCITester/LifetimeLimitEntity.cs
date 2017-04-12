using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    public class LifetimeLimitEntity
    {
        public int LifetimeLimitID;
        public double UpperRange;
        public double LowerRange;

        public LifetimeLimitEntity()
        {
            LifetimeLimitID = 0;
            UpperRange = 0;
            LowerRange = 0;
        }

        public LifetimeLimitEntity(int LifetimeLimitID, double UpperRange, double LowerRange)
        {
            this.LifetimeLimitID = LifetimeLimitID;
            this.UpperRange = UpperRange;
            this.LowerRange = LowerRange;
        }
    }
}
