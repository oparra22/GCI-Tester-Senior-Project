using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace GCITester
{
    public class LifeTimeReportHourData
    {
        public int Hour;
        public Double Temperature;
        public DateTime DateMeasured;
        public DateTime BaselineDate;
        public int BaselineID = 0;
        public bool BaselineInitialized;
        public Dictionary<Byte, Double> VoltageReadings;

        public LifeTimeReportHourData(int Hour)
        {
            this.Hour = Hour;
            BaselineInitialized = false;
            BaselineID = 0;
            VoltageReadings = new Dictionary<byte, double>();
        }

        public void AddData(byte Pin, double Voltage)
        {
            if (VoltageReadings.ContainsKey(Pin) == false)
            {
                VoltageReadings.Add(Pin, Voltage);
            }
        }
    }

    public class LifeTimeReportData
    {
        public List<String> AllSerialNumbers = new List<string>();
        public List<int> AllTestHours = new List<int>();
        public List<Byte> AllTestPins = new List<byte>();
        public Dictionary<String, Dictionary<int, LifeTimeReportHourData>> ParsedData;  //SerialNumber, TestHour, VoltageData
        public LifetimeLimitEntity LimitData;
        DataTable RawData;
        int TotalNumberOfPins = 0;

        public LifeTimeReportData(DataTable dtInput, LifetimeLimitEntity LimitData)
        {
            this.LimitData = LimitData;
            RawData = dtInput;
            AllSerialNumbers = new List<string>();
            AllTestHours = new List<int>();
            AllTestPins = new List<byte>();
            ParsedData = new Dictionary<string, Dictionary<int, LifeTimeReportHourData>>();
            GetUniqueSerialNumbers();
            GetUniqueTestHours();
            PopulateData();
        }



        public void PopulateData()
        {
            foreach (String SerialNumber in AllSerialNumbers)
            {
                Dictionary<int, LifeTimeReportHourData> Entry = new Dictionary<int, LifeTimeReportHourData>();
                foreach (int Hour in AllTestHours)
                {
                    Entry.Add(Hour, new LifeTimeReportHourData(Hour));
                }
                ParsedData.Add(SerialNumber, Entry);
            }

            //int BaselineID = 0;
            //DateTime BaselineDate = DateTime.Now;

            //bool Initialized = false;

            foreach (DataRow drRow in RawData.Rows)
            {
                int LifetimeTestID = Convert.ToInt16(drRow["LifetimeTestID"]);
                DateTime DateMeasured = Convert.ToDateTime(drRow["CreationDate"]);
                String SerialNumber = Convert.ToString(drRow["SerialNumber"]);
                int TestHour = Convert.ToInt16(drRow["TestHour"]);
                Byte PinNumber = Convert.ToByte(drRow["DUTPinNumber"]);
                Double Voltage = Convert.ToDouble(drRow["AverageVoltage"]);
                double Temperature = Convert.ToDouble(drRow["Temperature"]);
                ParsedData[SerialNumber][TestHour].Temperature = Temperature;
                ParsedData[SerialNumber][TestHour].DateMeasured = DateMeasured;


                if (ParsedData[SerialNumber][TestHour].BaselineInitialized == false && TestHour == 0)
                {
                    ParsedData[SerialNumber][TestHour].BaselineInitialized = true;
                    ParsedData[SerialNumber][TestHour].BaselineDate = DateMeasured;
                    ParsedData[SerialNumber][TestHour].BaselineID = LifetimeTestID;
                }

                if (ParsedData[SerialNumber][TestHour].BaselineInitialized == true && TestHour == 0)
                {
                    if (DateMeasured > ParsedData[SerialNumber][TestHour].BaselineDate.AddSeconds(15))
                    {
                        ParsedData[SerialNumber][TestHour].BaselineID = LifetimeTestID;
                        ParsedData[SerialNumber][TestHour].BaselineDate = DateMeasured;
                        ParsedData[SerialNumber][TestHour].VoltageReadings = new Dictionary<byte, double>();
                    }
                }
                if (TestHour == 0)
                {
                    if (LifetimeTestID == ParsedData[SerialNumber][TestHour].BaselineID)
                        ParsedData[SerialNumber][TestHour].AddData(PinNumber, Voltage);
                }
                else
                    ParsedData[SerialNumber][TestHour].AddData(PinNumber, Voltage);
                if (AllTestPins.Contains(PinNumber) == false)
                    AllTestPins.Add(PinNumber);


            }
            TotalNumberOfPins = ParsedData[AllSerialNumbers[0]][AllTestHours[0]].VoltageReadings.Count;
        }

        public void GetUniqueSerialNumbers()
        {
            foreach (DataRow drRow in RawData.Rows)
            {
                String SerialNumber = Convert.ToString(drRow["SerialNumber"]);
                if (AllSerialNumbers.Contains(SerialNumber) == false)
                    AllSerialNumbers.Add(SerialNumber);
            }
        }

        public void GetUniqueTestHours()
        {
            foreach (DataRow drRow in RawData.Rows)
            {
                int TestHour = Convert.ToInt16(drRow["TestHour"]);
                if (AllTestHours.Contains(TestHour) == false)
                    AllTestHours.Add(TestHour);
            }
        }

        public void MakePinTable(Excel.Worksheet excelSheet, int StartRow, int StartCol, String SerialNumber, double LowerRange, double UpperRange)
        {
            Excel.Range excelRange = excelSheet.UsedRange;

            excelRange.Cells.set_Item(StartRow, StartCol, "Part Reference:");
            excelRange.Cells.set_Item(StartRow, StartCol + 1, SerialNumber);
            ((Excel.Range)excelSheet.Cells[StartRow, StartCol + 1]).Font.Underline = true;

            excelRange.Cells.set_Item(StartRow + 2, StartCol, "Date/Time");

            //excelRange.Cells.set_Item(StartRow+2, StartCol, "Pin");

            // Create Pin Column
            for (int i = 0; i < TotalNumberOfPins; i++)
            {
                excelRange.Cells.set_Item(StartRow + 4 + i, StartCol, "Pin " + AllTestPins[i].ToString());
                ((Excel.Range)excelSheet.Cells[StartRow + 4 + i, StartCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)excelSheet.Cells[StartRow + 4 + i, StartCol]).Borders.Weight = 3;
            }

            int CurrentRow = 1, CurrentCol = 1;
            int TestHourIndex = 0;
            LifeTimeReportHourData Baseline = ParsedData[SerialNumber][0];
            foreach (int TestHour in ParsedData[SerialNumber].Keys)
            {
                LifeTimeReportHourData Data = ParsedData[SerialNumber][TestHour];
                DateTime MeasuredDate = Data.DateMeasured;

                if (TestHour == 0)
                {
                    Baseline = Data;
                    CurrentRow = StartRow + 2;
                    CurrentCol = StartCol + 1;
                    excelRange.Cells.set_Item(CurrentRow, CurrentCol, MeasuredDate);
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 3;

                    CurrentRow = StartRow + 3;
                    CurrentCol = StartCol + 1;
                    excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Baseline [V]");
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 3;

                    for (int i = 0; i < TotalNumberOfPins; i++)
                    {
                        Byte Pin = AllTestPins[i];
                        CurrentRow = StartRow + 4 + i;
                        CurrentCol = StartCol + 1;
                        excelRange.Cells.set_Item(CurrentRow, CurrentCol, Data.VoltageReadings[Pin]);
                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        //Excel.XlBordersIndex.xlEdgeBottom
                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 2;
                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 3;
                        if (i == TotalNumberOfPins - 1)
                            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                    }
                }
                else
                {
                    CurrentRow = StartRow + 2;
                    CurrentCol = StartCol + 0 + TestHourIndex * 2;
                    excelRange.Cells.set_Item(CurrentRow, CurrentCol, MeasuredDate);
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 3;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol + 1]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol + 1]).Borders.Weight = 3;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol + 1]).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlLineStyleNone; ;

                    CurrentRow = StartRow + 3;
                    CurrentCol = StartCol + 0 + TestHourIndex * 2;
                    excelRange.Cells.set_Item(CurrentRow, CurrentCol, Data.Hour.ToString() + " hrs - " + Data.Temperature.ToString() + "°C [V]");
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 3;

                    CurrentRow = StartRow + 3;
                    CurrentCol = StartCol + 1 + TestHourIndex * 2;
                    excelRange.Cells.set_Item(CurrentRow, CurrentCol, "Delta [mV]");
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Font.Bold = true;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 3;

                    for (int i = 0; i < TotalNumberOfPins; i++)
                    {
                        Byte Pin = AllTestPins[i];

                        // Measured Data
                        CurrentRow = StartRow + 4 + i;
                        CurrentCol = StartCol + 0 + TestHourIndex * 2;
                        excelRange.Cells.set_Item(CurrentRow, CurrentCol, Data.VoltageReadings[Pin]);
                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 2;
                        //((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 3;
                        if (i == TotalNumberOfPins - 1)
                            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;

                        // Delta Data
                        CurrentRow = StartRow + 4 + i;
                        CurrentCol = StartCol + 1 + TestHourIndex * 2;
                        double Delta = (Data.VoltageReadings[Pin] - Baseline.VoltageReadings[Pin]) * 1000;
                        excelRange.Cells.set_Item(CurrentRow, CurrentCol, Delta);
                        if (Delta < LowerRange || Delta > UpperRange)
                        {
                            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Cells.Interior.Color = Excel.XlRgbColor.rgbRed;
                        }
                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders.Weight = 2;
                        ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 3;
                        if (i == TotalNumberOfPins - 1)
                            ((Excel.Range)excelSheet.Cells[CurrentRow, CurrentCol]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                    }
                }


                TestHourIndex++;
            }
            Excel.Range oRange;
            //oRange = excelSheet.get_Range(excelSheet.Cells[1, 1], excelSheet.Cells[CurrentRow, CurrentCol]);
            Excel.Range c1 = excelSheet.Cells[1, 1];
            Excel.Range c2 = excelSheet.Cells[CurrentRow, CurrentCol];
            oRange = (Excel.Range)excelSheet.get_Range(c1, c2);

            oRange.EntireColumn.AutoFit();
        }

        public void GenerateExcelOutput(String CustomerName, String PONumber, String Description, String PartName, String BatchName)
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

            //excelSheet.PageSetup.Zoom = false;
            //excelSheet.PageSetup.FitToPagesWide = 1;
            //excelSheet.Name = PartName + " - " + BatchName;

            Excel.Range excelRange = excelSheet.UsedRange;
            float Scale = 0.20F;
            excelSheet.Shapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\GCILogo.jpg", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 125, 10, 1350 * Scale, 300 * Scale);

            excelRange.Cells.set_Item(1, 1, "Customer Name");
            ((Excel.Range)excelSheet.Cells[1, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(1, 2, CustomerName);

            excelRange.Cells.set_Item(2, 1, "PO #");
            ((Excel.Range)excelSheet.Cells[2, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(2, 2, PONumber);

            excelRange.Cells.set_Item(3, 1, "Product Description");
            ((Excel.Range)excelSheet.Cells[3, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(3, 2, Description);


            excelRange.Cells.set_Item(4, 1, "Part Name");
            ((Excel.Range)excelSheet.Cells[4, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(4, 2, PartName);

            excelRange.Cells.set_Item(5, 1, "Job Name");
            ((Excel.Range)excelSheet.Cells[5, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(5, 2, BatchName);

            excelRange.Cells.set_Item(7, 1, "Lifetime Lower Range Limit");
            ((Excel.Range)excelSheet.Cells[7, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(7, 2, LimitData.LowerRange + " mV");

            excelRange.Cells.set_Item(8, 1, "Lifetime Upper Range Limit");
            ((Excel.Range)excelSheet.Cells[8, 1]).Font.Bold = true;
            excelRange.Cells.set_Item(8, 2, LimitData.UpperRange + " mV");
            //MessageBox.Show(excelSheet.HPageBreaks.Count.ToString());
            //MessageBox.Show(excelSheet.VPageBreaks[1].Location.Row.ToString());
            for (int SerialIndex = 0; SerialIndex < AllSerialNumbers.Count; SerialIndex++)
            {
                MakePinTable(excelSheet, 12 + ((TotalNumberOfPins + 6) * SerialIndex), 3, AllSerialNumbers[SerialIndex], LimitData.LowerRange, LimitData.UpperRange);
            }
            //MessageBox.Show("H:" + excelSheet.HPageBreaks.Count.ToString() + " V:" + excelSheet.VPageBreaks.Count.ToString());
            //MessageBox.Show(excelSheet.VPageBreaks[1].Location.Row.ToString());

            /*foreach (Excel.VPageBreak test in excelSheet.VPageBreaks)
            {
                MessageBox.Show(test.Location.Row.ToString() + " " + test.Location.Column.ToString());
            }

            foreach (Excel.HPageBreak test in excelSheet.HPageBreaks)
            {
                MessageBox.Show(test.Location.Row.ToString() + " " + test.Location.Column.ToString());
            }*/

            excelApp.ScreenUpdating = true;
            GC.Collect();
            GC.WaitForPendingFinalizers();


        }
    }
}
