using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    public class LimitEntity
    {
        public int PartID;
        public int ProductionLimitID;
        public double UCL;
        public double LCL;
        public int PinID;
        public double AverageVoltage;
        public double StdDevVoltage;

        public LimitEntity()
        {
            PartID = 0;
            ProductionLimitID = 0;
            UCL = 0;
            LCL = 0;
            PinID = 0;
            AverageVoltage = 0;
            StdDevVoltage = 0;
        }

        public LimitEntity(int PartID, int LimitID, double UCL, double LCL, int PinID, double AverageVoltage, double StdDevVoltage)
        {
            this.PartID = PartID;
            this.ProductionLimitID = LimitID;
            this.UCL = UCL;
            this.LCL = LCL;
            this.PinID = PinID;
            this.AverageVoltage = AverageVoltage;
            this.StdDevVoltage = StdDevVoltage;
        }
    }
}
