using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using COMResolution;
using LOG;
using System.Data;
using System.Threading;
using System.Collections;

namespace BLL
{
    public abstract class BLLBase
    {
        public DALDATA dal = new DALDATA();
        public CRLConveryControl ConveryClass = new CRLConveryControl();
        public Log logsta = new Log("业务逻辑", ".\\输送机日志\\");
        public Log logLocation = new Log("烟箱信息", ".\\数据操作\\");

        public static List<CRCStatus> CRCItems = new List<CRCStatus>();//卫星小车状态类
        public static List<CRCStatusB> CRCItemsB = new List<CRCStatusB>();//卫星小车状态类
        public static Hashtable AlarmHash = Hashtable.Synchronized(new Hashtable());//卫星小车详细报警信息存储
        public static List<CRCObject> crclist = new List<CRCObject>();//卫星小车实体类
        public static List<CRCObjectB> crclistB = new List<CRCObjectB>();//卫星小车实体类
        public static CRCTask crc = new CRCTask();//卫星小车任务类
        public static CRCTaskB crcb = new CRCTaskB();
        public static BLLConvery blc = new BLLConvery();
        public static Thread TaskThread;
        public static Thread LEDThread;
        public static Thread CallOutThread;

        //报头
        public static byte[] MessageHeader_2000 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x03, 0x03, 0x08, 0x01, 0x3C, 0x00, 0x00, 0x00, 0x0C, 0xFF, 0x02 };
        public static byte[] MessageHeader_4000 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x05, 0x03, 0x08, 0x01, 0x3D, 0x00, 0x00, 0x00, 0x0C, 0xFF, 0x02 };
        public static byte[] MessageHeader_6000 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x05, 0x03, 0x08, 0x01, 0x46, 0x00, 0x00, 0x00, 0x47, 0xFF, 0x02 };
        public static byte[] MessageHeaderHC_6000 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x05, 0x03, 0x08, 0x01, 0x46, 0x00, 0x00, 0x00, 0x3D, 0xFF, 0x02 };
        //PLC报头
        public static byte[] MessageHeader_PLC2000 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x05, 0x03, 0x08, 0x01, 0x0A, 0x00, 0x00, 0x00, 0x96, 0xFF, 0x02 };
        public static byte[] MessageHeader_PLC2001 = new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x03, 0x03, 0x08, 0x01, 0x0B, 0x00, 0x00, 0x00, 0x14, 0xFF, 0x02 };

        //FunctionReport
        public static string CR_GETC_EXECUTED = "121";//取货完成
        public static string CR_PUTC_EXECUTED = "137";//放货完成
        public static int CR_GETC = 112;//取货指令
        public static int CR_PUTC = 128;//放货指令
        public static int CR_NO_FUNC = 0;
        public static int CR_POS_GET = 16;//取货前行走
        public static string CR_POS_GET_EXECUTED = "25";//取货前行走完成

        //状态
        public static string EQUIPMENT_ERROR = "Equipment_Error";
        public static string EQUIPMENT_OK = "Equipment_OK";

        public DataTable GetTaskInfo(int tasktype, int taskstatus, string fromtime, string totime, string wmstaskno, string taskno, string fromstation, string tostation, string barcode)
        {
            return dal.GetTaskInfo(tasktype, taskstatus, fromtime, totime, wmstaskno, taskno, fromstation, tostation, barcode);
        }
        public bool DeleteSubTask(string taskno)
        {
            return dal.DeleteSubTask(taskno);
        }
        public bool InsertYSBatch(string rfid,int isend)
        {
            return dal.InsertYSBatch(rfid,isend);
        }

        public bool AddTask(string fromstation, string tostation)
        {
            return dal.AddTask(fromstation, tostation);
        }

        public string[] ReadStationInfo(int x, int y)
        {
            return ConveryClass.ReadStationInfo(x, y);
        }

        public DataTable GetSCMode()
        {
            return dal.GetSCMode();
        }
        //public string GetIsBCtoA()
        //{
        //    return dal.GetIsBCtoA();
        //}
        public bool UpdateIsBCtoA(string value)
        {
            return dal.UpdateIsBCtoA(value);
        }
        public string[] ReadStationInfo(int stationno)
        {
            return ConveryClass.ReadStationInfo(stationno);
        }
        public byte[] ReadStationInfoBy51(int stationno)
        {
            return ConveryClass.ReadStationInfoBy51(stationno);
        }
        public byte[] ReadStationInfoBy90(int stationno)
        {
            return ConveryClass.ReadStationInfoBy90(stationno);
        }
        //public byte[] ReadStationInfoBy556(int i)
        //{
        //    return ConveryClass.ReadStationInfoBy556(i);
        //}
        public string GetDestStationHis(string taskno)
        { 
            return dal.GetDestStationHis(taskno);
        }
        public string GetDestStation(string taskno)
        {
            return dal.GetDestStation(taskno);
        }
        public string GetLedText(string taskno)
        {
            return dal.GetLedText(taskno);
        }
        public bool WriteStationInfo(string[] str)
        {
            return ConveryClass.WriteStationInfo(str);
        }
        public bool WriteBits(int station, string value, int index)
        {
            return ConveryClass.WriteBits(station, value, index);
        }
        public bool WriteStationDB554(string[] str)
        {
            return ConveryClass.WriteStationDB554(str);
        }
        //public bool WriteItemCode(byte[] byt,int stationno)
        //{
        //    return ConveryClass.WriteItemCode(byt, stationno);
        //}
        public byte[] GetHexBytes(string info)
        {
            return ConveryClass.GetHexBytes(info);
        }
        public string GetWMSTaskNo(string taskno)
        {
            return dal.GetWMSTaskNo(taskno);
        }
        public string UpdateTaskStatus(string TaskNo, string TaskType, string TaskStatus, string RunStatus)
        {
            return dal.UpdateTaskStatus(TaskNo, TaskType, TaskStatus, RunStatus);
        }
        public string CallPalletOut(string TaskNo)
        {
            return dal.CallPalletOut(TaskNo);
        }
        public Boolean UpdateTaskStatus(string TaskNo, string TaskType, string TaskStatus, string RunStatus,int ScNo)
        { 
            return dal.UpdateTaskStatus(TaskNo, TaskType, TaskStatus, RunStatus, ScNo);
        }
        public Boolean UpdateEquipmentStatus(string TaskNo, string position, string functionMode, string taskStatus, string fromStion, string toStion, string alarmMessage, string ScNo)
        {
            return dal.UpdateEquipmentStatus(TaskNo, position, functionMode, taskStatus, fromStion, toStion, alarmMessage, ScNo);
        }

        public bool UpdateSCMode(string sc1in, string sc1out, string sc2in, string sc2out)
        {
            return dal.UpdateSCMode(sc1in, sc1out, sc2in, sc2out);
        }

        //获取烟丝入库任务
        public bool YS_GetInboundTask(string BinType, string SourceStation, string TaskNo1, string TaskNo2, Int32 BinQty,int ScNo, string UserID, ref string reftaskno)
        {
            return dal.YS_GetInboundTask(BinType, SourceStation, TaskNo1, TaskNo2, BinQty,ScNo, UserID, ref reftaskno);
        }
        //查询是否已经有正在出的空箱任务
        public int YS_QueryEmptyPalletTask(string StaNo)
        {
            return dal.YS_QueryEmptyPalletTask(StaNo);
        }
        //获取烟丝出库任务
        public bool YS_CreateOutBoundTask(string SourceStation, int TaskType, string ItemCode, string BatchNo, string BomNO, int BinQty, string BoxType, int ScNo, ref string taskNo)
        {
            return dal.YS_CreateOutBoundTask(SourceStation, TaskType, ItemCode, BatchNo, BomNO, BinQty, BoxType, ScNo, ref taskNo);
        }
        //查询正在出库实箱批次
        public DataSet YS_QueryBillWMSTask(string TaskNo)
        {
            return dal.YS_QueryBillWMSTask(TaskNo);
        }
        //查询正在出库的烟丝批次和牌号
        public DataSet YS_SerchMachineProductPlanExcuting()
        {
            return dal.YS_SerchMachineProductPlanExcuting();
        }
        //写入RFID信息到数据库
        public string GetTask(string taskno, string barcode, string goodstype, string fromstation, string tostation, string checkinfo, string field1, string field2, string field3, string items)//, string isbctoa)
        {
            return dal.GetTask(taskno, barcode, goodstype, fromstation, tostation, checkinfo, field1, field2, field3, items);//, isbctoa);
        }
        public string GetAllowSend(string itemid)
        {
            return dal.GetAllowSend(itemid);
        }
        public DataTable GetEventLog(string str, DateTime dts, DateTime dte, int ch)
        {
            return dal.GetEventLog(str, dts, dte, ch);
        }
        public DataTable GetOutrequest(string itemcode, string itemname, string barcode, DateTime dts, DateTime dte)
        {
            return dal.GetOutrequest(itemcode, itemname, barcode, dts, dte);
        }
        public string ReturnPark(string line)
        {
            string result = string.Empty;
            switch (line) 
            {
                case "14": result = "2011"; break;
                case "15": result = "2012"; break;
                case "16": result = "2021"; break;
            };

            return result;
        }
        public string GetTaskStatusBase(string code,string codeh)
        {
            //if (codeh == "1")
            //{
            //    #region
            //    switch (code)
            //    {
            //        case "112": return "CR_DROPSH_GETC_ WAIT_H";//等待取货
            //        case "113": return "CR_DROPSH_GETC_ POS_RUNNING";//取货运行中
            //        case "114": return "CR_DROPSH_GETC_ DROP_RUNNING";//取货伸叉
            //        case "121": return "CR_DROPSH_GETC_ EXECUTED";//取货完成
            //        case "122": return "CR_DROPSH_GETC_ FAILED";//取货失败
            //        case "123": return "CR_DROPSH_GETC_ ABORTED";//取货中止
            //        case "124": return "CR_DROPSH_GETC_ MF_FAULT";//
            //        case "125": return "CR_DROPSH_GETC_ LOC_EMPTY";
            //        case "126": return "CR_DROPSH_GETC_ PNOTREACH";
            //        case "128": return "CR_DROPSH_PUTC_ WAIT_H";
            //        case "129": return "CR_DROPSH_PUTC_ POS_RUNNING";
            //        case "130": return "CR_DROPSH_PUTC_ DROP_RUNNING";
            //        case "137": return "CR_DROPSH_PUTC_ EXECUTED";
            //        case "138": return "CR_DROPSH_PUTC_ FAILED";
            //        case "139": return "CR_DROPSH_PUTC_ ABORTED";
            //        case "140": return "CR_DROPSH_PUTC_ MF_FAULT";
            //        case "141": return "CR_DROPSH_PUTC_ LOC_FULL";
            //        case "142": return "CR_DROPSH_PUTC_ PNOTREACH";
            //        case "144": return "CR_DROPSH_PARK_ WAIT_H";
            //        case "145": return "CR_DROPSH_PARK_ POS_RUNNING";
            //        case "146": return "CR_DROPSH_PARK_ DROP_RUNNING";
            //        case "153": return "CR_DROPSH_PARK_ EXECUTED";
            //        case "154": return "CR_DROPSH_PARK_ FAILED";
            //        case "155": return "CR_DROPSH_PARK_ ABORTED";
            //        default: return "";
            //    }
            //    #endregion
            //}
            //else if (codeh == "2")
            //{
            //    #region
            //    switch (code)
            //    {
            //        case "112": return "CR_PICKSH_GETC_ WAIT_H";//等待取货
            //        case "113": return "CR_PICKSH_GETC_ POS_RUNNING";//取货运行中
            //        case "114": return "CR_PICKSH_GETC_ PICK_RUNNING";//取货伸叉
            //        case "121": return "CR_PICKSH_GETC_ EXECUTED";//取货完成
            //        case "122": return "CR_PICKSH_GETC_ FAILED";//取货失败
            //        case "123": return "CR_PICKSH_GETC_ ABORTED";//取货中止
            //        case "124": return "CR_PICKSH_GETC_ MF_FAULT";//
            //        case "125": return "CR_PICKSH_GETC_ LOC_EMPTY";
            //        case "126": return "CR_PICKSH_GETC_ PNOTREACH";
            //        case "128": return "CR_PICKSH_PUTC_ WAIT_H";
            //        case "129": return "CR_PICKSH_PUTC_ POS_RUNNING";
            //        case "130": return "CR_PICKSH_PUTC_ PICK_RUNNING";
            //        case "137": return "CR_PICKSH_PUTC_ EXECUTED";
            //        case "138": return "CR_PICKSH_PUTC_ FAILED";
            //        case "139": return "CR_PICKSH_PUTC_ ABORTED";
            //        case "140": return "CR_PICKSH_PUTC_ MF_FAULT";
            //        case "141": return "CR_PICKSH_PUTC_ LOC_FULL";
            //        case "142": return "CR_PICKSH_PUTC_ PNOTREACH";
            //        case "144": return "CR_PICKSH_PARK_ WAIT_H";
            //        case "145": return "CR_PICKSH_PARK_ POS_RUNNING";
            //        case "146": return "CR_PICKSH_PARK_ PICK_RUNNING";
            //        case "153": return "CR_PICKSH_PARK_ EXECUTED";
            //        case "154": return "CR_PICKSH_PARK_ FAILED";
            //        case "155": return "CR_PICKSH_PARK_ ABORTED";
            //        default: return "";
            //    }
            //    #endregion
            //}
            //else
            //{
                #region
                switch (code)
                {
                    case "0": return "CR_NO_FUNC_RUNNING";//空闲
                    case "16": return "CR_POS_GET_WAIT_H";//等待取货
                    case "17": return "CR_POS_GET_RUNNING";//取货运行中
                    case "25": return "CR_POS_GET_EXECUTED";//取货完成
                    case "26": return "CR_POS_GET_FAILED";//取货失败
                    case "27": return "CR_POS_GET_ABORTED";//取货中断
                    case "32": return "CR_GET_WAIT_H";
                    case "33": return "CR_GET_RUNNING";
                    case "41": return "CR_GET_EXECUTED";
                    case "42": return "CR_GET_FAILED";
                    case "43": return "CR_GET_ABORTED";
                    case "44": return "CR_GET_MF_FAULT";
                    case "45": return "CR_GET_LOC_EMPTY";
                    case "46": return "CR_GET_PNOTREACH";
                    case "48": return "CR_POS_PUT_WAIT_H";
                    case "49": return "CR_POS_PUT_RUNNING";
                    case "57": return "CR_POS_PUT_EXECUTED";
                    case "58": return "CR_POS_PUT_FAILED";
                    case "59": return "CR_POS_PUT_ABORTED";
                    case "64": return "CR_PUT_WAIT_H";
                    case "65": return "CR_PUT_RUNNING";
                    case "73": return "CR_PUT_EXECUTED";
                    case "74": return "CR_PUT_FAILED";
                    case "75": return "CR_PUT_ABORTED";
                    case "76": return "CR_PUT_MF_FAULT";
                    case "77": return "CR_PUT_LOC_FULL";
                    case "78": return "CR_PUT_PNOTREACH";
                    case "80": return "CR_POS_PICK_WAIT_H";
                    case "81": return "CR_POS_PICK_RUNNING";
                    case "89": return "CR_POS_PICK_EXECUTED";
                    case "90": return "CR_POS_PICK_FAILED";
                    case "91": return "CR_POS_PICK_ABORTED";
                    case "112": return "CR_GETC_WAIT_H";//等待取货
                    case "113": return "CR_GETC_POS_RUNNING";//取货运行中
                    case "114": return "CR_GETC_GET_RUNNING";//取货伸叉
                    case "121": return "CR_GETC_EXECUTED";//取货完成
                    case "122": return "CR_GETC_FAILED";//取货失败
                    case "123": return "CR_GETC_ABORTED";//取货中止
                    case "124": return "CR_GETC_MF_FAULT";//
                    case "125": return "CR_GETC_LOC_EMPTY";
                    case "126": return "CR_GETC_PNOTREACH";
                    case "128": return "CR_PUTC_WAIT_H";
                    case "129": return "CR_PUTC_POS_RUNNING";
                    case "130": return "CR_PUTC_PUT_RUNNING";
                    case "137": return "CR_PUTC_EXECUTED";
                    case "138": return "CR_PUTC_FAILED";
                    case "139": return "CR_PUTC_ABORTED";
                    case "140": return "CR_PUTC_MF_FAULT";
                    case "141": return "CR_PUTC_LOC_FULL";
                    case "142": return "CR_PUTC_PNOTREACH";
                    default: return "";
                }
                #endregion
            //}
        }

        public bool WriteStationInfo(int station, string value, int index)
        {
            return ConveryClass.WriteStationInfo(station, value, index);
        }
    }
}
