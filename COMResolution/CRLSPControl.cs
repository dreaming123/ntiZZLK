using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using COMRW;
using System.IO;

namespace COMResolution
{
    public class CRLSPControl
    {
        public RWLSocketClient SPSocket = new RWLSocketClient();
        private LOG.Log log = new LOG.Log("PLC扫描数据解析日志", ".\\");
        private String ServerAddr = String.Empty;
        private int ServerPortRecv = 0, ServerPortSend = 0;
        private String ImagePath = String.Empty;

        public static byte[] MessageHeader_2000 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x05, 0x03, 0x08, 0x01, 0x0A, 0x00, 0x00, 0x00, 0x14, 0xFF, 0x02 };
        public static byte[] MessageHeader_2001 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x03, 0x03, 0x08, 0x01, 0x32, 0x00, 0x00, 0x00, 0x14, 0xFF, 0x02 };

        public CRLSPControl()
        {
            ServerAddr = System.Configuration.ConfigurationSettings.AppSettings["SPIP"];
            ServerPortRecv = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["SPPortRecv"]);
            ServerPortSend = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["SPPortSend"]);
            //ImagePath = System.Configuration.ConfigurationSettings.AppSettings["ImagePath"];
        }

        public bool Run()
        {
            return SPSocket.Connect_SP(ServerAddr, ServerPortRecv);
        }

        public String ReadBarCode(ref String ItemCode)
        {
            String YBCode = String.Empty;
            byte[] bytBuffer = SPSocket.RecvSP();
            if (bytBuffer != null)
            {
                String Temp = Encoding.Default.GetString(bytBuffer).Trim('\0').Trim('\r');
                YBCode = Temp.Split(',')[2].Trim();
                ItemCode = Temp.Split(',')[1].Trim();
            }
            return YBCode;

        }

        public bool SendCmmd(String cmmd)
        {
            byte[] bytBuffer = (new UnicodeEncoding()).GetBytes(cmmd);
            return SPSocket.Send(bytBuffer);
        }

        public void Close()
        {
            SPSocket.Close();
        }

        public byte[] ReadImage()
        {
            string[] files = Directory.GetFiles(ImagePath);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            byte[] byteImage = GetPictureBytes(ImagePath+files[0]);
            return byteImage;
        }

        public static byte[] GetPictureBytes(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            byte[] buffer = new byte[fileInfo.Length];
            using (FileStream stream = fileInfo.OpenRead())
            {
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}
