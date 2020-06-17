using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using LOG;

namespace GenSong
{
    public class CRNTask : CRNExcuteBase
    {
        private LOG.Log log = new Log("堆垛机任务", ".\\堆垛机任务日志\\");
        public delegate void NotifySRMTask(string type, string msg);
        public static event NotifySRMTask ShowSRMTaskInfo;
       
        //CRLConveryControl bll = new CRLConveryControl();
        /// <summary>
        /// 更新堆垛机状态
        /// </summary>
        /// <param name="num">堆垛机编号</param>
        /// <param name="str">交互信息</param>
        /// <param name="aisle">巷道</param>
        public string UpdateSCStatus(string num, string str, string aisle,string zone)
        {
            if (str.Length == 43)
            {
                num = "CRN00" + (int.Parse(str.Substring(41, 1)) + 2);
                aisle = str.Substring(41, 1);
            }
            if (str.Length == 17)
            {
                num = "CRN00" + (int.Parse(str.Substring(15, 1)) + 2);
                aisle = str.Substring(15, 1);
            }
            
            CRNStatus css = CRNList.Find(
                delegate(CRNStatus info)
                {
                    return info.CRCNum == num && info.Zone == zone;
                }
            );
            if (css == null)
            {
                CRNStatus status = new CRNStatus();
                status.CRCNum = num;
                //status.AisleNumber = aisle;
                status.Zone = zone;
                status.CraneMode = "MCT已连接";
                status.CrcStatus = "Socket已连接！";
                status.Position = "0000";
                status.RackSide = "0";
                status.RackLevel = "0";
                status.FunctionReport = "112";
                status.AlarmMessage = "无故障";
                CRNList.Add(status);
                return "";
            }
            string alarmMsg = "";
            #region 关闭模式
            if (str == "Break")
            {
                css.CraneMode = "MCT断开!";
                css.CrcStatus = "Socket断开！";
                css.Position = "0000";
                css.RackSide = "0";
                css.RackLevel = "0";
                css.FunctionReport = "112";
                css.AlarmMessage = "Socket断开！";
                alarmMsg = css.AlarmMessage;
                css.LabelInfo = "任务号:" + css.TaskNo + ".位置:" + css.Position + ".是否在站点位置:否" + ".状态:" + alarmMsg + ".事件类型:无" + ".起止:" + css.Fromstation + "-->" + css.Tostation + ".无故障";
                dal.UpdateSCStatus("SC" + (int.Parse(num.Substring(5, 1))+2).ToString(), "Equipment_Error");
            }
            #endregion
            #region 连接
            if (str == "Connect")
            {
                css.CraneMode = "MCT已连接";
                css.CrcStatus = "Socket已连接！";
                css.Position = "0000";
                css.RackSide = "0";
                css.RackLevel = "0";
                css.FunctionReport = "112";
                css.AlarmMessage = "无故障";
                css.LabelInfo = "任务号:" + css.TaskNo + ".位置:" + css.Position + ".是否在站点位置:否" + ".MCT模式:" + css.CraneMode + ".状态:" + css.CrcStatus + ".事件类型:无" + ".货叉1起止:" + css.Fromstation + "-->" + css.Tostation + ".货叉2起止:" + css.SecFromstation + "-->" + css.SecTostation + ".无故障";
                if (css.CrcStatus == "Equipment_OK")
                {
                    dal.UpdateSCStatus(num, "Equipment_OK");
                }
            }
            #endregion
            #region 返回堆垛机状态信息
            else if (str.Substring(1, 2) == "LA")
            {
                //css.CraneMode = GetGenSongMode(str.Substring(3, 2));
                css.CrcStatus = GetGenSongStatus(str.Substring(3, 2));
                if (str.Substring(3, 2) == "02")
                    css.CraneMode = "可调度";
                else
                    css.CraneMode = "不可调度";
                css.Position = GetGenSongPosition(str.Substring(5, 6)).Replace("?", "0");
                css.RackPosition = str.Substring(5, 3).Replace("?", "0");
                css.RackLevel = str.Substring(8, 3).Replace("?", "0");
                string stationPosistion = str.Substring(12, 1);
                css.AlarmMessage = "无故障";
                foreach (DictionaryEntry de in AlarmHash)
                {
                    if (de.Key.ToString() == str.Substring(13, 4))
                    {
                        css.AlarmMessage = de.Value.ToString();
                    }
                }
                string eventType = GetEventType(str.Substring(17, 1));
                int etype = int.Parse(str.Substring(17, 1));
                css.TaskNo = str.Substring(30, 10).Replace("GOT", "000");
                css.RackSide = str.Substring(26, 1);
                css.RackStatus = str.Substring(28, 1);    //货叉1状态 1无货，2有货，3故障
                css.SecRackStatus = str.Substring(29, 1); //货叉2状态 1无货，2有货，3故障
                int number = int.Parse(str.Substring(40, 2));
                if (etype < 2)
                    css.FunctionReport = CR_GETC_WAIT_H;
                else if (etype == 3)
                    css.FunctionReport = CR_GETC_EXECUTED;
                else if (etype == 4)
                    css.FunctionReport = CR_PUTC_PUT_RUNNING;
                else if (etype == 5)
                    css.FunctionReport = CR_PUTC_EXECUTED;
                else
                    css.FunctionReport = CR_GETC_WAIT_H;
                css.LabelInfo = "任务号:" + css.TaskNo + ".位置:" + css.Position + ".是否在站点位置:" + (stationPosistion == "1" ? "是" : "否") + ".MCT模式:" + css.CraneMode + ".状态:" + css.CrcStatus + ".事件类型:" + eventType + ".货叉1起止:" + css.Fromstation + "-->" + css.Tostation + ".货叉2起止:" + css.SecFromstation + "-->" + css.SecTostation + "." + css.AlarmMessage;
            }
            #endregion
            #region 返回堆垛机动作信息
            else if (str.Substring(1, 2) == "LD")
            {
                string scno = str.Substring(14, 2);
                string taskStatus = str.Substring(3, 1);
                string taskno1 = str.Substring(4, 5);
                string taskno2 = str.Substring(9, 5);//.TrimStart('0');
                string[] taskinfo = dal.GetTostation(taskno1);
                string[] sectaskinfo = dal.GetTostation(taskno2);              
                string[] stainfo = null;
                string[] secstainfo = null;
                if (taskStatus == "1")//取货完成
                {                    
                    if (taskinfo != null && taskinfo[0] !=null && taskinfo[0].Length == 4)
                    {
                        dal.UpdateMiniGetState(taskno1);
                        stainfo = new string[11];// ConveryClass.ReadStationInfo(int.Parse(taskinfo[0]));
                        stainfo[0] = taskinfo[0];
                        for (int i = 1; i < 11; i++)
                            stainfo[i] = "0";
                        ConveryClass.WriteStationInfo(stainfo);
                    }
                    if (sectaskinfo != null && sectaskinfo[0] != null && sectaskinfo[0].Length == 4)
                    {
                        dal.UpdateMiniGetState(taskno2);
                        secstainfo = new string[11];// ConveryClass.ReadStationInfo(int.Parse(taskinfo[0]));
                        secstainfo[0] = sectaskinfo[0]; 
                        for (int i = 1; i < 11; i++)
                            secstainfo[i] = "0";
                        if (secstainfo[0] == "5211")
                            secstainfo[0] = "5011";
                        if (secstainfo[0] == "5212")
                            secstainfo[0] = "5012";
                        if (secstainfo[0] == "5215")
                            secstainfo[0] = "5015";
                        ConveryClass.WriteStationInfo(secstainfo);
                    }
                    alarmMsg += num + "堆垛机取货完成：" + css.TaskNo;
                }
                else if (taskStatus == "2" || taskStatus == "4")//堆垛机放货完成
                {
                    #region 堆垛机放货完成
                    if (taskinfo != null && taskinfo[0] != null)
                    {
                        if (taskinfo[1].Length == 4)//放货到输送机
                        {
                            stainfo = ConveryClass.ReadStationInfo(int.Parse(taskinfo[1]));
                            if (int.Parse(stainfo[1]) > 0)
                            {
                                alarmMsg += taskinfo[1] + "站台有货物！";
                            }
                            else
                            {
                                stainfo[1] = taskno1;
                                stainfo[2] = "0";
                                stainfo[3] = "0";
                                stainfo[4] = "3";//类型 件烟
                                stainfo[5] = taskinfo[1];// "0"; //起始站台 taskinfo[1];//
                                stainfo[6] = "5156";//5156或0
                                stainfo[7] = "0";
                                stainfo[8] = "0";
                                stainfo[9] = "0";
                                stainfo[10] = "0";
                                if (scno == "01")
                                {
                                    stainfo[6]="5353";
                                }
                                else if (scno == "02")
                                { 
                                    stainfo[6]="5343";
                                }
                                else if (scno == "03")
                                {
                                    stainfo[6] = "5342";
                                }
                                ConveryClass.WriteStationInfo(stainfo);

                                if (dal.UpdateTaskStatus(taskno1, "SC", "1", "1") != "0")//堆垛机完成任务
                                {
                                    dal.UpdateTaskStatus(taskno1, "STA", "2", "1");
                                }
                            }
                        }
                        else
                            dal.UpdateTaskStatus(taskno1, "SC", "1", "1");//堆垛机完成任务
                    }
                    if (sectaskinfo != null && sectaskinfo[0] != null)
                    {
                        if (sectaskinfo[1].Length == 4)//放货到输送机
                        {
                            secstainfo = ConveryClass.ReadStationInfo(int.Parse(sectaskinfo[1])+200);
                            if (int.Parse(secstainfo[1]) > 0)
                            {
                                alarmMsg += sectaskinfo[1] + "站台有货物！";
                            }
                            else
                            {
                                secstainfo[1] = taskno2;
                                secstainfo[2] = "0";
                                secstainfo[3] = "0";
                                secstainfo[4] = "3";//类型 件烟
                                secstainfo[5] = (int.Parse(sectaskinfo[1]) + 200).ToString();// "0"; 起始站台
                                secstainfo[6] = "5156";//5156或0
                                secstainfo[7] = "0";
                                secstainfo[8] = "0";
                                secstainfo[9] = "0";
                                secstainfo[10] = "0";
                                if (scno == "01")
                                {
                                    secstainfo[6] = "5353";
                                }
                                else if (scno == "02")
                                {
                                    secstainfo[6] = "5343";
                                }
                                else if (scno == "03")
                                {
                                    secstainfo[6] = "5342";
                                }
                                ConveryClass.WriteStationInfo(secstainfo);
                                if (dal.UpdateTaskStatus(taskno2, "SC", "1", "1") != "0")//堆垛机完成任务
                                {
                                    dal.UpdateTaskStatus(taskno2, "STA", "2", "1");
                                }
                            }
                        }
                        else
                            dal.UpdateTaskStatus(taskno2, "SC", "1", "1");//堆垛机完成任务
                    }
                    #endregion
                    if (taskinfo[1].Length == 4 && (css.CrcStatus == "无货待命" || css.CrcStatus == "无货运行" || css.CrcStatus == "有货运行" || css.CrcStatus == "有货待命"))
                    {
                        DataSet ds = dal.GetGSInTask(scno);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                CRNObject co = CRNOBList.Find(
                                    delegate(CRNObject info)
                                    {
                                        return (info.ScNo == "CRN00" + row["equipmentnumber"].ToString().Substring(5, 1) && info.zone == zone);
                                    }
                                );
                                if (co != null)
                                {
                                    //((GenSongSRM)co.BllSrm).SendTask(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString());
                                    ((GenSongSRM)co.BllSrm).SendTask(row["taskno"].ToString(), row["fromstation"].ToString(), row["tostation"].ToString(), row["sectaskno"].ToString(), row["secfromstation"].ToString(), row["sectostation"].ToString(), row["currentstation"].ToString(), row["field2"].ToString());
                                }
                            }
                        }
                    }
                }
                else if (taskStatus == "3")
                {
                    alarmMsg += "任务被取消，任务号：" + taskno1+taskno2;
                }
            }
            #endregion
            //if (css.LabelInfo == css.OldLabelInfo)
            //    css.Change = false;
            //else
            //{
                if (str.Substring(1, 2) == "LA")
                {
                    //if (str.Substring(15, 4) != "0000")
                    string test = str.Substring(13, 4);

                    if (str.Substring(13, 4) != "0000")
                    {
                        //dal.UpdateSCStatus(num, "Equipment_Error");//"SC" + int.Parse(num.Substring(5, 3)).ToString()
                        dal.UpdateSCStatus("SC" + int.Parse(num.Substring(5, 1)).ToString(), "Equipment_Error");
                        alarmMsg = css.AlarmMessage;
                    }
                    //else
                    //{
                    //    dal.UpdateSCStatus("SC" + int.Parse(num.Substring(5, 1)).ToString(), "Equipment_OK");
                    //}
                    //dal.UpdateSCStatus(num, "Equipment_OK");
                    if (str.Substring(3, 2) == "11" || str.Substring(3, 2) == "12")
                    {
                        dal.UpdateSCStatus("SC" + int.Parse(num.Substring(5, 1)).ToString(), "Equipment_Error");
                        alarmMsg = "未连接或手动状态";
                    }
                }
                css.OldLabelInfo = css.LabelInfo;
                css.Change = true;
            //}
            return alarmMsg;
        }
        /// <summary>
        /// 获取堆垛机状态
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string[] GetCRCStatus(string num,string zone)
        {
            string[] ret = new string[14] { "", "", "0", "", "", "", "0", "0", "0", "", "0", "0", "0", "0" };
            CRNStatus css = CRNList.Find(
                delegate(CRNStatus info)
                {
                    return info.CRCNum == num && info.Zone == zone;
                }
            );
            if (css != null)
            {
                ret[0] = css.Position;
                ret[1] = css.FunctionReport;
                ret[2] = css.FunctionMode;
                ret[3] = css.TaskNo;
                ret[6] = css.AisleNumber;
                ret[7] = css.RackPosition;
                ret[8] = css.RackPlace;
                ret[9] = css.LabelInfo;
                if (!System.String.IsNullOrEmpty(css.TaskNo) && int.Parse(css.TaskNo.Substring(0,5)) > 0)
                {
                    if (System.String.IsNullOrEmpty(css.Fromstation) || System.String.IsNullOrEmpty(css.Tostation))
                    {
                        string[] ss = new string[2];
                        //ss = dal.GetTostation(css.TaskNo, "SC", "2");
                        if (ss != null)
                        {
                            css.Fromstation = ss[0];
                            css.Tostation = ss[1];
                        }
                    }
                }
                ret[4] = css.Fromstation;
                ret[5] = css.Tostation;
                ret[10] = css.CrcTaskFlag;
                ret[11] = css.IsInitial;
                ret[12] = css.Palletnu;
                ret[13] = css.CraneMode;
            }
            return ret;
        }
        /// <summary>
        /// 更新堆垛机任务
        /// </summary>
        /// <param name="num"></param>
        /// <param name="fromstation"></param>
        /// <param name="tostation"></param>
        /// <param name="statostation"></param>
        /// <returns></returns>
        public bool UpdateCRCTaskInfo(string num, string fromstation, string tostation, string statostation, string taskno,string zone)
        {
            CRNStatus css = CRNList.Find(
                delegate(CRNStatus info)
                {
                    return info.CRCNum == num && info.Zone == zone;
                }
            );
            if (css != null)
            {
                if (!System.String.IsNullOrEmpty(fromstation))
                    css.Fromstation = fromstation;
                css.Tostation = tostation;
                css.STAToStation = statostation;
                css.TaskNo = taskno;
                css.CrcTaskFlag = "0";
                return true;
            }
            return false;
        }//+0603
        public bool UpdateCRCTaskInfo(string num, string fromstation, string tostation, string secfromstation, string sectostation, string statostation, string taskno, string sectaskno, string zone)
        {
            CRNStatus css = CRNList.Find(
                delegate(CRNStatus info)
                {
                    return info.CRCNum == num && info.Zone == zone;
                }
            );
            if (css != null)
            {
                if (!System.String.IsNullOrEmpty(fromstation))
                    css.Fromstation = fromstation;
                css.Tostation = tostation;
                css.SecFromstation = secfromstation;
                css.SecTostation = sectostation;
                css.STAToStation = statostation;
                css.TaskNo = taskno+sectaskno;
                css.CrcTaskFlag = "0";
                return true;
            }
            return false;
        }
        public bool UpdateCRCTaskInfo(string num,string zone)
        {
            CRNStatus css = CRNList.Find(
                delegate(CRNStatus info)
                {
                    return info.CRCNum == num && info.Zone == zone;
                }
            );
            if (css != null)
            {
                css.Fromstation = "";
                css.Tostation = "";
                css.SecFromstation = "";
                css.SecTostation = "";
                css.STAToStation = "";
                css.TaskNo = "0";
                css.CrcTaskFlag = "1";
                return true;
            }
            return false;
        }

        private string CheckStation(string fromstation, string tostation, string taskno, int num)
        {
            if (fromstation == "00000000" && tostation == "00000000")
                return taskno;
            //if (fromstation.Length == 10 && tostation.Length == 10)//移库
            //    return taskno;
            if (taskno.TrimStart('0').Length == 5 )
            {
                string[] stationinfo;
                //if (int.Parse(fromstation.Substring(3,3)) > 92)//入库
                if (fromstation.Length==4)//入库
                {
                    if (fromstation == "5211" && num == 2)
                        fromstation = "5011";
                    if (fromstation == "5212" && num == 2)
                        fromstation = "5012";
                    if (fromstation == "5215" && num == 2)
                        fromstation = "5015";
                    //return taskno;
                    stationinfo = ConveryClass.ReadStationInfo(int.Parse(fromstation));
                    return stationinfo[1];
                }
                else
                {
                    if (tostation == "5145" && num == 2)
                        tostation = "5345";
                    if (tostation == "5144" && num == 2)
                        tostation = "5344";
                    if (tostation == "5141" && num == 2)
                        tostation = "5341";                   
                    stationinfo = ConveryClass.ReadStationInfo(int.Parse(tostation));
                    if (stationinfo[1] == "0")
                        return taskno;
                }
            }
            return "0";
        }
       
        public DataTable GetTask(string zone)
        {
            try
            {
                string lane = "", executingsc = "", iserror="";
                int distance;
                bool flag = true;
                DataTable dt = new DataTable();

                dt.Columns.Add("num", Type.GetType("System.String"));//堆垛机号
                dt.Columns.Add("taskno", Type.GetType("System.String"));//任务号        货叉1
                dt.Columns.Add("fromstation", Type.GetType("System.String"));//开始站台 货叉1
                dt.Columns.Add("tostation", Type.GetType("System.String"));//结束站台   货叉1
                dt.Columns.Add("statostation", Type.GetType("System.String"));//目标站台货叉1
                dt.Columns.Add("palletno", Type.GetType("System.String"));//托盘号
                dt.Columns.Add("distance", Type.GetType("System.String"));//距离
                dt.Columns.Add("sectaskno", Type.GetType("System.String"));//任务号        货叉2
                dt.Columns.Add("secfromstation", Type.GetType("System.String"));//开始站台 货叉2
                dt.Columns.Add("sectostation", Type.GetType("System.String"));//结束站台   货叉2
                //dt.Columns.Add("secstatostation", Type.GetType("System.String"));//目标站台货叉2
                CRNStatus css = null;
                DataSet ds = null;
                foreach (CRNObject co in CRNOBList)
                {
                    lane = "";
                    lane += co.ScNo.Substring(co.ScNo.Length - 2, 2);
                    css = CRNList.Find(
                        delegate(CRNStatus info)
                        {
                            return info.CRCNum == co.ScNo && info.Zone == zone;
                        }
                    );
                    if (css != null)
                        //lane += (css.CrcStatus == "无货待命" && css.CraneMode == "自动") ? "1" : "0";
                        lane += (css.CrcStatus == "无货待命" || css.CrcStatus == "无货运行" || css.CrcStatus == "有货待命" || css.CrcStatus == "有货运行") ? "1" : "0";
                    else
                        lane += "0";
                    if (COMResolution.Global.outFirst)
                    { 
                        iserror = "0"; 
                    }
                    else
                    { 
                        iserror = "1"; 
                    }
                    ds = dal.GetGSSRMTask(lane, css.RackStatus, css.SecRackStatus, iserror);
                    if (ds != null && css.CrcStatus == "无货待命")
                    {
                        DataTable dts = ds.Tables[0];
                        foreach (DataRow row in dts.Rows)
                        {
                            if (row["taskstatus"].ToString() == "2")
                                executingsc += row["equipmentnumber"].ToString() + ";";
                        }
                        for (int k = 0; k < dts.Rows.Count; k++)
                        {
                            if (dts.Rows[k]["taskstatus"].ToString() == "0" && !executingsc.Contains(dts.Rows[k]["equipmentnumber"].ToString()))
                            {
                                if (CheckStation(dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["taskno"].ToString(),1) == dts.Rows[k]["taskno"].ToString() && CheckStation(dts.Rows[k]["secfromstation"].ToString(), dts.Rows[k]["sectostation"].ToString(), dts.Rows[k]["sectaskno"].ToString(),2) == dts.Rows[k]["sectaskno"].ToString())//检查当前任务是不是正在执行的任务
                                {
                                    distance = GetDistance(dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["equipmentnumber"].ToString(),zone);
                                    if (distance >= 0)
                                    {
                                        int index = dt.Rows.Count;

                                        for (int i = 0; i < index; i++)
                                        //foreach (DataRow drow in dt.Rows)
                                        {
                                            DataRow drow = dt.Rows[i];
                                            if (drow == null)
                                                continue;
                                            if (drow["num"].ToString() == dts.Rows[k]["equipmentnumber"].ToString())
                                            {
                                                flag = false;
                                                if (int.Parse(drow["distance"].ToString()) > distance)
                                                {
                                                    // dt.Rows.Remove(drow);
                                                    drow.Delete();
                                                    //dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), dts.Rows[k]["barcode"].ToString(), distance });
                                                    dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["sectaskno"].ToString(), dts.Rows[k]["secfromstation"].ToString(), dts.Rows[k]["sectostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), dts.Rows[k]["field2"].ToString(), distance });
                                                }
                                            }
                                        }
                                        dt.AcceptChanges();
                                        if (flag)
                                            dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["sectaskno"].ToString(), dts.Rows[k]["secfromstation"].ToString(), dts.Rows[k]["sectostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), dts.Rows[k]["field2"].ToString(), distance });
                                            //dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), dts.Rows[k]["barcode"].ToString(), distance });
                                        else
                                            flag = true;
                                    }
                                }
                                else
                                {
                                    log.WriteLog("********************************************");
                                    log.WriteLog("系统已经获取到了任务" + dts.Rows[k]["taskno"].ToString() + "，但堆垛机入库到位站台没有相关的任务信息！！！");
                                    log.WriteLog("********************************************");
                                }
                            }
                        }

                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.WriteLog("堆垛机获取任务异常,原因如下：" + ex.Message + ",详细信息" + ex.StackTrace);
                return null;
            }
        }
        //获取机架与货位的距离
        private int GetDistance(string fromstation, string tostation, string equipmentnumber,string zone)
        {
            CRNStatus css = CRNList.Find(
           delegate(CRNStatus info)
           {
               return info.CRCNum == "CRN00" + equipmentnumber.Substring(5, 1) && info.Zone == zone;
           }
           );
            if (css != null && css.RackPosition != null)
            {
                if (fromstation.Length == 4)
                    return 1;//int.Parse(css.RackPosition);
                else
                    return Math.Abs(int.Parse(css.RackPosition) - int.Parse(fromstation.Substring(4, 3)));
            }
            if (fromstation.Length == 8 && tostation.Length == 8)//移库
                return 1;
            return -1;
        }
        //获取目标地址
        //public string GetTargetAddr(string equipmentnumber)
        //{
        //    CRCStatus css = CRCItems.Find
        //  (
        //      delegate(CRCStatus info)
        //      {
        //          return info.CRCNum == equipmentnumber;
        //      }
        //  );
        //    if (css != null)
        //        return css.Tostation;
        //    return "";
        //}
        //将地址装换成WCS可用的地址
        public string ConvertToWCSAddress(string address, string srmnu)
        {
            string SRMAddress = string.Empty;
            if (address.IndexOf("P") != -1)
            {
                switch (address.Replace("#", "").Trim())
                {
                    case "EP011": SRMAddress = "2004"; break;
                    case "EP021": SRMAddress = "2008"; break;
                    case "EP031": SRMAddress = "2012"; break;
                    case "EP041": SRMAddress = "2004"; break;
                    case "EP051": SRMAddress = "2008"; break;
                    case "EP061": SRMAddress = "2012"; break;
                    case "AP011": SRMAddress = "2001"; break;
                    case "AP021": SRMAddress = "2005"; break;
                    case "AP031": SRMAddress = "2009"; break;
                    case "AP041": SRMAddress = "2001"; break;
                    case "AP051": SRMAddress = "2005"; break;
                    case "AP061": SRMAddress = "2009"; break;
                    case "EP012": SRMAddress = "2065"; break;
                    case "EP022": SRMAddress = "2062"; break;
                    case "EP032": SRMAddress = "2059"; break;
                    case "EP042": SRMAddress = "2065"; break;
                    case "EP052": SRMAddress = "2062"; break;
                    case "EP062": SRMAddress = "2059"; break;
                    case "AP012": SRMAddress = "2065"; break;
                    case "AP022": SRMAddress = "2062"; break;
                    case "AP032": SRMAddress = "2059"; break;
                    case "AP042": SRMAddress = "2065"; break;
                    case "AP052": SRMAddress = "2062"; break;
                    case "AP062": SRMAddress = "2059"; break;

                }
            }
            else
            {
                SRMAddress = address.Replace("#", "");
                if (SRMAddress.Length == 20)
                {
                    //06020100090001010100
                    //S0060020012
                    string strAisle = "S" + (int.Parse(SRMAddress.Substring(3, 1)) == 2 ? (int.Parse(SRMAddress.Substring(0, 2)) * 2).ToString("D3") : (int.Parse(SRMAddress.Substring(0, 2)) * 2 - 1).ToString("D3")).ToString();
                    string strWarehouseSide = "0" + Convert.ToString(2 - (Convert.ToInt32(address.Substring(1, 3)) % 2));
                    string strDepth = SRMAddress.Substring(5, 1);//2位，深度
                    string strXCoordinate = "0" + SRMAddress.Substring(8, 2);//4位，列
                    string strYCoordinate = "0" + SRMAddress.Substring(12, 2);//4位，层
                    SRMAddress = strAisle + strXCoordinate + strYCoordinate + strDepth;
                }
            }
            return SRMAddress;
        }
        public string GetGenSongStatus(string state)
        {
            string status = "";
            switch (state)
            {
                case "00": status = "初始化"; break;
                case "01": status = "回原点"; break;
                case "02": status = "无货待命"; break;
                case "03": status = "有货待命"; break;
                case "04": status = "无货运行"; break;
                case "05": status = "有货运行"; break;
                case "06": status = "取货"; break;
                case "07": status = "放货"; break;
                case "08": status = "报警停机"; break;
                case "09": status = "故障复位"; break;
                case "10": status = "未知报文"; break;
                case "11": status = "未连接"; break;
                case "12": status = "手动操作"; break;
            }
            return status;
        }
        public string GetEventType(string msg)
        {
            string type = "";
            switch (msg)
            {
                case "0": type = "初始化"; break;
                case "1": type = "运行"; break;
                case "2": type = "开始取货"; break;
                case "3": type = "取货完成"; break;
                case "4": type = "开始放货"; break;
                case "5": type = "放货完成"; break;
                case "6": type = "完成"; break;
                case "7": type = "急停"; break;
                case "8": type = "报警暂停"; break;
                case "9": type = "回原点"; break;
            }
            return type;
        }
        public string GetTaskStatus(string msg)
        {
            string status = "";
            switch (msg)
            {
                case "1": status = "接收任务正确"; break;
                case "2": status = "接收任务不正确"; break;
                case "3": status = "设备忙(正在执行任务)"; break;
            }
            return status;
        }
        public string GetGenSongMode(string state)
        {
            string status = "";
            switch (state)
            {
                case "01": status = "自动"; break;
                case "02": status = "半自动"; break;
                case "03": status = "手动操作"; break;
                case "04": status = "未连接"; break;
            }
            return status;
        }
        public string GetGenSongPosition(string subposition)
        {
            string position = "";
            //switch (side)
            //{
            //    case "1": position = "000" + subposition + "0"; break;
            //    case "2": position = "001" + subposition + "1"; break;
            //    case "3": position = "001" + subposition + "1"; break;
            //    case "4": position = "002" + subposition + "1"; break;
            //    case "5": position = "002" + subposition + "1"; break;
            //    case "6": position = "001" + subposition + "2"; break;
            //    case "7": position = "001" + subposition + "2"; break;
            //    case "8": position = "002" + subposition + "2"; break;
            //    case "9": position = "002" + subposition + "2"; break;
            //}
            position = subposition.Substring(0, 3) + "列" + subposition.Substring(3, 3) + "层";
            return position;
        }
    }
}
