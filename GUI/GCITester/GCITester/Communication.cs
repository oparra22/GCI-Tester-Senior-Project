using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Added Libraries
using System.IO.Ports;
using System.Threading;
using System.Windows;


namespace GCITester
{
    class Communication
    {
        //This class is responsible for Communication, mostly handling the serial port information!

        //Variables from previous system
        public static SerialPort comPort = new SerialPort();    //Create the port Object
        public static byte[] RecvBuffer = new Byte[0xFFFF];     //Initialize the Receiving Buffer to 0xFFFF - Why?
        public static int RecvBufferCurIndex = 0;               //Initialize index to 0

        public static bool PortOpen = false;

        //**********Need to go through these variables************
        public delegate void ResultComplete();
        public static event ResultComplete OnResultComplete;
        public delegate void OverFlowError();
        public static event OverFlowError OnOverFlowError;
        public static String PortName = String.Empty;
        public static int BaudRate = 9600;
        public static int DataBits = 8;
        public static Parity Parity = System.IO.Ports.Parity.None;
        public static StopBits StopBit = StopBits.One;

        public static bool DelegateInitialized = false;

        private static bool ReadingResult = false;
        private static int StartLoc = 0;
        public static int PinID1 = 0;
        public static int PinID2 = 0;
        public static int PinID3 = 0;
        public static int PinID4 = 0;

        public static int PinValue = 0;

        //Method for opening the port
        public static bool OpenPort()
        {
            try
            {
                //If Portname is still empty just like it is when it is initialized
                if (PortName.Length == 0)
                {
                    //PortName = "COM1";//Just for Prototyping
                    PortName = Properties.Settings.Default.ComPort;//ComPort is grabbed from the automatically generated code so need to figure out how to get this to work. AKA Set up settings
                }
                if (comPort.IsOpen == true)
                {
                    PortOpen = true;
                    comPort.Close();//If it is open, then close it instead of having a separate close method(double check why later)
                }
                //Delay? why
                Thread.Sleep(100);
                //This block was directly Copied and pasted from previous code
                comPort.BaudRate = Properties.Settings.Default.BaudRate;    //BaudRate
                comPort.DataBits = Properties.Settings.Default.DataBits;    //DataBits
                comPort.StopBits = Properties.Settings.Default.StopBits;    //StopBits
                comPort.Parity = Properties.Settings.Default.Parity;    //Parity
                comPort.PortName = PortName;   //PortName
                comPort.Handshake = Handshake.None;
                comPort.ReadTimeout = 1000;
                comPort.WriteTimeout = 1000;
                /********Block for debugging purposese only******/
                //Console.WriteLine($"baud rate{comPort.BaudRate}");
                //Console.WriteLine($"Data bits{comPort.DataBits}");
                //Console.WriteLine($"Stop Bits{comPort.StopBits}");
                //Console.WriteLine($"Parity{comPort.Parity}");
                //Console.WriteLine($"portName{comPort.PortName}");
                /************************************************/
                //comPort.ReceivedBytesThreshold = 10;

                //This block was also copied, need to double check what it does
                if (DelegateInitialized == false)
                {
                    DelegateInitialized = true;
                    comPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    comPort.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorHandler);
                }

                comPort.Open();
                comPort.DtrEnable = true;
                comPort.RtsEnable = true;
                Console.WriteLine($"Port Open = {comPort.IsOpen}");
                ClearBuffers();
                return true;//If try block completes then it was opened successfully, return true
            }//End Try Block
            catch
            {
                Console.WriteLine("catch method ran - Communication.cs");
                //AddOutput(ex.Message) //Commented out of previous code. debugging purposes. Test!
                return false;
            }//End catch
        }//End Open Port

        public static void ClearBuffers()
        {
            RecvBuffer = new byte[0xFFFF];
            Array.Clear(RecvBuffer, 0, 0xFFFF);
            RecvBufferCurIndex = 0;
        }

        public static void ErrorHandler(object sender, SerialErrorReceivedEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Please screen shot this and send to amagner@abound.com - " + e.
            switch (e.EventType)
            {
                case SerialError.Frame:
                    System.Windows.Forms.MessageBox.Show("Framing error");
                    break;
                case SerialError.Overrun:
                    //System.Windows.Forms.MessageBox.Show("Overrun error");

                    if (OnOverFlowError != null)
                        OnOverFlowError();
                    break;
                case SerialError.RXOver:
                    System.Windows.Forms.MessageBox.Show("RX Over");
                    break;
                case SerialError.RXParity:
                    System.Windows.Forms.MessageBox.Show("RX Parity");
                    break;
                case SerialError.TXFull:
                    System.Windows.Forms.MessageBox.Show("TX Parity");
                    break;
            }
        }


