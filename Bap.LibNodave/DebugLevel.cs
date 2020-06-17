using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{
    public enum DebugLevel : int
    {
        RawRead = 0x01,	/* Show the single bytes received */
        SpecialChars = 0x02,	/* Show when special chars are read */
        RawWrite = 0x04,	/* Show the single bytes written */
        ListReachables = 0x08,	/* Show the steps when determine devices in MPI net */
        InitAdapter = 0x10,	/* Show the steps when Initilizing the MPI adapter */
        Connect = 0x20,	/* Show the steps when connecting a PLC */
        Packet = 0x40,
        Byte = 0x80,
        Compare = 0x100,
        Exchange = 0x200,
        PDU = 0x400,	/* debug PDU handling */
        Upload = 0x800,	/* debug PDU loading program blocks from PLC */
        MPI = 0x1000,
        PrintErrors = 0x2000,	/* Print error messages */
        Passive = 0x4000,
        ErrorReporting = 0x8000,
        Open = 0x10000,
        All = 0x1ffff
    }
}
