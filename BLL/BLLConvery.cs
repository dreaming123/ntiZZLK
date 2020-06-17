using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOG;
using DataAccess;
using COMResolution;
using System.Data;
using System.Data.OracleClient;
using LEDControl_UDP;
using System.Threading;
using System.ServiceModel;
using ComputeClient;
using ComputeService;
using BLL;
using MOControl;
using RFID;
using System.Timers;
using COMRW;
namespace BLL
{
    public class BLLConvery : BLLBase
    {
        public delegate void GetEquipment_Inquery(List<CRLStationInfo> stationinfo, string msg, string moinfo, string time, int pos);
        public event GetEquipment_Inquery GetEquipMentInfo;
        public delegate void Compute_Inquery(string msg);
        public event Compute_Inquery ComputeInquery;

        private Dictionary<string, string> LedInfo = new Dictionary<string, string>();
        BLLLED led = new BLLLED();
        private string moinfo = "";
        CComputeClient cc = new CComputeClient();
        Dictionary<int, LED_UDP> DLed = new Dictionary<int, LED_UDP>();
        Dictionary<int, string> DLedInfo = new Dictionary<int, string>();
        Dictionary<int, int> DledWaitCount = new Dictionary<int, int>();
        Dictionary<int, int> DStvWaitCount = new Dictionary<int, int>();
        List<CRLStationInfo> StvStations = new List<CRLStationInfo>();
        public string LEDString = System.Configuration.ConfigurationSettings.AppSettings["LEDShowInfo"].ToString();
       
