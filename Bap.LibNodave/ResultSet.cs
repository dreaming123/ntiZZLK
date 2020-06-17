using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{
    public class ResultSet : PseudoPointer
    {
        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern IntPtr daveNewResultSet();
        public ResultSet()
        {
            Pointer = daveNewResultSet();
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern void daveFreeResults(IntPtr rs);
      
        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetErrorOfResult(IntPtr rs, int number);
        public int GetErrorOfResult(int number)
        {
            return daveGetErrorOfResult(Pointer, number);
        }

        public override void Dispose()
        {
            daveFreeResults(Pointer);
        }

    }
}
