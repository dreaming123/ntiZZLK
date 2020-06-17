using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bap.LibNodave
{
    public enum ResultCodes : int
    {
        ResCannotOpenSocket = -100,//无法打开socket
        ResNotConnected = -101,//未连接
        ResOK = 0,			/* means all ok */
        ResNoPeripheralAtAddress = 1,	/* CPU tells there is no peripheral at address */
        ResMultipleBitsNotSupported = 6, /* CPU tells it does not support to read a bit block with a */
        /* length other than 1 bit. */
        ResItemNotAvailable200 = 3,	/* means a a piece of data is not available in the CPU, e.g. */
        /* when trying to read a non existing DB or bit bloc of length<>1 */
        /* This code seems to be specific to 200 family. */
        
        ResItemNotAvailable = 10,	/* means a a piece of data is not available in the CPU, e.g. */
        /* when trying to read a non existing DB */

        AddressOutOfRange = 5,		/* means the data address is beyond the CPUs address range */
        WriteDataSizeMismatch = 7,	/* means the write data size doesn't fit item size */
        ResCannotEvaluatePDU = -123,     /* PDU is not understood by libnodave */
        ResCPUNoData = -124,
        UnknownError = -125,
        EmptyResultError = -126,
        EmptyResultSetError = -127,
        ResUnexpectedFunc = -128,
        ResUnknownDataUnitSize = -129,

        ResShortPacket = -1024,
        ResTimeout = -1025
    }
}
