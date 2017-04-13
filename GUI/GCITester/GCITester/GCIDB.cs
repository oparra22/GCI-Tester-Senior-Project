using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
//using System.Windows.Forms;
using System.Data;

namespace GCITester
{
    class GCIDB
    {
        private static MySqlConnection connection;
        private static string server;
        private static string port;
        private static string database;
        private static string uid;
        private static string password;

        static bool ConnectionOpen = false;

        //Constructor
        public GCIDB()
        {
            Initialize();
        }

        //Initialize values
        public static void Initialize()
        {
            server = "localhost";
            port ="3306";
            database = "gci";
            uid = "root";
            password = "root";
            //password = "DieRemoval1";*/
            //server = Properties.Settings.Default.Database_Server;
            //database = Properties.Settings.Default.Database_Name;
            //uid = Properties.Settings.Default.Database_Username;
           // password = Properties.Settings.Default.Database_Password;

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "PORT=" + port + ";" + "DATABASE=" + 
		    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        public static bool OpenConnection()
        {
            try
            {
                if (ConnectionOpen == false)
                    connection.Open();
                ConnectionOpen = true;
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error );
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
                return false;
            }
        }

        //Close connection
        public static bool CloseConnection()
        {
            try
            {
                connection.Close();
                ConnectionOpen = false;
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void ChangePinMap(int TestBoardID, int PartID, byte DUTPin, byte GCITesterPin)
        {
            string formatForMySql = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //UPDATE `gci`.`testboard` SET `GCITesterPIN`='15', `LastEditDate`='2012-10-23 13:04:21' WHERE `TestBoardEntryID`='68';
            string query = "UPDATE GCI.TestBoard SET GCITesterPin='" + GCITesterPin +"' , LastEditDate='" + formatForMySql + "' WHERE PartID=" + PartID + " AND TestBoardID=" + TestBoardID + " AND DUTPin=" + DUTPin;
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public static void AddTestPinMap(int TestBoardID, string BoardName, int PartID, int SocketIndex,String SocketName, byte DUTPin, byte GCITesterPin,DateTime UploadTime)
        {
            string formatForMySql = UploadTime.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO GCI.TestBoard (TestBoardID,BoardName,PartID,SocketIndex,SocketName,DUTPin,GCITesterPin,CreationDate,LastEditDate) VALUES('" + TestBoardID + "','" + BoardName + "','" + PartID + "','" + SocketIndex + "','" + SocketName + "','" + DUTPin + "','" + GCITesterPin + "','" + formatForMySql + "','" + formatForMySql + "')";
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public static void AddProductionTestData(string BatchName, int ProductionTestID, int PartID, int LimitID, byte DUTPinNumber, int MeasurementNumber, double MeasuredVoltage, double AverageVoltage, double StdDevVoltage, bool Result, DateTime UploadTime)
        {
            string formatForMySql = UploadTime.ToString("yyyy-MM-dd HH:mm:ss");
            string ResultBit = "0";
            if (Result == true)
                ResultBit = "1";
            string query = "INSERT INTO GCI.ProductionData (BatchName,ProductionTestID,PartID,ProductionLimitID,DUTPinNumber,MeasurementNumber,MeasuredVoltage,AverageVoltage,StdDevVoltage,TestResult,CreationDate) VALUES('" + BatchName + "','" + ProductionTestID + "','" + PartID + "','" + LimitID + "','" + DUTPinNumber + "','" + MeasurementNumber + "','" + MeasuredVoltage + "','" + AverageVoltage + "','" + StdDevVoltage + "'," + ResultBit + ",'" + formatForMySql + "')";
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public static int GetMostRecentLifetimeTestID_BaseLine(int PartID, String SerialNumber, String BatchName)
        {
            string query = @"SELECT LifetimeTestID,CreationDate FROM GCI.LifetimeData
                            WHERE PartID = " + PartID + " AND TestHour=0 AND MeasurementNumber=0 AND SerialNumber='" + SerialNumber + "' AND BatchName = '" + BatchName + "'";
            DateTime MostRecentDate = new DateTime();
            int MostRecentLifetimeTestID = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                bool Initialize = false;

                while (dataReader.Read())
                {
                    DateTime CreationDate = Convert.ToDateTime(dataReader["CreationDate"]);
                    int LifetimeTestID = Convert.ToInt16(dataReader["LifetimeTestID"]);
                    if (Initialize == false)
                    {
                        Initialize = true;
                        MostRecentDate = CreationDate;
                        MostRecentLifetimeTestID = LifetimeTestID;
                    }

                    if (CreationDate > MostRecentDate)
                    {
                        MostRecentDate = CreationDate;
                        MostRecentLifetimeTestID = LifetimeTestID;
                    }
                    /*Byte DUTPinNumber = Convert.ToByte(dataReader["DUTPinNumber"]);
                    double AverageVoltage = Convert.ToDouble(dataReader["AverageVoltage"]);
                    if (Results.ContainsKey(DUTPinNumber) == false)
                    {
                        Results.Add(DUTPinNumber, AverageVoltage);
                    }*/
                }
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
            }
            return MostRecentLifetimeTestID;
        }

        public static Dictionary<Byte, Double> GetLifetimeBaselineData(int LifeTimeTestID)
        {
            Dictionary<Byte, Double> Results = new Dictionary<byte, double>();

            //string query = @"SELECT LifetimeTestID,DUTPinNumber,AverageVoltage FROM GCI.LifetimeData
            //                WHERE PartID = " + PartID + " AND TestHour=0 AND MeasurementNumber=0 AND SerialNumber='" + SerialNumber + "'";
            string query = @"SELECT DUTPinNumber,AverageVoltage FROM GCI.LifetimeData WHERE LifeTimeTestID = " + LifeTimeTestID + " AND TestHour=0 AND MeasurementNumber=0";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Byte DUTPinNumber = Convert.ToByte(dataReader["DUTPinNumber"]);
                    double AverageVoltage = Convert.ToDouble(dataReader["AverageVoltage"]);
                    if (Results.ContainsKey(DUTPinNumber) == false)
                    {
                        Results.Add(DUTPinNumber, AverageVoltage);
                    }
                }
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
            }

            return Results;
        }

        public static DataTable GetProductionData(String PartName, String BatchName)
        {
            DataTable dtResult = new DataTable();
            string query = @"SELECT * FROM GCI.ProductionData_v WHERE PartName='" + PartName + "' AND BatchName='" + BatchName + "' ORDER BY CreationDate,DUTPinNumber ASC";
           // string query = "SELECT * from productiondata";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                // MySqlDataAdapter TestAdapter = new MySqlDataAdapter(cmd);
                dtResult.Load(cmd.ExecuteReader());

               // productionReport.dt = dtResult;

               // return dtResult;
               


                //TestAdapter.Fill(dtResult);
                //TestAdapter.Dispose();
                //dataGrid.DataContext = dtResult;
                /*while (dataReader.Read())
                {
                    Byte DUTPinNumber = Convert.ToByte(dataReader["DUTPinNumber"]);
                    double AverageVoltage = Convert.ToDouble(dataReader["AverageVoltage"]);
                    if (Results.ContainsKey(DUTPinNumber) == false)
                    {
                        Results.Add(DUTPinNumber, AverageVoltage);
                    }
                }
                dataReader.Close();*/

                //close Connection
                CloseConnection();

                //return list to be displayed
            }

            return dtResult;
        }

        public static DataTable GetLifetimeData(String PartName, String BatchName, List<String> SerialNumbers)
        {
            DataTable dtResult = new DataTable();

            string sqlSerialNumbersList = string.Empty; 
            for (int i = 0;i< SerialNumbers.Count;i++)
            {
                string SerialNumber = SerialNumbers[i];
                sqlSerialNumbersList += "'" + SerialNumbers[i] + "'";
                if (i < SerialNumbers.Count - 1)
                    sqlSerialNumbersList += ",";
            }
            string query = @"SELECT LifetimeTestID,CreationDate,PartName,BatchName,SerialNumber,TestHour,Temperature,DUTPinNumber,AverageVoltage FROM GCI.LifetimeData_v_average WHERE PartName='" + PartName + "' AND BatchName='" + BatchName + "' AND SerialNumber IN (" + sqlSerialNumbersList + ") ORDER BY SerialNumber,TestHour, DUTPinNumber ASC";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                MySqlDataAdapter TestAdapter = new MySqlDataAdapter(cmd);

                

                TestAdapter.Fill(dtResult);
                TestAdapter.Dispose();

                /*while (dataReader.Read())
                {
                    Byte DUTPinNumber = Convert.ToByte(dataReader["DUTPinNumber"]);
                    double AverageVoltage = Convert.ToDouble(dataReader["AverageVoltage"]);
                    if (Results.ContainsKey(DUTPinNumber) == false)
                    {
                        Results.Add(DUTPinNumber, AverageVoltage);
                    }
                }
                dataReader.Close();*/

                //close Connection
                CloseConnection();

                //return list to be displayed
            }
            
            return dtResult;
        }