        private static void ResetRecvBuffer()
        {
            ReadingResult = false;
            RecvBuffer = new byte[0xFFFF];
            RecvBufferCurIndex = 0;
            Array.Clear(RecvBuffer, 0, 0xFFFF);
        }



        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType != SerialData.Chars)
            {
                return;
            }
            SerialPort sp = (SerialPort)sender;
            //string indata = sp.ReadExisting();
            string indata = string.Empty;

            byte[] buffer = new byte[sp.BytesToRead];
            int bytesRead = sp.Read(buffer, 0, buffer.Length);

            Console.WriteLine($"Bytes read= {bytesRead}");
            // message has successfully been received
            //indata = Encoding.ASCII.GetString(buffer, 0, bytesRead);


            for (int i = 0; i < bytesRead; i++)
            {


                byte Byte = buffer[i];


                Console.WriteLine($"Byte {i} = {buffer[i]}");
                RecvBuffer[RecvBufferCurIndex] = Byte;


                if (RecvBuffer[RecvBufferCurIndex] == 'R' && ReadingResult == false)
                {
                    ReadingResult = true;
                    StartLoc = RecvBufferCurIndex;
                    Console.WriteLine("R read");
                    /*PinID1 = (int)indata[i + 1];
                    PinValue = (indata[i + 2] << 8) | indata[i + 3];
                    if (OnResultComplete != null)
                        OnResultComplete();
                    RecvBuffer = new char[0xFFFF];
                    RecvBufferCurIndex = 0;
                    Array.Clear(RecvBuffer, 0, 0xFFFF);
                    return;*/

                }

                //if (ReadingResult == true && RecvBufferCurIndex >= 6)
                if (ReadingResult == true && RecvBufferCurIndex >= 7)
                {
                    //For debugging, comment out before final
                    Console.WriteLine($"Reading Result");
                    PinID1 = (int)RecvBuffer[StartLoc + 1];
                    PinID2 = (int)RecvBuffer[StartLoc + 2];
                    PinID3 = (int)RecvBuffer[StartLoc + 3];
                    PinID4 = (int)RecvBuffer[StartLoc + 4];
                    //Print Buffer to Console For Debugging purposes
                    Console.WriteLine($"buffer[{RecvBufferCurIndex}] = {RecvBuffer[RecvBufferCurIndex]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 1}] = {RecvBuffer[RecvBufferCurIndex + 1]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 2}] = {RecvBuffer[RecvBufferCurIndex + 2]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 3}] = {RecvBuffer[RecvBufferCurIndex + 3]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 4}] = {RecvBuffer[RecvBufferCurIndex + 4]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 5}] = {RecvBuffer[RecvBufferCurIndex + 5]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 6}] = {RecvBuffer[RecvBufferCurIndex + 6]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 7}] = {RecvBuffer[RecvBufferCurIndex + 7]}");
                    Console.WriteLine($"buffer[{RecvBufferCurIndex + 8}] = {RecvBuffer[RecvBufferCurIndex + 8]}");



                    Console.WriteLine($"PinID1 = {PinID1} PinID2{PinID2} Byte R = {(Byte)'R'}");
                    Console.WriteLine($"PinID3 = {PinID3} PinID4{PinID4} Byte R = {(Byte)'R'}");


                    PinValue = (RecvBuffer[StartLoc + 5] << 8) | RecvBuffer[StartLoc + 6];//adjusted
                    Console.WriteLine($"pinValue = {PinValue}");

                    //MessageBox.Show($"PinValue = {PinValue}");
                    //MessageBox.Show($"index {StartLoc + 5} = {RecvBuffer[StartLoc + 5]} - index {StartLoc + 6} = {RecvBuffer[StartLoc + 6]}");
                    //if (RecvBuffer[StartLoc + 7] == 255 && RecvBuffer[StartLoc + 8] == 255)
                    Console.WriteLine("Before Final check");
                    if (RecvBuffer[StartLoc + 7] == (Byte)'Z')
                        Console.WriteLine($"Final Character Read: Byte Z = {(Byte)'Z'}");
                    {
                        if (OnResultComplete != null)
                            OnResultComplete();
                        //Console.WriteLine("FUCK");  
                    }
                    ResetRecvBuffer();
                    return;
                }

                RecvBufferCurIndex++;
            }
        }

        //Function for testing a pin, used in manually test a pin screen
        public static void TestPin(Byte PinID1, Byte PinID2)
        {
            Console.WriteLine("Continuity Test Ran");

            if (!(comPort.IsOpen == true))
            {
                OpenPort();
            }

            Byte[] Data = new Byte[5];
            Data[0] = (Byte)'T';
            Data[1] = PinID1;
            Data[2] = PinID2;
            Data[3] = 0;
            Data[4] = 0;

            comPort.Write(Data, 0, 5);
            //debugging comment out of final
            Console.WriteLine("TestPin Method Finished");
        }
        public static void TestPinShort(Byte PinID1, Byte PinID2, Byte PinID3, Byte PinID4)
        {
            Console.WriteLine("Short Test Ran");
            if (!(comPort.IsOpen == true))
            {
                OpenPort();
            }

            Byte[] Data = new Byte[5];
            Data[0] = (Byte)'S';
            Data[1] = PinID1;
            Data[2] = PinID2;
            Data[3] = PinID3;
            Data[4] = PinID4;

            comPort.Write(Data, 0, 5); // was 3
            //debugging comment out of final
            Console.WriteLine("TestPin Method Finished");
        }

        public static bool ClosePort()
        {
            if (comPort.IsOpen == true)
            {
                PortOpen = false;
                comPort.Close();
            }
            return true;
        }


    }
}
