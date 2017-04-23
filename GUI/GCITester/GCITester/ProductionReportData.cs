using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace GCITester
{
    class ProductionReportPinData
    {
        public int PinID;
        public List<Double> VoltageReadings;

        public ProductionReportPinData()
        {
            PinID = 0;
            VoltageReadings = new List<double>();
        }

        public ProductionReportPinData(int PinID)
        {
            this.PinID = PinID;
            VoltageReadings = new List<double>();
        }

        public ProductionReportPinData(int PinID, double Voltage)
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


    class ProductionReportData
    {
        Dictionary<Byte, double> BaselineData;
        Dictionary<Byte, ProductionReportPinData> BatchData;
        List<int> AllProductionID;
        public int TotalDevices = 0;
        public int TotalPins = 0;
        public List<Byte> AllTestPins = new List<byte>();
        DataTable RawData;
        public String PartName;
        public String BatchName;


        public ProductionReportData(DataTable RawData, String PartName, String BatchName)
        {
            this.RawData = RawData;
            this.PartName = PartName;
            this.BatchName = BatchName;
            BaselineData = new Dictionary<byte, double>();
            BatchData = new Dictionary<byte, ProductionReportPinData>();
            AllProductionID = new List<int>();
            PopulateData();
        }

        public void PopulateData()
        {
            foreach (DataRow drRow in RawData.Rows)
            {
                int ProductionTestID = Convert.ToInt16(drRow["ProductionTestID"]);
                DateTime DateMeasured = Convert.ToDateTime(drRow["CreationDate"]);
                Byte PinNumber = Convert.ToByte(drRow["DUTPinNumber"]);
                Double MeasuredVoltage = Convert.ToDouble(drRow["AverageVoltage"]);
                Double BaselineVoltage = Convert.ToDouble(drRow["BaselineVoltage"]);
                int Pass = Convert.ToInt16(drRow["TestResult"]);
                if (AllProductionID.Contains(ProductionTestID) == false)
                {
                    AllProductionID.Add(ProductionTestID);
                }

                if (AllTestPins.Contains(PinNumber) == false)
                {
                    AllTestPins.Add(PinNumber);
                }

                if (BaselineData.ContainsKey(PinNumber) == false)
                {
                    BaselineData.Add(PinNumber, BaselineVoltage);
                }

                if (Pass == 1)
                {
                    if (BatchData.ContainsKey(PinNumber) == false)
                    {
                        BatchData.Add(PinNumber, new ProductionReportPinData(PinNumber, MeasuredVoltage));
                    }
                    else
                    {
                        BatchData[PinNumber].VoltageReadings.Add(MeasuredVoltage);
                    }
                }
            }

            TotalDevices = AllProductionID.Count;
            TotalPins = BaselineData.Keys.Count;
        }

        public void GenerateExcelOutput(String PartName, String BatchName)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

            excelApp.Visible = true;
            excelApp.DisplayAlerts = false;
            excelApp.ScreenUpdating = false;

            int sheetIndex = 0;

            Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(
                        excelWorkbook.Sheets.get_Item(++sheetIndex),
                        Type.Missing, 1, Excel.XlSheetType.xlWorksheet);

            //excelSheet.Name = PartName + " - " + BatchName;

            Excel.Range excelRange = excelSheet.UsedRange;
            int CurrentCol = 0;
            int CurrentRow = 0;

            CurrentCol = 1;
            CurrentRow = 1;

            excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Part Name");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 1, PartName);

            CurrentCol = 1;
            CurrentRow = 2;

            excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Batch Name");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 1, BatchName);

            CurrentCol = 1;
            CurrentRow = 3;
            excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Total Devices");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 1, TotalDevices.ToString());


            CurrentCol = 1;
            CurrentRow = 4;
            excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Total Pins");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 1, TotalPins.ToString());

            CurrentCol = 4;
            CurrentRow = 1;

            excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Pin");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;

            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 1, "Baseline Voltage");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol + 1]).Font.Bold = true;

            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 2, "Average Voltage");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol + 2]).Font.Bold = true;

            excelRange.Cells.set_Item(CurrentRow, CurrentCol + 3, "StdDev Voltage");
            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol + 3]).Font.Bold = true;

            for (int PinIndex = 0; PinIndex < TotalPins; PinIndex++)
            {
                byte CurrentPin = AllTestPins[PinIndex];
                double BaselineVoltage = BaselineData[CurrentPin];
                double AverageVoltage = 0;
                double StdDevVoltage = 0;
                bool AllFail = false;
                if (BatchData.ContainsKey(CurrentPin) == true)
                {
                    if (BatchData[CurrentPin].VoltageReadings.Count > 0)
                    {
                        AverageVoltage = BatchData[CurrentPin].GetVoltageAverage();
                        StdDevVoltage = BatchData[CurrentPin].GetStandardDeviation();
                    }
                    else
                        AllFail = true;
                }
                else
                    AllFail = true;
                excelRange.Cells.set_Item(CurrentRow + 1 + PinIndex, CurrentCol, "Pin " + CurrentPin);
                excelRange.Cells.set_Item(CurrentRow + 1 + PinIndex, CurrentCol + 1, BaselineVoltage);
                if (AllFail == false)
                {
                    excelRange.Cells.set_Item(CurrentRow + 1 + PinIndex, CurrentCol + 2, AverageVoltage);
                    excelRange.Cells.set_Item(CurrentRow + 1 + PinIndex, CurrentCol + 3, StdDevVoltage);
                }
                else
                {
                    excelRange.Cells.set_Item(CurrentRow + 1 + PinIndex, CurrentCol + 2, "All Failed");
                    excelRange.Cells.set_Item(CurrentRow + 1 + PinIndex, CurrentCol + 3, "All Failed");
                }
            }

            Excel.Range oRange;
            Excel.Range c1 = excelSheet.Cells[1, 1];
            Excel.Range c2 = excelSheet.Cells[CurrentRow + 1 + TotalPins, CurrentCol + 3];
            oRange = (Excel.Range)excelSheet.get_Range(c1, c2);

            //oRange = excelSheet.get_Range(excelSheet.Cells[1, 1], excelSheet.Cells[CurrentRow + 1 + TotalPins, CurrentCol + 3]);

            oRange.EntireColumn.AutoFit();

            excelApp.ScreenUpdating = true;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
