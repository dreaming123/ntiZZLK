using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bap.LibNodave
{
    /// <summary>
    /// file descriptor，在Unix/Linux系统下，一个socket句柄，可以看做是一个文件，在socket上收发数据，相当于对一个文件进行读写，
    /// 所以一个socket句柄，通常也用表示文件句柄的fd来表示。
    /// </summary>
    public struct FD
    {
        public int rfd;
        public int wfd;
    }
}