        public static void AddLifetimeTestData(int LifetimeTestID, string SerialNumber, string BatchName, int PartID, int TestHour, int LifetimeLimitID, double Temperature, byte DUTPinNumber, int MeasurementNumber, double MeasuredVoltage, double AverageVoltage, double StdDevVoltage, DateTime UploadTime)
        {
            string formatForMySql = UploadTime.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO GCI.LifetimeData (LifetimeTestID,SerialNumber,BatchName,PartID,TestHour,LifetimeLimitID,Temperature,DUTPinNumber,MeasurementNumber,MeasuredVoltage,AverageVoltage,StdDevVoltage,CreationDate) VALUES('" + LifetimeTestID + "','" + SerialNumber + "','" + BatchName + "','" + PartID +"','" + TestHour + "','" + LifetimeLimitID + "','" + Temperature +  "','" + DUTPinNumber + "','" + MeasurementNumber + "','" + MeasuredVoltage + "','" + AverageVoltage + "','" + StdDevVoltage + "','"  + formatForMySql + "')";
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public static bool AddProductionLimit(int ProductionLimitID, byte PinID, double UCL, double LCL, double AverageVoltage, double StdDevVoltage)
        {
            string formatForMySql = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO GCI.ProductionLimits (ProductionLimitID, DUTPinNumber,LowerControlLimit,UpperControlLimit,CreationDate,AverageVoltage,StdDevVoltage) VALUES('" + ProductionLimitID + "','" + PinID + "','" + LCL + "','" + UCL + "','" + formatForMySql + "','" + AverageVoltage + "','" + StdDevVoltage + "')";

            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
            return true;
        }

        public static int AddPartID(string PartName)
        {
            int PartID = GetPartID(PartName);
            if (PartID == 0)
            {
                string formatForMySql = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string query = "INSERT INTO GCI.Part (PartName,IsActive,CreationDate,LastEditDate) VALUES('" + PartName + "',1,'" + formatForMySql + "','" + formatForMySql + "')";

                //open connection
                if (OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                }

                PartID = GetPartID(PartName);
            }
            return PartID;
        }

        public static int GetPartID(string PartName)
        {
            string query = @"SELECT PartID FROM GCI.Part
                            WHERE PartName = '" + PartName + "' AND IsActive=1";
            int Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Result = Convert.ToInt16(dataReader["PartID"]);

                }
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result;
            }
            return 0;
        }

