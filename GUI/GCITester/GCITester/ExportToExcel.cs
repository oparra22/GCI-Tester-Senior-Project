using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;
using System.ComponentModel;

namespace GCITester
{
    class ExportToExcel
    {
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


        public static void FastExportToExcel(DataSet dataSet)
        {
            // Create the Excel Application object
            Excel.Application excelApp = new Excel.Application();

            // Create a new Excel Workbook
            Excel.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);
            excelApp.Visible = true;
            excelApp.DisplayAlerts = false;

            int sheetIndex = 0;

            // Copy each DataTable
            foreach (System.Data.DataTable dt in dataSet.Tables)
            {

                // Copy the DataTable to an object array
                object[,] rawData = new object[dt.Rows.Count + 1, dt.Columns.Count];

                // Copy the column names to the first row of the object array
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    rawData[0, col] = dt.Columns[col].ColumnName;
                }

                // Copy the values to the object array
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        //if (typeof(dt.Rows[row].ItemArray[col]) == DateTime)

                        rawData[row + 1, col] = dt.Rows[row].ItemArray[col].ToString();
                    }
                }

                // Calculate the final column letter
                string finalColLetter = string.Empty;
                string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int colCharsetLen = colCharset.Length;

                if (dt.Columns.Count > colCharsetLen)
                {
                    finalColLetter = colCharset.Substring(
                                (dt.Columns.Count - 1) / colCharsetLen - 1, 1);
                }

                finalColLetter += colCharset.Substring(
                                        (dt.Columns.Count - 1) % colCharsetLen, 1);

                // Create a new Sheet
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(
                            excelWorkbook.Sheets.get_Item(++sheetIndex),
                            Type.Missing, 1, Excel.XlSheetType.xlWorksheet);

                excelSheet.Name = dt.TableName;

                // Fast data export to Excel
                string excelRange = string.Format("A1:{0}{1}",
                            finalColLetter, dt.Rows.Count + 1);


                excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;

                // Mark the first row as BOLD
                ((Excel.Range)excelSheet.Rows[1, Type.Missing]).Font.Bold = true;

            }

            // Save and Close the Workbook
            /*excelWorkbook.SaveAs(outputPath, XlFileFormat.xlWorkbookNormal, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            excelWorkbook = null;

            // Release the Application object
            excelApp.Quit();
            excelApp = null;*/

            // Collect the unreferenced objects
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        public static void FastExportToExcel(DataTable dt)
        {
            // Create the Excel Application object
            Excel.Application excelApp = new Excel.Application();
            Excel.Range oRange;
            // Create a new Excel Workbook
            Excel.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);
            excelApp.Visible = true;
            excelApp.DisplayAlerts = false;

            int sheetIndex = 0;

            // Copy each DataTable


            // Copy the DataTable to an object array
            object[,] rawData = new object[dt.Rows.Count + 1, dt.Columns.Count];
            int DateTimeCol = 0;
            // Copy the column names to the first row of the object array
            for (int col = 0; col < dt.Columns.Count; col++)
            {
                rawData[0, col] = dt.Columns[col].ColumnName;
                if (dt.Columns[col].DataType == typeof(DateTime))
                {
                    DateTimeCol = col + 1;

                }
            }

            // Copy the values to the object array
            for (int col = 0; col < dt.Columns.Count; col++)
            {
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    rawData[row + 1, col] = dt.Rows[row].ItemArray[col];
                }
            }

            // Calculate the final column letter
            string finalColLetter = string.Empty;
            string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int colCharsetLen = colCharset.Length;

            if (dt.Columns.Count > colCharsetLen)
            {
                finalColLetter = colCharset.Substring(
                            (dt.Columns.Count - 1) / colCharsetLen - 1, 1);
            }

            finalColLetter += colCharset.Substring(
                                    (dt.Columns.Count - 1) % colCharsetLen, 1);

            // Create a new Sheet
            Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(
                        excelWorkbook.Sheets.get_Item(++sheetIndex),
                        Type.Missing, 1, Excel.XlSheetType.xlWorksheet);

            if (dt.TableName != string.Empty)
                excelSheet.Name = dt.TableName;
            else
                excelSheet.Name = "Table";

            // Fast data export to Excel
            string excelRange = string.Format("A1:{0}{1}",
                        finalColLetter, dt.Rows.Count + 1);


            excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;

            // Mark the first row as BOLD
            ((Excel.Range)excelSheet.Rows[1, Type.Missing]).Font.Bold = true;

            if (DateTimeCol > 0)
            {
                oRange = (Excel.Range)excelSheet.Cells[1, DateTimeCol];
                oRange.EntireColumn.NumberFormat = "MM/DD/YYYY HH:MM:SS";
            }

            oRange = excelSheet.get_Range(excelSheet.Cells[1, 1],
                          excelSheet.Cells[dt.Rows.Count, dt.Columns.Count]);
            oRange.EntireColumn.AutoFit();


            // Save and Close the Workbook
            /*excelWorkbook.SaveAs(outputPath, XlFileFormat.xlWorkbookNormal, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            excelWorkbook = null;

            // Release the Application object
            excelApp.Quit();
            excelApp = null;*/

            // Collect the unreferenced objects
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        public void SendToExcel(DataTable dt)
        {
            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            Excel.Range oRange;

            // Start Excel and get Application object. 
            oXL = new Excel.Application();

            // Set some properties 
            oXL.Visible = true;
            oXL.DisplayAlerts = false;

            // Get a new workbook. 
            oWB = oXL.Workbooks.Add(Missing.Value);

            // Get the active sheet 
            oSheet = (Excel.Worksheet)oWB.ActiveSheet;
            //oSheet.Name = "Customers";

            // Process the DataTable 
            // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE 
            //DataTable dt = Customers.RetrieveAsDataTable();

            int rowCount = 1;
            /*foreach (DataRow dr in dt.Rows)
            {
                rowCount += 1;
                for (int i = 1; i < dt.Columns.Count + 1; i++)
                {
                    // Add the header the first time through 
                    if (rowCount == 2)
                    {
                        oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                    }
                    oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
                }
            }*/

            oSheet.get_Range("A1:H25", Type.Missing).Value2 = dt.AsEnumerable().ToArray();

            //Excel.Range excelCell = (Excel.Range)oSheet.Cells[1, 1];
            //excelCell.Value2 = dt.AsEnumerable().ToArray();

            // Resize the columns 
            oRange = oSheet.get_Range(oSheet.Cells[1, 1],
                          oSheet.Cells[rowCount, dt.Columns.Count]);
            oRange.EntireColumn.AutoFit();

            // Save the sheet and close 
            oSheet = null;
            oRange = null;
            /*oWB.SaveAs("test.xls", Excel.XlFileFormat.xlWorkbookNormal,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Excel.XlSaveAsAccessMode.xlExclusive,
                Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);
            oWB.Close(Missing.Value, Missing.Value, Missing.Value);
            oWB = null;
            oXL.Quit();*/

            // Clean up 
            // NOTE: When in release mode, this does the trick 
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }
    }
}
