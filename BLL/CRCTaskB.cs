using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using DataAccess;
using System.Data;

namespace BLL
{
    public class CRCTaskB : BLLBase
    {
        private bool writeflag = true;
        public void UpdateCRCStatus(string num, string str, string aisle)
        {
            string[] msg = str.Split(new char[] { ';' });
            CRCStatusB css = CRCItemsB.Find(
                delegate(CRCStatusB info)
                {
                    return info.CRCNum == num;
                }
            );
            if (css != null)
            {
                if (str == "Break")
                {
                    css.CrcStatus = EQUIPMENT_ERROR;
                    css.CraneMode = "0";
                    css.LabelInfo = "位置:" + css.Position + ".模式:关闭.状态:无.起止:" + css.Fromstation + "-->" + css.Tostation + ".";
                    dal.UpdateSCStatusNTI(css.CRCNum, css.CrcStatus);
                }
                else if (msg.Length > 38)
                {
                    string mode = EQUIPMENT_ERROR;
                    int[] functionmode = FunctionMode(int.Parse(msg[23]));
                    int[] cranemode = FunctionMode(int.Parse(msg[38]));
                    if (functionmode[0] == 1 && functionmode[4] == 1 && msg[38] == "1")
                    { mode = EQUIPMENT_OK; }

                    int t = int.Parse(msg[20].ToString()) * 256 + int.Parse(msg[21].ToString());
                    if (t != 0 && t.ToString() != css.TaskNo)
                        dal.UpdateTaskStatus(t.ToString(), "SC", "2", "1");
                        //dal.UpdateTaskStatus(t.ToString(), "SC", "2", "1");
                    css.TaskNo = t.ToString();
                    css.CraneMode = msg[38];
                    //css.FunctionReportH = msg[38];
                    css.FunctionMode = msg[23];
                    css.FunctionReport = msg[22];
                    css.CrcStatus = mode;
                    css.ZAlarm = functionmode[7];
                    css.ActionPoint = msg[26];
                    css.AisleNumber = msg[27];
                    css.RackSide = msg[28];
                    css.RackPosition = msg[29];
                    css.RackPlace = msg[30];
                    css.RackLevel = msg[31];
                    css.RackDepth = msg[32];
                    css.Position = GetLocation(num,int.Parse(msg[27]), int.Parse(msg[28]), int.Parse(msg[29]), int.Parse(msg[30]), int.Parse(msg[31]), int.Parse(msg[32]));
                    css.AlarmMessage = GetAlarmInfo(num, "", false);
                    css.LabelInfo = "任务号:" + css.TaskNo + ".位置:" + css.Position + ".模式:" + GetFunctionMode(functionmode, cranemode) + ".状态:" + GetTaskStatusBase(msg[22],msg[38]) + ".起止:" + css.Fromstation + "-->" + css.Tostation + "." + css.AlarmMessage;
                    if (css.LabelInfo == css.OldLabelInfo)
                        css.Change = false;
                    else
                    {
                        css.OldLabelInfo = css.LabelInfo;
                        css.Change = true;
                        dal.UpdateSCStatusNTI(css.CRCNum, css.CrcStatus);
                        //UpdateEquipmentStatus(t.ToString(), css.Position, GetFunctionMode(functionmode, cranemode), GetTaskStatusBase(msg[22],msg[38]), css.Fromstation, css.Tostation, css.AlarmMessage, num);
                    }
                    if (css.CrcStatus == EQUIPMENT_ERROR)
                        css.CrcTaskFlag = "0";
                    else
                        css.CrcTaskFlag = "1";

                    CRCObjectB co = crclistB.Find(
                         delegate(CRCObjectB cob)
                         {
                             return (cob.ScNo == num && cob.Port == 2000);
                         }
                    );
                    if (co != null)
                    {
                        if (System.String.IsNullOrEmpty(css.Fromstation) || System.String.IsNullOrEmpty(css.Tostation))
                        {
                            string[] ss = new string[2];
                            ss = dal.GetTostation(css.TaskNo);
                            if (ss != null)
                            {
                                css.Fromstation = ss[0];
                                css.Tostation = ss[1];
                            }
                        }
                        string destr = "";
                        if (!System.String.IsNullOrEmpty(css.Tostation))
                        {
                            if (css.Tostation.Contains("A"))
                                destr = css.Tostation.Replace("A", "");
                            else
                                destr = css.Tostation;
                        }
                        if (css.FunctionReport == CR_GETC_EXECUTED)
                        {
                            #region CR_GETC_EXECUTED 取货完成
                            if (css.Fromstation != null && css.Fromstation.Length == 4 && css.TaskNo.Length > 4)
                            {
                                string[] clear = new string[11];
                                string[] info = ReadStationInfo(int.Parse(css.Fromstation));
                                for (int i = 0; i < 11; i++)
                                {
                                    clear[i] = "0";
                                }
                                clear[0] = css.Fromstation;
                                co.BllSrm.SendCode(num, CR_PUTC, 0, int.Parse(css.TaskNo), destr.Replace("A", "").Replace("B", ""));
                                if (info[1] == css.TaskNo)
                                {
                                    logsta.WriteLog("清除入库口任务" + clear[0]);
                                    bool clearstation = WriteStationInfo(clear);
                                    if (clearstation)
                                    {
                                        logsta.WriteLog("清除入库口任务成功");
                                    }
                                }
                            }
                            //发指令调试注释
                            else
                                //co.BllSrm.SendCode(num, CR_PUTC, 0, int.Parse(css.TaskNo), css.Tostation.Replace("A",""));
                                co.BllSrm.SendCode(num, CR_PUTC, 0, int.Parse(css.TaskNo), destr.Replace("A", "").Replace("B", ""));                             
                            //发指令调试注释
                            #endregion
                        }
                        else if (css.FunctionReport == CR_PUTC_EXECUTED)
                        {
                            #region CR_PUTC_EXECUTED 放货完成                                                         
                            if (css.TaskNo != null && css.TaskNo.Length > 4)
                            {                                    
                                //co.BllSrm.SendCode(num, CR_NO_FUNC, 0, int.Parse(css.TaskNo), destr);
                                //dal.UpdateTaskStatus(css.TaskNo, "SC", "1", "1");
                                if (co.BllSrm.SendCode(num, CR_NO_FUNC, 0, int.Parse(css.TaskNo), destr.Replace("A", "").Replace("B", "")))
                                {
                                    dal.UpdateTaskStatus(css.TaskNo, "SC", "1", "1");
                                    #region Tostation.Length == 4
                                    if (css.Tostation != null && css.Tostation.Length == 4 && dal.GetIsPut(css.TaskNo)=="0")
                                    {
                                        string itemtype;
                                        string barcode, addressstation = "0";
                                        barcode = dal.GetBarcode(css.TaskNo);
                                        string[] info = barcode.Split(new char[] { ',' });
                                        if (info.Length > 2)
                                            addressstation = info[2];

                                        //else
                                        //    addressstation = num == "CRC001" ? "5066" : "5071";

                                        if (info[0] == "99999999")
                                        {
                                            itemtype = "2";
                                        }
                                        else
                                        {
                                            itemtype = "3";
                                        }
                                        string[] strsta = new string[11];
                                        strsta[0] = css.Tostation;
                                        strsta[1] = css.TaskNo;
                                        int upbarcode1 = 0;
                                        int upbarcode2 = 0;
                                        strsta[2] = upbarcode1.ToString();
                                        strsta[3] = upbarcode2.ToString();
                                        strsta[4] = itemtype;
                                        strsta[5] = css.Tostation;
                                        strsta[6] = addressstation;//info[2];//css.STAToStation;
                                        strsta[7] = "0";
                                        strsta[8] = "0";
                                        strsta[9] = "0";
                                        strsta[10] = "0";
                                        if (WriteStationInfo(strsta))
                                        {
                                            dal.UpdateTaskStatus(css.TaskNo, "STA", "2", "1");
                                            #region 出库放货后取入库任务
                                            if (num.Equals("CRC001"))
                                            {
                                                string task5041 = ReadStationInfo(5041)[1];
                                                if (Int32.Parse(task5041) > 9999)
                                                {
                                                    DataSet dstemp = dal.CheckInTask_1073(task5041);
                                                    if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count > 0)
                                                    {
                                                        String taskno = dstemp.Tables[0].Rows[0][0].ToString();
                                                        String fromstation = dstemp.Tables[0].Rows[0][1].ToString();
                                                        String tostation = dstemp.Tables[0].Rows[0][2].ToString();
                                                        if (crcb.UpdateCRCTaskInfo(num, fromstation, tostation, "0", "Task_InBound"))
                                                        {
                                                            bool sendflag = co.BllSrm.SendCode(num, CR_GETC, 0, int.Parse(taskno), fromstation);
                                                            if (sendflag)
                                                                dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                                                            //dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                                                        }
                                                    }
                                                }
                                            }
                                            if (num.Equals("CRC002"))
                                            {
                                                string task5049 = ReadStationInfo(5049)[1];
                                                if (Int32.Parse(task5049) > 9999)
                                                {
                                                    DataSet dstemp = dal.CheckInTask_1073(task5049);
                                                    if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count > 0)
                                                    {
                                                        String taskno = dstemp.Tables[0].Rows[0][0].ToString();
                                                        String fromstation = dstemp.Tables[0].Rows[0][1].ToString();
                                                        String tostation = dstemp.Tables[0].Rows[0][2].ToString();
                                                        if (crcb.UpdateCRCTaskInfo(num, fromstation, tostation, "0", "Task_InBound"))
                                                        {
                                                            bool sendflag = co.BllSrm.SendCode(num, CR_GETC, 0, int.Parse(taskno), fromstation);
                                                            if (sendflag)
                                                                dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                                                            //dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion 
                                }
                            }           
                            
                            #endregion 
                        }
                        //else if (css.FunctionReport == CR_NO_FUNC.ToString())
                        //{
                        //    if (num.Equals("CRC001"))
                        //    {
                        //        string task5041 = ReadStationInfo(5041)[1];
                        //        if (Int32.Parse(task5041) > 9999)
                        //        {
                        //            DataSet dstemp = dal.CheckInTask_1073(task5041);
                        //            if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count > 0)
                        //            {
                        //                String taskno = dstemp.Tables[0].Rows[0][0].ToString();
                        //                String fromstation = dstemp.Tables[0].Rows[0][1].ToString();
                        //                String tostation = dstemp.Tables[0].Rows[0][2].ToString();
                        //                if (crc.UpdateCRCTaskInfo(num, fromstation, tostation, "0", "Task_InBound"))
                        //                {
                        //                    bool sendflag = co.BllSrm.SendCode(num, CR_GETC, 0, int.Parse(taskno), fromstation);
                        //                    if(sendflag)
                        //                        dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                        //                    //dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                        //                }
                        //            }
                        //        }
                        //    }
                        //    if (num.Equals("CRC002"))
                        //    {
                        //        string task5049 = ReadStationInfo(5049)[1];
                        //        if (Int32.Parse(task5049) > 9999)
                        //        {
                        //            DataSet dstemp = dal.CheckInTask_1073(task5049);
                        //            if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count > 0)
                        //            {
                        //                String taskno = dstemp.Tables[0].Rows[0][0].ToString();
                        //                String fromstation = dstemp.Tables[0].Rows[0][1].ToString();
                        //                String tostation = dstemp.Tables[0].Rows[0][2].ToString();
                        //                if (crc.UpdateCRCTaskInfo(num, fromstation, tostation, "0", "Task_InBound"))
                        //                {
                        //                    bool sendflag = co.BllSrm.SendCode(num, CR_GETC, 0, int.Parse(taskno), fromstation);
                        //                    if(sendflag)
                        //                        dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                        //                    //dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            else
            {
                CRCStatusB status = new CRCStatusB();
                status.CRCNum = num;
                status.AisleNumber = aisle;
                CRCItemsB.Add(status);
            }
        }

        public string GetAlarmInfo(string num, string alarm, bool update)
        {
            CRCStatusB css = CRCItemsB.Find(
                delegate(CRCStatusB info)
                {
                    return info.CRCNum == num;
                }
            );
            if (css == null)
                return "";
            if (update)
                css.AlarmInfo = alarm;
            else
            {
                string stralarm = "", changealarm = "";
                int[] crcmodestatus = new int[8];
                int[] crcplcinfo = new int[11];
                if (!System.String.IsNullOrEmpty(css.AlarmInfo))
                {
                    string[] msg = css.AlarmInfo.Split(new char[] { ';' });
                    for (int i = 0; i < msg.Length; i++)
                    {
                        if (msg[i] != "")
                        {
                            string index = msg[i].Substring(0, msg[i].IndexOf(':'));
                            string ala = msg[i].Substring(msg[i].IndexOf(':') + 1, msg[i].Length - msg[i].IndexOf(':') - 1);
                            int[] alarmbit = FunctionMode(int.Parse(ala));
                            for (int j = 0; j < alarmbit.Length; j++)
                            {
                                if (alarmbit[j] == 1)
                                {
                                    foreach (DictionaryEntry de in AlarmHash)
                                    {
                                        if (de.Key.ToString() == css.CRCNum.Substring(0, 1) + index + "." + j.ToString())
                                        {
                                            stralarm += de.Value.ToString() + ";";
                                            changealarm += index + "." + j.ToString() + ";";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int k = 0; k < (stralarm.Length-1) / 15; k++)
                    {
                        stralarm = stralarm.Insert((k + 1) * 15, ".");//stralarm.Substring(0, (k + 1) * 15 + k) + "." + stralarm.Substring((k + 1) * 15 + k + 1, stralarm.Length - 1);
                    }
                    return stralarm;
                }
            }
            return "";
        }
        //public bool UpdateCRCTaskInfo(string num, string fromstation, string tostation, string statostation,string tasktype,string field3)
        public bool UpdateCRCTaskInfo(string num, string fromstation, string tostation, string statostation,string tasktype)
        {
            CRCStatusB css = CRCItemsB.Find(
                delegate(CRCStatusB info)
                {
                    return info.CRCNum == num;
                }
            );
            if (css != null)
            {
                if (!System.String.IsNullOrEmpty(fromstation))
                    css.Fromstation = fromstation;
                css.Tostation = tostation;
                css.STAToStation = statostation;
                css.TaskType = tasktype;
                //css.Field3 = field3;
                return true;
            }
            return false;
        }
        private string GetLocation(string num,int p1, int p2, int p3, int p4, int p5, int p6)
        {
            string str = "";
            //int row = p3 == 255 ? 0 : p3; ;// p3 > 10 ? 0 : p3; //(p1 - 1) * 2 + (p2 == 0 ? 1 : p2);
            //int col = p6 == 255 ? 0 : p6;// (p3 - 1) * 2 + p4;
            int row = p3;
            int col = p6;
            if (num == "CRC001")
            {
                //col = p6 == 255 ? 0 : p6 == 0 ? 0 : (13 - col);
                col = p6 == 255 ? 0 : p6 == 0 ? 0 : (13 - col);
                row = p3 == 255 ? 1 : p3 == 254 ? 2 : p3;
            }
            else
            {
                row = p3 == 254 ? 1 : p3 == 255 ? 2 : p3;
            }
            str += p5;
            str += row.ToString().PadLeft(2, '0');
            str += col.ToString().PadLeft(2, '0');
            //str += "1";//p5.ToString().PadLeft(3, '0') + p6.ToString();
            return str;
        }
        public int[] FunctionMode(int checkinfo)
        {
            string errorcode = Convert.ToString(checkinfo, 2);
            Int32 length = errorcode.Length;
            int[] ErrorCode = new int[8];
            for (int i = length - 1; i >= 0; i--)
            {
                ErrorCode[length - i - 1] = int.Parse(errorcode.Substring(i, 1).ToString());
            }
            return ErrorCode;
        }
        public string GetFunctionMode(int[] functionmode, int[] cranemode)
        {
            string mode = (functionmode[0] == 1 ? "自动;" : "");
            mode += (functionmode[1] == 1 ? "半自动;" : "");
            mode += (functionmode[2] == 1 ? "手动;" : "");
            mode += (functionmode[3] == 1 ? "软停;" : "");
            mode += (functionmode[4] == 1 ? "就绪;" : "");
            mode += (functionmode[7] == 1 ? "故障;" : "正常;");
            if (cranemode[0] == 1)
                mode += "远程;";
            if (cranemode[1] == 1)
                mode += "本地;";
            if (cranemode[7] == 1)
                mode += "报警;";
            return mode;
        }

        private string CheckStation(string fromstation, string tostation, string taskno)
        {
            if (fromstation.Length == 10 && tostation.Length == 10)
                return taskno;
            else
            {
                string[] stationinfo;
                if (fromstation.Length == 4)
                {
                    stationinfo = ReadStationInfo(int.Parse(fromstation));
                    return stationinfo[1];
                }
                else
                {
                    stationinfo = ReadStationInfo(int.Parse(tostation));
                    if (stationinfo[1].Length < 5)
                        return taskno;
                }
            }
            return "0";
        }

        public DataTable GetTask()
        {
            string lane = "", executingsc = "";
            int distance;
            bool flag = true;
            DataTable dt = new DataTable();
            dt.Columns.Add("num", Type.GetType("System.String"));
            dt.Columns.Add("taskno", Type.GetType("System.String"));
            dt.Columns.Add("fromstation", Type.GetType("System.String"));
            dt.Columns.Add("tostation", Type.GetType("System.String"));
            dt.Columns.Add("statostation", Type.GetType("System.String"));
            dt.Columns.Add("distance", Type.GetType("System.Int32"));
            dt.Columns.Add("tasktype", Type.GetType("System.String"));
            //dt.Columns.Add("field3", Type.GetType("System.String"));
            CRCStatusB css = null;
            foreach (CRCObjectB co in crclistB)
            {
                if (co.Port == 4000)
                {
                    lane = "";
                    lane += co.ScNo.Substring(co.ScNo.Length - 2, 2);
                    css = CRCItemsB.Find(
                        delegate(CRCStatusB info)
                        {
                            return info.CRCNum == co.ScNo;
                        }
                    );
                    if (css != null)
                        lane += css.CrcTaskFlag;
                    else
                        lane += "0";
                    //DataSet ds = dal.GetSRMTask("010021");
                    DataSet ds = dal.GetSRMTask(lane);
                    if (ds != null && css.FunctionReport == CR_NO_FUNC.ToString())
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
                                if (CheckStation(dts.Rows[k]["fromstation"].ToString().Replace("B", ""), dts.Rows[k]["tostation"].ToString().Replace("B", ""), dts.Rows[k]["taskno"].ToString()) == dts.Rows[k]["taskno"].ToString())
                                {
                                    distance = GetDistance(dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["equipmentnumber"].ToString());
                                    if (distance >= 0)
                                    {
                                        //for (int i = 0; i < dt.Rows.Count; i++)
                                        //{
                                        //    if (dt.Rows[i]["num"].ToString() == dts.Rows[k]["equipmentnumber"].ToString())
                                        //    {
                                        //        flag = false;
                                        //        if (int.Parse(dt.Rows[i]["distance"].ToString()) > distance)
                                        //        {
                                        //            dt.Rows.Remove(dt.Rows[i]);
                                        //            dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), distance });
                                        //        }
                                        //    }
                                        //}
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            if (row["num"].ToString() == dts.Rows[k]["equipmentnumber"].ToString())
                                            {
                                                flag = false;
                                                if (int.Parse(row["distance"].ToString()) > distance)
                                                {
                                                    row["num"] = dts.Rows[k]["equipmentnumber"].ToString();
                                                    row["taskno"] = dts.Rows[k]["taskno"].ToString();
                                                    row["fromstation"] = dts.Rows[k]["fromstation"].ToString();
                                                    row["tostation"] = dts.Rows[k]["tostation"].ToString();
                                                    row["statostation"] = dts.Rows[k]["currentstation"].ToString();
                                                    row["tasktype"] = dts.Rows[k]["field2"].ToString();
                                                    row["distance"] = distance;
                                                    //row["field3"] = dts.Rows[k]["field3"].ToString();
                                                    //dt.Rows.Remove(row);
                                                    //dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), distance });
                                                }
                                            }
                                        }
                                        if (flag)
                                        {
                                            //if (dts.Rows[k]["tostation"].ToString().Length < 5 && !string.IsNullOrEmpty(dts.Rows[k]["field4"].ToString()))
                                            //    //dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), distance, dts.Rows[k]["tasktype"].ToString(), dts.Rows[k]["field3"].ToString() });
                                            //    dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), distance, dts.Rows[k]["tasktype"].ToString() });
                                            //else if (dts.Rows[k]["tostation"].ToString().Length > 4 || dts.Rows[k]["barcode"].ToString() == "99999999")
                                            dt.Rows.Add(new object[] { dts.Rows[k]["equipmentnumber"].ToString(), dts.Rows[k]["taskno"].ToString(), dts.Rows[k]["fromstation"].ToString(), dts.Rows[k]["tostation"].ToString(), dts.Rows[k]["currentstation"].ToString(), distance, dts.Rows[k]["field2"].ToString() });
                                        }
                                        else
                                            flag = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return dt;
        }

        private int GetDistance(string fromstation, string tostation, string equipmentnumber)
        {
            fromstation = fromstation.Replace("A", "").Replace("B", "");
            tostation = tostation.Replace("A", "").Replace("B", "");
            if (equipmentnumber.Length == 1)
            {
                equipmentnumber = "CRC00" + equipmentnumber;
            }
            else
            {
                equipmentnumber = "CRC0" + equipmentnumber;
            }
            CRCStatusB css = CRCItemsB.Find
            (
                delegate(CRCStatusB info)
                {
                    return info.CRCNum == equipmentnumber;
                }
            );
            if (css != null && css.RackPosition != null)
            {
                if (fromstation.Length == 4)
                    return int.Parse(css.RackPosition);//XF列
                else
                    return Math.Abs(int.Parse(css.RackPosition) - (int.Parse(fromstation.Substring(0, 3)) - 100));
            }
            return -1;
            //return 1;
        }
    }
}
