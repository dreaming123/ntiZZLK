using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenSong
{
    public class GenSongSC : SCBase
    {
        public override byte[] RequestStatusBuffer(byte[] head)//
        {
            byte[] endStr = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            byte[] sendBuffer = new byte[30];
            sendBuffer[0] = 0x02;
            Array.Copy(head, 0, sendBuffer, 1, 2);
            sendBuffer[3] = 0x03;
            Array.Copy(endStr, 0, sendBuffer, 4, 26);
            return sendBuffer;
        }

        public override byte[] SendTaskBuffer(string SCNo, string SourcePosition, string Destination, string order, string TaskNo)
        {
            byte[] sendBuffer = new byte[34];
            try
            {
                sendBuffer[0] = 0x02;
                Array.Copy(Encoding.ASCII.GetBytes("HB"), 0, sendBuffer, 1, 2);
                Array.Copy(Encoding.ASCII.GetBytes(SCNo.Substring(SCNo.Length - 1, 1).PadLeft(2, '0')), 0, sendBuffer, 3, 2);
                Array.Copy(Encoding.ASCII.GetBytes(SourcePosition), 0, sendBuffer, 5, 9);
                Array.Copy(Encoding.ASCII.GetBytes(Destination), 0, sendBuffer, 14, 9);
                //Array.Copy(Encoding.ASCII.GetBytes(order), 0, sendBuffer, 19, 2);
                Array.Copy(Encoding.ASCII.GetBytes(TaskNo.PadLeft(10, '0')), 0, sendBuffer, 23, 10);
                sendBuffer[33] = 0x03;                
            }
            catch (Exception ex)
            {
            }
            return sendBuffer;
        }

        public override byte[] SendTaskBuffer(string SCNo, string SourcePosition, string Destination, string SecSourcePosition, string SecDestination, string order, string TaskNo)
        {
            byte[] sendBuffer = new byte[48];
            try
            {
                sendBuffer[0] = 0x02;
                Array.Copy(Encoding.ASCII.GetBytes("HB"), 0, sendBuffer, 1, 2);
                //Array.Copy(Encoding.ASCII.GetBytes(SCNo.Substring(SCNo.Length - 1, 1).PadLeft(2, '0')), 0, sendBuffer, 3, 2);
                Array.Copy(Encoding.ASCII.GetBytes(SourcePosition), 0, sendBuffer, 3, 8);
                Array.Copy(Encoding.ASCII.GetBytes(Destination), 0, sendBuffer, 11, 8);
                Array.Copy(Encoding.ASCII.GetBytes(SecSourcePosition), 0, sendBuffer, 19, 8);
                Array.Copy(Encoding.ASCII.GetBytes(SecDestination), 0, sendBuffer, 27, 8);
                Array.Copy(Encoding.ASCII.GetBytes(TaskNo.PadRight(10, '0')), 0, sendBuffer, 35, 10);
                string ii = (int.Parse(SCNo.Substring(SCNo.Length - 1, 1)) - 2).ToString();
                Array.Copy(Encoding.ASCII.GetBytes(ii.PadLeft(2, '0')), 0, sendBuffer, 45, 2);
                sendBuffer[47] = 0x03;
            }
            catch(Exception ex)
            {
            }
            return sendBuffer;
        }

        #region //发送出入库任务
        /// <summary>
        /// 发送出入库任务
        /// </summary>
        /// <param name="SRMNo"></param>
        /// <param name="SourcePosition"></param>
        /// <param name="Destination"></param>
        /// <param name="PalletNo"></param>
        /// <param name="TaskNo"></param>
        public override byte[] SendTaskMsg(string SCNo, string SourcePosition, string Destination, string PalletNo, string TaskNo)
        {
            return SendTaskBuffer(SCNo, SourcePosition, Destination, "11", TaskNo);
        }
        public override byte[] SendTaskMsg(string SCNo, string SourcePosition, string Destination, string SecSourcePosition, string SecDestination, string PalletNo, string TaskNo)
        {
            return SendTaskBuffer(SCNo, SourcePosition, Destination, SecSourcePosition, SecDestination, "11", TaskNo);
        }
        #endregion

        #region //发送紧急停止
        /// <summary>
        /// 发送紧急停止
        /// </summary>
        public override byte[] SCEStop()
        {
            return RequestStatusBuffer(Encoding.ASCII.GetBytes("HE"));

        }
        #endregion

        #region //发送取消停止
        /// <summary>
        /// 发送取消停止
        /// </summary>
        public override byte[] TaskCancelEStop()//取消停止
        {
            return RequestStatusBuffer(Encoding.ASCII.GetBytes("HC"));
        }
        #endregion

        #region //发送堆垛机回原点
        /// <summary>
        /// 发送堆垛机回原点
        /// </summary>
        public override byte[] SCBackOrigin()//发送堆垛机回原点
        {
            return RequestStatusBuffer(Encoding.ASCII.GetBytes("HP"));
        }
        #endregion

        #region //状态请求
        /// <summary>
        /// 状态请求
        /// </summary>
        /// <param name="strSrmNu"></param>
        public override byte[] RequestSRMState()
        {
            return RequestStatusBuffer(Encoding.ASCII.GetBytes("HA"));

        }
        #endregion

        #region 堆垛机反馈解析
        /// <summary>
        /// 堆垛机反馈解析
        /// </summary>
        /// <param name="strSrmNu"></param>
        public override object Analysis(string msg)
        {
            msg = msg.Replace("\0", "").Trim();
            string head = msg.Substring(1, 2);
            object ob = new object();
            switch (head)
            {
                case "LA": ob = ResponseStatus(msg); break;
                case "LB": ob = ResponseTaskConfirm(msg); break;
                default: ob = null; break;
            }
            return ob;

        }
        #endregion

        public override CRNStatus ResponseStatus(string msg)//堆垛机状态反馈解析
        {
            CRNStatus ss = new CRNStatus();
            msg = msg.Replace("\0", "").Trim();
            string[] rs = new string[msg.Length - 2];
            rs[0] = msg.Substring(3, 2);//堆垛机状态
            switch (rs[0])
            {
                case "00": rs[1] = "初始化"; break;
                case "01": rs[1] = "回原点"; break;
                case "02": rs[1] = "无货待命"; break;
                case "03": rs[1] = "有货待命"; break;
                case "04": rs[1] = "无货运行"; break;
                case "05": rs[1] = "有货运行"; break;
                case "06": rs[1] = "取货"; break;
                case "07": rs[1] = "放货"; break;
                case "08": rs[1] = "报警停机"; break;
                case "09": rs[1] = "故障复位"; break;
                case "10": rs[1] = "未知报文"; break;
                case "11": rs[1] = "未连接"; break;
                case "12": rs[1] = "手动操作"; break;
                default: rs[1] = "返回状态异常"; break;
            }
            rs[2] = msg.Substring(5, 3);//列号
            rs[3] = msg.Substring(8, 3);//层号
            rs[4] = msg.Substring(11, 1);//货叉位置
            switch (rs[4])
            {
                case "1": rs[5] = "中位"; break;
                case "2": rs[5] = "左（里）"; break;
                case "3": rs[5] = "左（里）极限"; break;
                case "4": rs[5] = "右（里）"; break;
                case "5": rs[5] = "右（里）极限"; break;
                case "6": rs[5] = "左（外）"; break;
                case "7": rs[5] = "左（外）极限"; break;
                case "8": rs[5] = "右（外）"; break;
                case "9": rs[5] = "右（外）极限"; break;
                default: rs[5] = "返回货叉位置异常"; break;
            }
            rs[6] = msg.Substring(12, 1);//货叉高低位
            switch (rs[6])
            {
                case "0": rs[7] = "中位"; break;
                case "1": rs[7] = "高位"; break;
                case "2": rs[7] = "低位"; break;
                default: rs[7] = "返回货叉高低位异常"; break;
            }
            rs[8] = msg.Substring(12, 1);//货叉高低位
            rs[9] = msg.Substring(13, 1);//是否在站点位置
            switch (rs[9])
            {
                case "0": rs[10] = "位置对"; break;
                case "1": rs[10] = "位置错"; break;
                default: rs[10] = "返回站点位置异常"; break;
            }
            rs[11] = msg.Substring(14, 4);//故障信息
            switch (rs[11])
            {
                case "0000": rs[12] = "设备正常"; break;
                default: rs[12] = "设备有故障"; break;
            }
            rs[13] = msg.Substring(18, 1);//事件类型
            switch (rs[13])
            {
                case "0": rs[14] = "初始化"; break;
                case "1": rs[14] = "运行"; break;
                case "2": rs[14] = "开始取货"; break;
                case "3": rs[14] = "取货完成"; break;
                case "4": rs[14] = "开始放货"; break;
                case "5": rs[14] = "放货完成"; break;
                case "6": rs[14] = "完成"; break;
                case "7": rs[14] = "急停"; break;
                case "8": rs[14] = "报警暂停"; break;
                case "9": rs[14] = "回原点"; break;
                default: rs[14] = "返回事件类型异常"; break;
            }
            rs[15] = int.Parse(msg.Substring(19, 7)).ToString();//任务号

            ss.CraneMode = rs[1];
            ss.AlarmInfo = rs[11];
            ss.AlarmMessage = (rs[11] == "0000" ? "" : rs[12]);
            ss.CrcStatus = (rs[11] == "0000" ? "Equipment_OK" : "Equipment_Error");
            if (rs[0] == "11" || rs[0] == "12")
            {
                ss.CrcStatus = "Equipment_Error";
            }
            ss.TaskNo = rs[15];
            ss.Position = rs[2] + rs[3];
            if (ss.OldLabelInfo == ss.LabelInfo)
                ss.Change = false;
            ss.OldLabelInfo = ss.LabelInfo;
            ss.LabelInfo = rs[1] + ";" + rs[5] + ";" + rs[7] + ";" + rs[10] + ";" + rs[14];

            return ss;
        }

        public override string[] ResponseTaskConfirm(string msg)//堆垛机接受任务后反馈解析
        {
            string[] rs = null;
            msg = msg.Replace("\0", "").Trim();
            rs[0] = msg.Substring(3, 1);//堆垛机状态
            switch (rs[0])
            {
                case "1": rs[1] = "接受任务正确"; break;
                case "2": rs[1] = "接受任务不正确"; break;
                default: rs[1] = "返回接受异常"; break;
            }
            rs[2] = int.Parse(msg.Substring(4, 8)).ToString();//任务号

            return rs;
        }

    }
}
