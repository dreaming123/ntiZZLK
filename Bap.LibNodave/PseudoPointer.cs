using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{

    public class PseudoPointer : IDisposable
    {
        public IntPtr Pointer;

        [DllImport("libnodave.dll"/*, PreserveSig=false*/)]
        private static extern int daveFree(IntPtr p);
        protected static int Free(IntPtr p)
        {
            return (int)daveFree(p);
        }

        public virtual void Dispose()
        {
            Free(Pointer);
        }
    }
}