        public int[] LED_flag = new int[] { 1, 1, 1, 1};
        public bool flag = true;
        public void Run()
        {
            ConveryClass.GetEquipMentInfo += new CRLConveryControl.GetEquipment_Inquery(ConveryClass_GetEquipMentInfo);
            //CComputeService.eNotify += new CComputeService.Notify(CComputeService_eNotify);
            DLed.Add(1272, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED1A1"].ToString(), 28123, 1272));
            DLed.Add(1251, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED1A2"].ToString(), 28123, 1251));
            DLed.Add(1205, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED2A1"].ToString(), 28123, 1205));
            DLed.Add(1172, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED2A2"].ToString(), 28123, 1172));
            DLed.Add(1009, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED3A1"].ToString(), 28123, 1009));
            DLed.Add(1103, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED3A2"].ToString(), 28123, 1103));
            DLed.Add(1084, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED4A1"].ToString(), 28123, 1084));
            DLed.Add(1001, new LED_UDP(System.Configuration.ConfigurationSettings.AppSettings["LED4A2"].ToString(), 28123, 1001));
            if (ConveryClass.InitOPCData(dal.GetOPCItem()))
            {
                ConveryClass.Run();

                LEDThread = new Thread(new ThreadStart(TimeScan));
                LEDThread.IsBackground = true;
                LEDThread.Start();         
            }
            else
                logsta.WriteLog("加载数据库OPC数据失败！");           
        }

        //private void CComputeService_eNotify(int type, string robotlineID, object ob)
        //{
        //    int[] value = (int[])ob;
        //    string[] set = new string[11];
        //    for (int i = 2; i < 11; i++)
        //    {
        //        set[i] = "0";
        //    }

        //    switch (type)
        //    {
        //        case 1:
        //        case 2:
        //            set[0] = value[0].ToString();
        //            set[5] = value[0].ToString();
        //            set[6] = value[1].ToString();
        //            break;
        //        case 3:
        //            set[0] = value[1].ToString();
        //            set[5] = value[1].ToString();
        //            set[6] = value[0].ToString();
        //            break;
        //    }
        //    string[] info = ReadStationInfo(int.Parse(set[0]));
        //    if (!System.String.IsNullOrEmpty(info[1]))
        //    {
        //        for (int i = 1; i < 11; i++)
        //            set[i] = info[i];
        //    }
        //    WriteStationInfo(set);
        //}
        //private void ComputeInter(int type, string sortingLineID, string itemBarcode)
        //{
        //    string info;
        //    switch (type)
        //    {
        //        case 1: info = cc.RobotLineContainerIn(sortingLineID, itemBarcode); break;
        //        case 2: info = cc.RobotLinePickingContainerOut(sortingLineID); break;
        //        case 3: info = cc.RobotPickingFinished(sortingLineID); break;
        //        case 4: info = cc.SortingLineItemBarcodeChecking(sortingLineID, itemBarcode); break;
        //        case 5: info = cc.DockItemReceived(sortingLineID, itemBarcode); break;
        //        default: info = ""; break;
        //    }
        //    if (!System.String.IsNullOrEmpty(info))
        //        logsta.WriteLog(info);
        //    if (ComputeInquery != null)
        //        ComputeInquery(info);
        //}
      
        private void ConveryClass_GetEquipMentInfo(List<CRLStationInfo> stationinfo, string time)
        {

            string msg = "";
            //string isBCtoA = dal.GetIsBCtoA();//System.Configuration.ConfigurationSettings.AppSettings["IsBCtoA"];
            //if (flag)//PLC未连上时临时用,正式使用时改为 flag
            //{
            foreach (CRLStationInfo si in stationinfo)
            {
                if (si.stationno == si.tostation && si.taskno > 0)
                {
                    int StaNo = si.stationno;
                    string ledstr = "";
                    string[] info;
                    string[] str = new string[11];
                    string[] strstv = new string[11];
                    string itemcode = "", containerbarcode = Global.TxtBarcode, strMBarcode = "", strBarcode = "";//si.barcode1.ToString().PadLeft(8, '0');
                    if (si.checkinfo == 0)
                    {
                        //普通申请
                        msg = GetTask(si.taskno.ToString(), si.barcode1.ToString(), si.goodstype.ToString(), si.tostation.ToString(), si.tostation.ToString(), si.checkinfo.ToString(), si.field1.ToString(), si.weight2.ToString(), si.weight1.ToString(), itemcode);//, isBCtoA);//Global.IsSequence);
                    }
                    else
                    {
                        //外检和称重故障
                        ledstr = ErrorCode(si.checkinfo.ToString());                            
                    }
                    #region 出库穿梭车接货站台
                    if (si.config.Contains("NoReq"))
                    {
                        string[] stvinfo = ReadStationInfo(5182);
                        DataSet ds_outlaneway = dal.SelectOutLaneway(si.taskno.ToString());
                        if (ds_outlaneway != null && ds_outlaneway.Tables.Count > 0 && ds_outlaneway.Tables[0].Rows.Count > 0)
                        {
                            string desstation = si.stationno.ToString();
                            foreach (DataRow dr in ds_outlaneway.Tables[0].Rows)
                            {
                                string docktaskno = ReadStationInfo(int.Parse(dr["field5"].ToString()))[1]; //拆垛位
                                string buffertaskno = ReadStationInfo(int.Parse(dr["field4"].ToString()))[1];//缓存位
                                if (buffertaskno == "0")
                                {
                                    if (stvinfo[1] == "0" && docktaskno == "0")//去拆垛位
                                    {
                                        desstation = dr["field5"].ToString();
                                        break;
                                    }
                                    if (stvinfo[1].Length == 5 && stvinfo[5] != dr["field5"].ToString() && docktaskno == "0")
                                    {
                                        desstation = dr["field5"].ToString();
                                        break;
                                    }
                                    if (docktaskno.Length == 5 && stvinfo[1] == "0")//去缓存位
                                    {
                                        desstation = dr["field4"].ToString();
                                        break;
                                    }
                                    if (docktaskno.Length == 5 && stvinfo[1].Length == 5 && stvinfo[5] != dr["field4"].ToString())
                                    {
                                        desstation = dr["field4"].ToString();
                                        break;
                                    }
                                    if (docktaskno == "0" && stvinfo[1].Length == 5 && stvinfo[5] == dr["field5"].ToString())
                                    {
                                        desstation = dr["field4"].ToString();
                                        break;
                                    }
                                }
                            }
                            msg = si.stationno + "&" + si.taskno + "&0&4&0&" + desstation + "&0&0&0&0&0&0";
                        }
                    }
                    #endregion
                    #region 一号工程扫码申请
                    if (si.config.Contains("RFID"))
                    {
                        char[] cr = System.Text.ASCIIEncoding.ASCII.GetChars(ReadStationInfoBy51(si.stationno));
                        for (int j = 0; j < cr.Length; j++)
                            strMBarcode = strMBarcode + cr[j].ToString();
                        if (si.stationno == 5028)
                            itemcode = strMBarcode;
                        msg = GetTask(si.taskno.ToString(), strMBarcode.Trim('\0'), "3", si.tostation.ToString(), si.tostation.ToString(), si.checkinfo.ToString(), si.field1.ToString(), si.weight2.ToString(), si.weight1.ToString(), itemcode);//, isBCtoA);//Global.IsSequence);
                    }
                    #endregion
                       
                    if (!System.String.IsNullOrEmpty(msg) && msg.Length > 10)
                    {
                        info = msg.Split(new char[] { '&' });
                        str[0] = info[0];
                        str[1] = info[1];
                        int upbarcode1 = int.Parse(info[2]) / 65536;
                        int upbarcode2 = int.Parse(info[2]) % 65536;
                        str[2] = upbarcode1.ToString();
                        str[3] = upbarcode2.ToString();
                        str[4] = info[3];
                        str[5] = info[4];
                        str[6] = info[5];
                        str[7] = info[9];
                        str[8] = info[8];
                        str[9] = info[6];
                        str[10] = info[7];
                        WriteStationInfo(str);
                        ledstr = info[10]; 
                    }
                    if (si.config.Contains("LED"))
                    {
                        //DLedInfo.Add(GetLedKey(si.stationno), ledstr);
                        led.send(DLed[GetLedKey(si.stationno)], ledstr.Trim());
                    }
                }
            }
            if (GetEquipMentInfo != null)
                GetEquipMentInfo(stationinfo, msg, moinfo, time, 1);
        }
        //拆垛道口放行 DB556
        //public void GetOutLanewayGono()
        //{
        //    int taskno, sendcount, pickcount, lanewayno, pickstate;
        //    string resequenceold = "",resequence = "", itemid = "", requestqty = "", targettext = "", state = "", field4="";
        //    byte[] byt = new byte[10];
        //    DataSet ds_outrequestB = dal.QueryOutrequest("B类烟");
        //    if (ds_outrequestB != null && ds_outrequestB.Tables.Count > 0 && ds_outrequestB.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ds_outrequestB.Tables[0].Rows)
        //        {
        //            //resequence = dr["requestsequence"].ToString();
        //            resequence = dr["field3"].ToString();
        //            itemid = dr["itemid"].ToString();
        //            requestqty = dr["requestqty"].ToString();
        //            targettext = dr["targetaddresstext"].ToString();
        //            state = dr["state"].ToString();
        //            if (dal.QueryItemCatalog(itemid) != "A类烟")// || state=="New")
        //            {
        //                int n = dal.QueryNotFlowCount();
        //                if (dal.QueryNotFlowCount() < MiniLoadOutBuffer)
        //                {
        //                    //for (int i = int.Parse(requestqty); i > 0; i--)
        //                    //{
        //                    if (dal.CallOutTask(itemid, "5162", resequence) < 1)
        //                        break;
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //    DataSet ds_outrequestA = dal.QueryOutrequest("A类烟");
        //    if (ds_outrequestA != null && ds_outrequestA.Tables.Count > 0 && ds_outrequestA.Tables[0].Rows.Count > 0)
        //    {                
        //        foreach (DataRow dr in ds_outrequestA.Tables[0].Rows)
        //        {
        //            resequenceold = dr["requestsequence"].ToString();
        //            resequence = dr["field3"].ToString();
        //            itemid = dr["itemid"].ToString();
        //            requestqty = dr["requestqty"].ToString();
        //            targettext = dr["targetaddresstext"].ToString();
        //            state = dr["state"].ToString();
        //            field4 = dr["field4"].ToString();
        //                //if (dal.QueryExcuNopro(targettext) == 0)//查询此目的地执行中且处理量为0的数量
        //                if (dal.QuerySeqMin() == resequence)//A类烟 在outrequest中比对顺序号是否最小
        //                {
        //                    string subseqno=resequence.Substring(resequence.Length - 4, 4).ToString();
        //                    if(subseqno=="0000")
        //                        subseqno="12345";
        //                    DataSet ds_lanewaybyitem = dal.QueryLanewayStateByItemid(itemid);
        //                    if (ds_lanewaybyitem != null && ds_lanewaybyitem.Tables.Count > 0 && ds_lanewaybyitem.Tables[0].Rows.Count > 0)
        //                    {
        //                        foreach (DataRow drw in ds_lanewaybyitem.Tables[0].Rows)
        //                        {
        //                            bool flag = false;
        //                            int pcdock = Global.sendflag;
        //                            int Lanewayend = int.Parse(drw["field5"].ToString());                                    
        //                            string wcspoint = drw["wcseqposition"].ToString();
        //                            //string RobotStation = ReadStationInfo(Lanewayend)[1].ToString();
        //                            string[] RobotStation = ReadStationInfo(Lanewayend);
        //                            if (Lanewayend == 5089)
        //                                Lanewayend = 5095;
        //                            if (Lanewayend == 5088)
        //                                Lanewayend = 5104;
        //                            if (Lanewayend == 5087)
        //                                Lanewayend = 5105;
        //                            if (Lanewayend == 5086)
        //                                Lanewayend = 5114;
        //                            if (Lanewayend == 5084)
        //                                Lanewayend = 5115;
        //                            if (Lanewayend == 5083)
        //                                Lanewayend = 5124;
        //                            if (Lanewayend == 5082)
        //                                Lanewayend = 5125;
        //                            if (Lanewayend == 5081)
        //                                Lanewayend = 5134;
        //                            ////if (Lanewayend == 5090)
        //                            ////    Lanewayend = 5294;
        //                            if (itemid == "2dcd472779d74373a01ca73abbc1846f" && (RobotStation[1] == "0" || RobotStation[5] == "5091"))
        //                            {
        //                                if (ReadStationInfo(Lanewayend)[1].ToString() == "0" && ReadStationInfo(5113)[1].ToString() == "0" && ReadStationInfo(5312)[1].ToString() == "0" && ReadStationInfo(5111)[1].ToString() == "0" && ReadStationInfo(5112)[1].ToString() == "0" && ReadStationInfo(5294)[1].ToString() != "0")
        //                                {
        //                                    ////核对放行，直接放行注释
        //                                    //pcdock = 1;
        //                                    //直接放行,核对注释
        //                                    wcspoint = "0";
        //                                    Lanewayend = 5294;
        //                                }
        //                            }
        //                            ////核对放行，直接放行注释
        //                            //if (pcdock == 1)
        //                            //    wcspoint = "0";
        //                            byte[] OutLaneway = ReadStationInfoBy556(int.Parse(wcspoint));
        //                            taskno = OutLaneway[0] * 256 + OutLaneway[1];    //任务号或顺序号
        //                            sendcount = OutLaneway[2] * 256 + OutLaneway[3];
        //                            pickcount = OutLaneway[4] * 256 + OutLaneway[5];
        //                            lanewayno = OutLaneway[6] * 256 + OutLaneway[7];
        //                            pickstate = OutLaneway[8] * 256 + OutLaneway[9];
                                    
                                   
        //                            if (ReadStationInfo(Lanewayend)[1].ToString() == "0" &&(RobotStation[1] == "0" || RobotStation[5] == "5091"))//&& RobotStation[1] == "0")
        //                            {
        //                                break;
        //                            }
        //                            ////核对放行，直接放行注释
        //                            //if (pickstate == 9 || (int.Parse(subseqno)) != taskno)
        //                            //{
        //                            //    flag = true;
        //                            //}
        //                            ////核对放行，直接放行注释
        //                            //if ((int.Parse(subseqno)) == taskno)
        //                            //{
        //                            //    dal.UpdatePickOutCount(int.Parse(requestqty), itemid);
        //                            //    if (dal.UpdateOutReqExecuting(resequenceold.ToString()))
        //                            //    {
        //                            //        flag = false;
        //                            //        if (pcdock == 1)
        //                            //            pcdock = 0;
        //                            //    }
                                        
        //                            //}
        //                            //if (flag && (pickstate == 9 || pickstate == 3))//核对放行，直接放行注释
        //                            if (field4.Trim().Length==0 && (pickstate == 9 || pickstate == 3))//下发放行  直接放行,核对注释
        //                            {
        //                                string[] str = new string[4];
        //                                str[0] = (int.Parse(wcspoint) + 1).ToString();
        //                                str[1] = subseqno;//resequence.Substring(resequence.Length - 4, 4);
        //                                str[2] = requestqty;
        //                                str[3] = "1";
        //                                if (WriteStationDB554(str))
        //                                {
        //                                    dal.UpdatePickOutCount(int.Parse(requestqty), itemid);  //直接放行,核对注释
        //                                    dal.UpdateOutReqExecuting(resequenceold.ToString(),subseqno);    //直接放行,核对注释
        //                                    ////dal.UpdateOutReqTime(resequenceold.ToString());
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }  
        //            }                   
        //        }
        //    Thread.Sleep(300); 
        //}
        //空托盘组出库申请
        public void GetOutBoundTask()
        {
            string[] info_5047 = ReadStationInfo(5047);
            int QtyPalletTask = 0;
            try
            {
                QtyPalletTask = dal.YS_QueryEmptyPalletTask("5047");
                string[] state = dal.QuerySCState().Split(',');
                if (info_5047[1] == "0" && QtyPalletTask == 0 && (state[0] == "Equipment_OK" || state[1] == "Equipment_OK"))
                {
                    dal.CallPalletOut("5047");
                }
            }
            catch(Exception ex)
            {
                logsta.WriteLog("站台申请出库任务异常:"+ex.Message);
                ComputeInquery("站台申请出库任务异常:"+ex.Message);
            }
        }
        public int GetLedKey(int stationno)
        {
            int LedKey=stationno;
            if (stationno == 1005)
                LedKey = 1001;
            if (stationno == 1013)
                LedKey = 1009;
            if (stationno == 1209)
                LedKey = 1205;
            if (stationno == 1276)
                LedKey = 1272;
            return LedKey;
        }
        public bool test(int station,string value)
        {
            return WriteStationInfo(station, value, 8);
        }
        //将信息由10进制转化为2进制,解析异常处理
        public string ErrorCode(string CheckInfo)
        {
            Int32 checkinfo = Int32.Parse(CheckInfo.ToString());
            string errorcode = Convert.ToString(checkinfo, 2).PadLeft(16, '0');
            // errorcode = errorcode.Substring(8, 8) + errorcode.Substring(0, 8);
            char[] ErrorCode = errorcode.ToCharArray();
            string LEDStr = "故障:";
            for (int j = 0; j < 16; j++)
            {
                if (ErrorCode[j] == '1')
                {
                    switch (j)
                    {
                        //case 0: LEDStr = LEDStr + "外形检测故障: ";
                        //    break;
                        //case 1: LEDStr = LEDStr + "超高";
                        //    break;
                        case 2: LEDStr = LEDStr + "右超宽";
                            break;
                        case 3: LEDStr = LEDStr + "左超宽";
                            break;
                        case 4: LEDStr = LEDStr + "后超长";
                            break;
                        case 5: LEDStr = LEDStr + "前超长";
                            break;
                        case 6: LEDStr = LEDStr + "超高";
                            break;
                        //case 7: LEDStr = LEDStr + "未知";
                        //    break;                       
                        case 11: LEDStr = LEDStr + "超出允许偏差值";
                            break;
                        case 12: LEDStr = LEDStr + "超重";
                            break;
                        case 13: LEDStr = LEDStr + "重量未读出";
                            break;                        
                        case 14: LEDStr = LEDStr + "条码未读出";
                            break;
                        case 15: LEDStr = LEDStr + "条码读取错误";
                            break;
                    }
                }
                if (ErrorCode[7] == '0')
                {
                    LEDStr = "";
                }
            }
            return LEDStr;
        }
        
        private void TimeScan()
        {
            int[] LEDCode = new int[] { 1272, 1251, 1205, 1172, 1009, 1103, 1084, 1001 };
            while (true)
            {
                foreach (var dd in LEDCode)
                {
                    LEDString = "设备运行中,请注意安全！ 日期:" + DateTime.Now.ToString("yyyy-MM-dd");
                    led.send(DLed[dd], LEDString.Trim());                    
                }
                System.Threading.Thread.Sleep(90000);
            }
        }
    }
}
