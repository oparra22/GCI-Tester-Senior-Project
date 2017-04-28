using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCITester
{
    public class TestPinEntity
    {
        public int TestBoardID = 0;
        public String BoardName = String.Empty;
        public int PartID = 0;
        public int SocketIndex = 0;
        public String SocketName = String.Empty;
        public int DUTPin = 0;
        public int GCIPin = 0;

        public TestPinEntity()
        {
            TestBoardID = 0;
            BoardName = String.Empty;
            PartID = 0;
            SocketName = String.Empty;
            DUTPin = 0;
            GCIPin = 0;
        }

        public TestPinEntity(int TestBoardID, String BoardName, int PartID, int SocketIndex, String SocketName, int DUTPin, int GCIPin)
        {
            this.TestBoardID = TestBoardID;
            this.BoardName = BoardName;
            this.PartID = PartID;
            this.SocketIndex = SocketIndex;
            this.SocketName = SocketName;
            this.DUTPin = DUTPin;
            this.GCIPin = GCIPin;
        }
    }
}