        public static int GetNextLifetimeTestID()
        {
            string query = @"SELECT MAX(LifetimeTestID) As NextLifetimeTestID FROM GCI.LifetimeData";
            int? Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["NextLifetimeTestID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["NextLifetimeTestID"]) + 1;
                    }
                    else
                        Result = 1;

                }

                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result.Value;
            }
            return 0;
        }

        public static int GetNextProductionTestID()
        {
            string query = @"SELECT MAX(ProductionTestID) As NextProductionTestID FROM GCI.ProductionData";
            int? Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["NextProductionTestID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["NextProductionTestID"]) + 1;
                    }
                    else
                        Result = 1;

                }

                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result.Value;
            }
            return 0;
        }

        public static int GetNextProductionLimitID()
        {
            string query = @"SELECT MAX(ProductionLimitID) As NextLimitID FROM GCI.ProductionLimits";
            int? Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["NextLimitID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["NextLimitID"]) + 1;
                    }
                    else
                        Result = 1;
                    
                }
                
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result.Value;
            }
            return 0;
        }

        public static int GetNextTestBoardID()
        {
            string query = @"SELECT MAX(TestBoardID) As NextTestBoardID FROM GCI.TestBoard";
            int? Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["NextTestBoardID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["NextTestBoardID"]) + 1;
                    }
                    else
                        Result = 1;

                }

                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result.Value;
            }
            return 0;
        }

        public static void DeletePart(String PartName)
        {
            //DELETE FROM `gci`.`testboard` WHERE `TestBoardEntryID`='27';
            int PartID = GetPartID(PartName);
            if (PartID != 0)
            {

                //string query = "DELETE FROM GCI.Part WHERE PartID=" + PartID;
                string query = "UPDATE GCI.Part SET IsActive=0 WHERE PartID=" + PartID.ToString();

                //open connection
                if (OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                }


            }
        }

        public static void DeleteBoard(String PartName, String BoardName)
        {
            //DELETE FROM `gci`.`testboard` WHERE `TestBoardEntryID`='27';
            int PartID = GetPartID(PartName);
            if (PartID != 0)
            {

                string query = "DELETE FROM GCI.TestBoard WHERE PartID=" + PartID + " AND BoardName='" +BoardName + "'";

                //open connection
                if (OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                }

                
            }
        }

        public static List<string> GetTestBoardList(String PartName)
        {
            List<string> Result = new List<string>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DISTINCT BoardName FROM GCI.TestBoard WHERE PartID = " + PartID + " ORDER BY BoardName ASC";
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string BoardName = Convert.ToString(dataReader["BoardName"]);
                        Result.Add(BoardName);
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static List<string> GetBatchNameList(String PartName)
        {
            List<string> Result = new List<string>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DISTINCT BatchName FROM GCI.lifetimedata_v_average WHERE PartName='" + PartName + "'";
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string BatchName = Convert.ToString(dataReader["BatchName"]);
                        Result.Add(BatchName);
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static List<string> GetProductionBatchNameList(String PartName)
        {
            List<string> Result = new List<string>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DISTINCT BatchName FROM GCI.ProductionData_V WHERE PartName='" + PartName + "'";
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string BatchName = Convert.ToString(dataReader["BatchName"]);
                        Result.Add(BatchName);
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static List<string> GetSerialNumberList(String PartName, String BatchName)
        {
            List<string> Result = new List<string>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DISTINCT SerialNumber FROM GCI.lifetimedata_v_average WHERE PartName='" + PartName + "' AND BatchName='" + BatchName + "' AND TestHour=0 ORDER BY SerialNumber ASC";
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string SerialNumber = Convert.ToString(dataReader["SerialNumber"]);
                        Result.Add(SerialNumber);
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static List<string> GetSerialNumberList(String PartName)
        {
            List<string> Result = new List<string>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DISTINCT SerialNumber FROM GCI.LifetimeData WHERE PartID = " + PartID;
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string SerialNumber = Convert.ToString(dataReader["SerialNumber"]);
                        Result.Add(SerialNumber);
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static List<string> GetPartList()
        {
            List<string> Result = new List<string>();
            string query = @"SELECT PartName FROM GCI.Part WHERE IsActive=1 ORDER BY PartName ASC";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string PartName = Convert.ToString(dataReader["PartName"]);
                    Result.Add(PartName);
                }
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                
            }
            return Result;
        }

        public static Dictionary<Byte,Byte> GetPinMap(string PartName, string BoardName, string SocketName)
        {
            Dictionary<Byte, Byte> Result = new Dictionary<Byte, Byte>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DUTPin, GCITesterPin FROM GCI.TestBoard WHERE PartID = " + PartID + " AND BoardName='" + BoardName + "' AND SocketName='" + SocketName + "'";
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        Byte DUTPin = Convert.ToByte(dataReader["DUTPin"]);
                        Byte GCITesterPin = Convert.ToByte(dataReader["GCITesterPin"]);
                        if (Result.ContainsKey(GCITesterPin) == false)
                        {
                            Result.Add(GCITesterPin,DUTPin);
                        }
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static List<string> GetSocketList(string PartName, string BoardName)
        {
            List<string> Result = new List<string>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                string query = @"SELECT DISTINCT SocketName FROM GCI.TestBoard WHERE PartID = " + PartID + " AND BoardName='" + BoardName + "'";
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string SocketName = Convert.ToString(dataReader["SocketName"]);
                        Result.Add(SocketName);
                    }
                    dataReader.Close();

                    //close Connection
                    CloseConnection();

                    //return list to be displayed

                }
            }
            return Result;
        }

        public static int GetLatestLifetimeLimitID()
        {
            string query = @"SELECT MAX(LifetimeLimitID) As RecentID FROM GCI.LifetimeLimit";
            int? Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["RecentID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["RecentID"]);
                    }
                    else
                        Result = 0;

                }

                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result.Value;
            }
            return 0;
        }

        public static int AddNewLifetimeLimit(String PartName, double LowerRange, double UpperRange)
        {
            int PartID = GetPartID(PartName);
            if (PartID != 0)
            {
                string formatForMySql = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = "INSERT INTO GCI.LifeTimeLimit (LowerRange,UpperRange,CreationDate) VALUES('" + LowerRange + "','" + UpperRange + "','" + formatForMySql + "')";

                //Open connection
                if (OpenConnection() == true)
                {
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                    return GetLatestLifetimeLimitID();
                }
            }
            return 0;
        }

        public static void SetLifetimeLimit(string PartName, int LifetimeLimitID)
        {
            int PartID = GetPartID(PartName);
            if (PartID != 0)
            {

                string formatForMySql = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                string query = "UPDATE GCI.Part SET LifetimeLimitID=" + LifetimeLimitID.ToString() + ", LastEditDate='" + formatForMySql + "' WHERE PartID=" + PartID.ToString();

                //Open connection
                if (OpenConnection() == true)
                {
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                }
            }

        }

        public static int AssociatePartToNewProductionLimit(string PartName)
        {
            int PartID = GetPartID(PartName);
            if (PartID != 0)
            {
                
                int NewLimitID = GetNextProductionLimitID();
                string formatForMySql = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                string query = "UPDATE GCI.Part SET ProductionLimitID=" + NewLimitID.ToString() + ", LastEditDate='" + formatForMySql + "' WHERE PartID=" + PartID.ToString();

                //Open connection
                if (OpenConnection() == true)
                {
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                    return NewLimitID;
                }
            }
            return 0;
        }

        public static int GetLifetimeLimitID(string PartName)
        {
            string query = @"SELECT LifetimeLimitID FROM GCI.Part
                            WHERE PartName = '" + PartName + "'";
            int Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["LifetimeLimitID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["LifetimeLimitID"]);
                    }

                }
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result;
            }
            return 0;
        }

        public static int GetProductionLimitID(string PartName)
        {
            string query = @"SELECT ProductionLimitID FROM GCI.Part
                            WHERE PartName = '" + PartName + "' AND IsActive=1";
            int Result = 0;
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (dataReader["ProductionLimitID"] != DBNull.Value)
                    {
                        Result = Convert.ToInt16(dataReader["ProductionLimitID"]);
                    }
                    
                }
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Result;
            }
            return 0;
        }



        public static LifetimeLimitEntity GetLifetimeLimits(string PartName)
        {
            LifetimeLimitEntity Results = new LifetimeLimitEntity();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                int LifetimeLimitID = GetLifetimeLimitID(PartName);
                if (LifetimeLimitID != 0)
                {
                    string query = @"SELECT LowerRange, UpperRange FROM GCI.LifetimeLimit
                                    WHERE LifetimeLimitID = " + LifetimeLimitID.ToString();
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        //Create a data reader and Execute the command
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        //Read the data and store them in the list
                        while (dataReader.Read())
                        {
                            
                            double LowerRange = Convert.ToDouble(dataReader["LowerRange"]);
                            double UpperRange = Convert.ToDouble(dataReader["UpperRange"]);

                            Results = new LifetimeLimitEntity(LifetimeLimitID, UpperRange, LowerRange);
                        }
                        dataReader.Close();

                        //close Connection
                        CloseConnection();
                    }
                }
            }
            return Results;
        }


        public static List<TestPinEntity> GetTestPins(string PartName, string BoardName)
        {
            List<TestPinEntity> Results = new List<TestPinEntity>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                int ProductionLimitID = GetProductionLimitID(PartName);
                if (ProductionLimitID != 0)
                {
                    string query = @"SELECT TestBoardID,SocketIndex,SocketName, DUTPin, GCITesterPin FROM GCI.TestBoard
                                    WHERE PartID = " + PartID.ToString() + " AND BoardName = '" + BoardName + @"'
                                     ORDER BY TestBoardEntryID ASC";
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        //Create a data reader and Execute the command
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        //Read the data and store them in the list
                        while (dataReader.Read())
                        {
                            int SocketIndex = Convert.ToInt16(dataReader["SocketIndex"]);
                            string SocketName = Convert.ToString(dataReader["SocketName"]);
                            byte DUTPin = Convert.ToByte(dataReader["DUTPin"]);
                            byte GCIPin = Convert.ToByte(dataReader["GCITesterPin"]);
                            int TestBoardID = Convert.ToInt16(dataReader["TestBoardID"]);
                            Results.Add(new TestPinEntity(TestBoardID, BoardName, PartID, SocketIndex,SocketName, DUTPin, GCIPin));
                        }
                        dataReader.Close();

                        //close Connection
                        CloseConnection();
                    }
                }
            }
            return Results;
        }

        public static List<Byte> GetDUTPins(string PartName)
        {
            List<Byte> Results = new List<Byte>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                int ProductionLimitID = GetProductionLimitID(PartName);
                if (ProductionLimitID != 0)
                {
                    string query = @"SELECT DutPinNumber FROM GCI.ProductionLimits
                                    WHERE ProductionLimitID = " + ProductionLimitID.ToString() +
                                    " ORDER BY DutPinNumber ASC";
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        //Create a data reader and Execute the command
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        //Read the data and store them in the list
                        while (dataReader.Read())
                        {
                            byte PinID = Convert.ToByte(dataReader["DUTPinNumber"]);
                            Results.Add(PinID);
                        }
                        dataReader.Close();

                        //close Connection
                        CloseConnection();
                    }
                }
            }
            return Results;
        }

        public static List<LimitEntity> GetProductionLimits(string PartName)
        {
            List<LimitEntity> Results = new List<LimitEntity>();
            int PartID = GetPartID(PartName);

            if (PartID != 0)
            {
                int ProductionLimitID = GetProductionLimitID(PartName);
                if (ProductionLimitID != 0)
                {
                    string query = @"SELECT DutPinNumber, LowerControlLimit, UpperControlLimit,AverageVoltage,StdDevVoltage FROM GCI.ProductionLimits
                                    WHERE ProductionLimitID = " + ProductionLimitID.ToString();
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        //Create a data reader and Execute the command
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        //Read the data and store them in the list
                        while (dataReader.Read())
                        {
                            byte PinID = Convert.ToByte(dataReader["DUTPinNumber"]);
                            double LCL = Convert.ToDouble(dataReader["LowerControlLimit"]);
                            double UCL = Convert.ToDouble(dataReader["UpperControlLimit"]);
                            double Average = Convert.ToDouble(dataReader["AverageVoltage"]);
                            double StdDev = Convert.ToDouble(dataReader["StdDevVoltage"]);

                            Results.Add(new LimitEntity(PartID,ProductionLimitID,UCL,LCL,PinID,Average,StdDev));
                        }
                        dataReader.Close();

                        //close Connection
                        CloseConnection();
                    }
                }
            }
            return Results;
        }

        

    }
}
