using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace DataAccess
{
    public class DALDATA : DALBase
    {
        public DataSet GetOPCItem()
        {
            OracleParameter parm = new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50);
            parm.Value = WareHouseID;
            DataSet ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_OPCITEM, parm);
            if (ds == null)
                loginfo.WriteLog("GetOPCItem()获取OPC项信息出错");
            return ds;
        }
        public int ADDInboundTask(string sql)
        {
            return ExecuteNonQuery(CommandType.Text, sql, null);
        }
        public DataSet GetEquipmentInfo(string zone)
        {
            OracleParameter parm = new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50);
            parm.Value = WareHouseID;
            DataSet ds;
            if (zone == "A")
            {
                ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_WCSEQUIPMENT, parm);
            }
            else
            {
                ds = ExecuteDataSet(CommandType.Text, "SELECT * FROM WCS_EQUIPMENTINFO WHERE WAREHOUSEID=:WareHouseID AND EQUIPMENTSTATUS='1' AND TO_NUMBER(EQUIPMENTAISLE)<9 ORDER BY EQUIPMENTNAME,EQUIPMENTPORT", parm);
            }
            if (ds == null)
                loginfo.WriteLog("GetEquipmentInfo()获取设备信息出错");
            return ds;
        }
        public DataSet GetEquipmentInfoB(string zone)
        {
            OracleParameter parm = new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50);
            parm.Value = WareHouseID;
            DataSet ds;
            if (zone == "A")
            {
                ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_WCSEQUIPMENT, parm);
            }
            else
            {
                ds = ExecuteDataSet(CommandType.Text, "SELECT * FROM WCS_EQUIPMENTINFO WHERE WAREHOUSEID=:WareHouseID AND EQUIPMENTSTATUS='1' AND TO_NUMBER(EQUIPMENTAISLE)>8 ORDER BY EQUIPMENTNAME,EQUIPMENTPORT", parm);
            }
            if (ds == null)
                loginfo.WriteLog("GetEquipmentInfo()获取设备信息出错");
            return ds;
        }
        /// <summary>
        /// chenjh 20150317
        /// </summary>
        /// <param name="plcNumber"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public DataSet GetEquipmentInfo(string plcNumber, string zone)
        {
            DataSet ds = null;
            try
            {
                OracleParameter[] parms = new OracleParameter[]
                {
                    new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                    new OracleParameter(PARAM_ZONE, OracleType.VarChar, 30),
                    new OracleParameter(PARAM_PLCNUMBER, OracleType.VarChar, 30),
                };
                parms[0].Value = WareHouseID;
                parms[1].Value = zone;
                parms[2].Value = plcNumber;
                ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_WCSEQUIPMENT_BYD, parms);
                if (ds == null)
                    loginfo.WriteLog("GetEquipmentInfo()获取设备信息出错");
            }
            catch (Exception e)
            { }
            return ds;
        }

        public string[] GetInboundTask(string Barcode, string FromStation, string USERID, int InsertWMSFlag, int TaskNo, int CheckFlag, string Itemcodes)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                new OracleParameter(@"i_Barcode", OracleType.VarChar, 50),
                new OracleParameter(@"i_FromStation", OracleType.VarChar, 10),
                new OracleParameter(@"i_USERID", OracleType.VarChar, 50),
                new OracleParameter(@"i_InsertWMSFlag", OracleType.Int32),
                new OracleParameter(@"i_TaskNo", OracleType.Int32),
                new OracleParameter(@"i_CheckFlag", OracleType.Int32),
                new OracleParameter(@"i_Itemcodes", OracleType.VarChar, 800),
                new OracleParameter(@"o_result", OracleType.Int32),
                new OracleParameter(@"o_TaskNO", OracleType.Int32),
                new OracleParameter(@"o_ToStation", OracleType.VarChar, 10)
            };
            for (int i = 0; i < 8; i++)
            { parms[i].Direction = ParameterDirection.Input; }
            parms[8].Direction = ParameterDirection.Output;
            parms[9].Direction = ParameterDirection.Output;
            parms[10].Direction = ParameterDirection.Output;
            parms[0].Value = WareHouseID;
            parms[1].Value = Barcode;
            parms[2].Value = FromStation;
            parms[3].Value = USERID;
            parms[4].Value = InsertWMSFlag;
            parms[5].Value = TaskNo;
            parms[6].Value = CheckFlag;
            parms[7].Value = Itemcodes;
            parms[8].Value = 0;
            parms[9].Value = 0;
            parms[10].Value = 0;
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_GETINBOUNDTASK_P", parms);
            if (str[7] == "0")
                loginfo.WriteLog("WCS_GETINBOUNDTASK_P 获取入库任务失败:申请站台" + FromStation + " 外形检测" + CheckFlag.ToString() + " 传入条码:" + Itemcodes);
            return str;
        }
        public bool UpdateOutrequestState(string requestno, string feild3, string state)
        {
            int proqty=1,actualqty=0;
            if (state == "2")
            {
                state = "Finished";
                actualqty = 1;
            }
            if (state == "0")
            {
                state = "New";
                proqty = 0;
            } 
            if (state == "1")
            {
                state = "Executing";
            }
            string str_UpdateOutrequest = "update outrequest set state='" + state + "',processedqty='" + proqty + "',actualqty='" + actualqty + "' where requestsequence =  '" + requestno + "'  and  field3 = '" + feild3 + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_UpdateOutrequest, null);
            return i > 0 ? true : false;
        }
        public string UpdateTaskStatus(string TaskNo, string TaskType, string TaskStatus, string RunStatus)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_WarehouseID", OracleType.VarChar, 50),
                new OracleParameter(@"i_TaskNo", OracleType.Number),
                new OracleParameter(@"i_EquipmentType", OracleType.VarChar, 10),
                new OracleParameter(@"i_TaskStatus", OracleType.VarChar),
                new OracleParameter(@"i_RunStatus", OracleType.VarChar, 10),
                new OracleParameter(@"o_result", OracleType.Int32)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = int.Parse(TaskNo);
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = TaskType;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = TaskStatus;
            parms[4].Direction = ParameterDirection.Input;
            parms[4].Value = RunStatus;
            parms[5].Direction = ParameterDirection.Output;
            parms[5].Value = 0;
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_UPDATETASKSTATUS_P", parms);
            if (System.String.IsNullOrEmpty(str[5]))
            {
                loginfo.WriteLog("WCS_UPDATETASKSTATUS_P 更改任务状态失败:" + TaskNo + "/" + TaskStatus);
                return "0";
            }
            return str[5];
        }
        public string CallPalletOut(string TaskNo)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                new OracleParameter(@"i_destination", OracleType.VarChar, 50),
                new OracleParameter(@"i_userid", OracleType.VarChar, 50), 
                new OracleParameter(@"o_result", OracleType.Int32)
            };

            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = TaskNo;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = "WCS";
            parms[3].Direction = ParameterDirection.Output;
            parms[3].Value = 0;
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_PALLETOUTBOUND_P", parms);
            if (System.String.IsNullOrEmpty(str[3]))
            {
                loginfo.WriteLog("WCS_PALLETOUTBOUND_P 出库空托盘组失败!");
                return "0";
            }
            return str[3];
        }
        public bool UpdateTaskStatus(string TaskNo, string TaskType, string TaskStatus, string RunStatus,int ScNo)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_WarehouseID", OracleType.VarChar, 50),
                new OracleParameter(@"i_taskno", OracleType.Number),
                new OracleParameter(@"i_equipmenttype", OracleType.VarChar, 10),
                new OracleParameter(@"i_scstatus", OracleType.VarChar),
                new OracleParameter(@"i_runstatus", OracleType.VarChar, 10),
                new OracleParameter(@"i_scno", OracleType.Int32),
                new OracleParameter(@"o_result", OracleType.Int32),
                new OracleParameter(@"o_newstationno",OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = int.Parse(TaskNo);
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = TaskType;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = TaskStatus;
            parms[4].Direction = ParameterDirection.Input;
            parms[4].Value = RunStatus;
            parms[5].Direction = ParameterDirection.Input;
            parms[5].Value = ScNo;
            parms[6].Direction = ParameterDirection.Output;
            parms[6].Value = 0;
            parms[7].Direction = ParameterDirection.Output;
            parms[7].Value = "0";
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_UPDATESCINFO_P", parms);
            if (System.String.IsNullOrEmpty(str[6]) || str[6] == "0")
            {
                loginfo.WriteLog("WCS_UPDATETASKSTATUS_P 更改任务状态失败:" + TaskNo + "/" + TaskStatus);
                return false;
            }
            if (TaskType == "SC" && TaskStatus == "5" && str[7].Length<6)
            {
                loginfo.WriteLog("WCS_UPDATETASKSTATUS_P 获取新地址失败:" + TaskNo + "/" + TaskStatus);
                return false;
            }
            return true;
        }
        public bool UpdateSCStatusNTI(string crcnum, string crcstatus)
        {
            string equipmentnumber = crcnum.Replace("CRC00", "SC");
            if (equipmentnumber == "SC1")
            { }
            string str_UpdateSCStatus = "update equipment set statecode='" + crcstatus + "' where equipmentnumber =  '" + equipmentnumber + "' and statecode!='Equipment_Manual'  and  warehouseid = '" + WareHouseID + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_UpdateSCStatus, null);
            return i > 0 ? true : false;
        }
        public bool UpdateSCStatus(string crcnum, string crcstatus)
        {
            //crcstatus = "Equipment_OK";
            string equipmentnumber = crcnum.Replace("CRC00", "SC");
            string str_UpdateSCStatus = "update equipment set statecode='" + crcstatus + "' where equipmentnumber =  '" + equipmentnumber + "' and statecode!='Equipment_Manual' and  warehouseid = '" + WareHouseID + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_UpdateSCStatus, null);
            return i > 0 ? true : false;
        }
        public bool UpdateEquipmentStatus(string TaskNo, string position, string functionMode, string taskStatus,string fromStion,string toStion,string alarmMessage, string ScNo)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_taskNo", OracleType.VarChar, 10),
                new OracleParameter(@"i_scno", OracleType.VarChar, 10),
                new OracleParameter(@"i_position", OracleType.VarChar, 50),
                new OracleParameter(@"i_functionMode", OracleType.VarChar,200),
                new OracleParameter(@"i_taskStatus", OracleType.VarChar, 50),
                new OracleParameter(@"i_fromStation", OracleType.VarChar, 50),
                new OracleParameter(@"i_toStation", OracleType.VarChar, 50),
                new OracleParameter(@"i_alarmMessage", OracleType.VarChar, 100),
                new OracleParameter(@"o_ifsucceed",OracleType.Int32)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = TaskNo;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = ScNo;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = position;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = functionMode;
            parms[4].Direction = ParameterDirection.Input;
            parms[4].Value = taskStatus;
            parms[5].Direction = ParameterDirection.Input;
            parms[5].Value = fromStion == null ? "" : fromStion;
            parms[6].Direction = ParameterDirection.Input;
            parms[6].Value = toStion == null ? "" : toStion;
            parms[7].Direction = ParameterDirection.Input;
            parms[7].Value = alarmMessage== null ? "":alarmMessage==""?"":alarmMessage.Replace(".","");
            parms[8].Direction = ParameterDirection.Output;
            parms[8].Value = 0;
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_UPDATEEQUIPMENT_STATUS", parms);
            if (str[8]=="0")
            {
                loginfo.WriteLog("WCS_UPDATEEQUIPMENT_STATUS更新设备状态失败:");
                return false;
            }
            return true;
        }
        public DataSet GetSRMTask(string scstatus)
        {
            OracleParameter[] parms = new OracleParameter[]
                {
                    new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                    new OracleParameter(@"i_scstatus", OracleType.VarChar, 50),
                    new OracleParameter(@"o_taskinfo", OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = scstatus;
            parms[2].Direction = ParameterDirection.Output;
            parms[2].Value = null;

            return ExecuteDataSet(CommandType.StoredProcedure, "WCS_SCGETTASK_P", parms);
        }
        public DataSet GetGSSRMTask(string scstatus, string rackonestatus, string racktwostatus, string iserror)
        {
            if (rackonestatus == null || racktwostatus == null)
                return null;
            if (rackonestatus == "3" && racktwostatus == "3")
                return null;
            OracleParameter[] parms = new OracleParameter[]
                {
                    new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                    new OracleParameter(@"i_scstatus", OracleType.VarChar, 50),
                    new OracleParameter(@"i_rackonestatus", OracleType.VarChar, 50),
                    new OracleParameter(@"i_racktwostatus", OracleType.VarChar, 50),
                    new OracleParameter(@"i_iserror", OracleType.VarChar, 50),
                    new OracleParameter(@"o_taskinfo", OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = scstatus;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = rackonestatus;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = racktwostatus;
            parms[4].Direction = ParameterDirection.Input;
            parms[4].Value = iserror;
            parms[5].Direction = ParameterDirection.Output;
            parms[5].Value = null;

            return ExecuteDataSet(CommandType.StoredProcedure, "WCS_SCGETTASK_GS_P", parms);
        }
        public DataSet GetGSInTask(string scno)//, string rackonestatus, string racktwostatus, string iserror)
        {
            if (scno == "01")
                scno = "SC3";
            if (scno == "02")
                scno = "SC4";
            if (scno == "03")
                scno = "SC5";
            OracleParameter[] parms = new OracleParameter[]
                {
                    new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                    new OracleParameter(@"i_equipmentnumber", OracleType.VarChar, 50),
                    //new OracleParameter(@"i_rackonestatus", OracleType.VarChar, 50),
                    //new OracleParameter(@"i_racktwostatus", OracleType.VarChar, 50),
                    //new OracleParameter(@"i_iserror", OracleType.VarChar, 50),
                    new OracleParameter(@"o_result",OracleType.Int32),
                    new OracleParameter(@"o_outboundtask", OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = scno;
            parms[2].Direction = ParameterDirection.Output;
            parms[2].Value = null;
            parms[3].Direction = ParameterDirection.Output;
            parms[3].Value = null;
            //parms[4].Direction = ParameterDirection.Input;
            //parms[4].Value = iserror;
            //parms[5].Direction = ParameterDirection.Output;
            //parms[5].Value = null;

            return ExecuteDataSet(CommandType.StoredProcedure, "WCS_SCGETCIRCLETASK_GS", parms);
        }
        public DataTable GetAGVTask()
        {
            OracleParameter[] parms = new OracleParameter[]
                {
                    new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                    new OracleParameter(@"o_agvtask", OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = null;
            DataSet ds= ExecuteDataSet(CommandType.StoredProcedure, "WCS_GETAGVTASK_P", parms);
            if (ds == null)
                return null;
            else
                return ds.Tables[0];
        }
        //写入RFID信息
        //写入RFID信息
        public bool Write_RFID_Task(string casecodesr, int statusit, Single tarere, Single dynamictarere, string batchidsr, string blendcodesr, string versionsr, int batchsequencenumit, Single netre, Single moisturere, ref string guidsr, ref string ptime)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                new OracleParameter(@"i_casecodesr", OracleType.VarChar, 50),
                new OracleParameter(@"i_statusit", OracleType.Number),
                new OracleParameter(@"i_tarere", OracleType.Number),
                new OracleParameter(@"i_dynamictarere", OracleType.Number),
                new OracleParameter(@"i_batchidsr", OracleType.VarChar,20),
                new OracleParameter(@"i_blendcodesr", OracleType.VarChar, 16),
                new OracleParameter(@"i_versionsr", OracleType.VarChar, 20),
                new OracleParameter(@"i_batchsequencenumit", OracleType.Number),
                new OracleParameter(@"i_netre", OracleType.Number),
                new OracleParameter(@"i_moisturere", OracleType.Number),
                new OracleParameter(@"o_result", OracleType.Int16),
                new OracleParameter(@"o_guidsr", OracleType.VarChar, 32),
                new OracleParameter(@"o_ptime", OracleType.VarChar, 50)
        };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Input;
            parms[5].Direction = ParameterDirection.Input;
            parms[6].Direction = ParameterDirection.Input;
            parms[7].Direction = ParameterDirection.Input;
            parms[8].Direction = ParameterDirection.Input;
            parms[9].Direction = ParameterDirection.Input;
            parms[10].Direction = ParameterDirection.Input;
            parms[11].Direction = ParameterDirection.Output;
            parms[12].Direction = ParameterDirection.Output;
            parms[13].Direction = ParameterDirection.Output;

            parms[0].Value = WareHouseID;
            parms[1].Value = casecodesr.PadLeft(4, '0');
            parms[2].Value = statusit;
            parms[3].Value = tarere;
            parms[4].Value = dynamictarere;
            parms[5].Value = batchidsr;
            parms[6].Value = blendcodesr;
            parms[7].Value = versionsr;
            parms[8].Value = batchsequencenumit;
            parms[9].Value = netre;
            parms[10].Value = moisturere;
            parms[11].Value = 0;
            parms[12].Value = "";
            parms[13].Value = "";

            try
            {
                string[] str = ExecuteProcedure(CommandType.StoredProcedure, "wcs_i_itemcode_p", parms);
                string Result = str[11].ToString();
                if (Result == "1")
                {
                    guidsr = str[12].ToString();
                    ptime = str[13].ToString();
                    return true;
                }
                else
                {
                    guidsr = "";
                    ptime = "";
                    loginfo.WriteLog("Write_RFID_Task更新RFID信息失败:");
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        //核对RFID信息
        public string Check_RFID_Task(string BinNo, string ProdNo, string BatchNo, Single InitBinweight, Single binweight, Single Netweight, Single moisturere, string BinType, string BoxVersion)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                new OracleParameter(@"i_barcode", OracleType.VarChar, 50),
                new OracleParameter(@"i_prodno", OracleType.VarChar, 50),
                new OracleParameter(@"i_batchno", OracleType.VarChar, 50),
                new OracleParameter(@"i_initbinweight", OracleType.Number),
                new OracleParameter(@"i_binweight", OracleType.Number),
                new OracleParameter(@"i_netweight", OracleType.Number),
                new OracleParameter(@"i_moisturere", OracleType.Number),
                new OracleParameter(@"i_emptybinflag", OracleType.VarChar,50),
                new OracleParameter(@"i_versionsr", OracleType.VarChar, 50),
                new OracleParameter(@"o_result", OracleType.Int32)
        };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Input;
            parms[5].Direction = ParameterDirection.Input;
            parms[6].Direction = ParameterDirection.Input;
            parms[7].Direction = ParameterDirection.Input;
            parms[8].Direction = ParameterDirection.Input;
            parms[9].Direction = ParameterDirection.Input;
            parms[10].Direction = ParameterDirection.Output;

            parms[0].Value = WareHouseID;
            parms[1].Value = BinNo.PadLeft(4, '0');
            parms[2].Value = ProdNo;
            parms[3].Value = BatchNo;
            parms[4].Value = InitBinweight;
            parms[5].Value = binweight;
            parms[6].Value = Netweight;
            parms[7].Value = moisturere;
            parms[8].Value = BinType;
            parms[9].Value = BoxVersion;
            parms[10].Value = 0;

            try
            {
                string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_CHECKRFID_P", parms);
                return str[10];
            }
            catch
            {
                return "0";
            }
        }
        //查询是否已经有正在出的空箱任务
        public int YS_QueryEmptyPalletTask(string StaNo)
        {
            //string equipmentnum = num == 1 ? "SC1" : "SC2";//leo TC1、TC2 to SC1、SC2
            string str = "select count(*) from task t left join equipment e on e.equipmentid=t.EXECUTIVEEQUIPMENTID where t.targetaddresstext='" + StaNo + "' and t.warehouseid= :WareHouseID";

            OracleParameter parm = new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50);
            parm.Value = WareHouseID;
            object ob = ExecuteScalar(CommandType.Text, str, parm);
            return int.Parse(ob.ToString());
        }
        //查询小车状态
        public string QuerySCState()
        {
            string str = "select a.statecode||','||b.statecode from equipment a,equipment b where  a.equipmentnumber='SC1' and b.equipmentnumber='SC2'";//leo TC1、TC2 to SC1、SC2

            object ob = ExecuteScalar(CommandType.Text, str, null);
            return ob.ToString();
        }
        //查询入库道口设道信息
        public string QueryInLanewayInfo(int stationno)
        {
            string str = "select a.itemcode||','||b.samecode||','||b.code||','||a.field1||','||a.field2 from lanewayselect a inner join item b on a.itemid=b.itemid where a.equipmentid=(select equipmentid from equipment where field5='"+stationno+"')";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            if (ob == null)
                return null;
            else
                return ob.ToString();
        }
        //查询放行道口设备id和状态
        public string QueryLanewayState(int i)
        {
            string str = "select equipmentid||','||statecode from equipment where typecode='Equipment_Laneway' and wcseqposition='"+i+"'";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            return ob.ToString();
        }
        //根据物料ID查询放行道口设备状态
        public DataSet QueryLanewayStateByItemid(string itemid)
        {
            string str = "select a.itemcode,b.equipmentnumber,b.description,b.statecode,b.wcseqposition,b.field4,b.field5 from lanewayselect a inner join equipment b on a.equipmentid=b.equipmentid where a.itemid='" + itemid + "' and b.statecode in ('Equipment_OK','Equipment_Using') and b.equipmentnumber!='DockManOut' and wcseqposition is not null";
            DataSet ds = ExecuteDataSet(CommandType.Text, str, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                loginfo.WriteLog("QueryLanewayStateByItemid查询出库实箱信息返回为空");
                return null;
            }
        }
        //查询缓存站台对应出库物料id
        public string QueryItemBuffer(string field4)
        {
            string str = "select itemid from lanewayselect where equipmentid in (select equipmentid from equipment where field4='" + field4 + "' and statecode in ('Equipment_OK','Equipment_Using') and typecode='Equipment_Laneway')";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            if (ob == null)
                return null;
            else
                return ob.ToString();
        }
        //查询ABC类根据物料分类
        public string QueryItemCatalog(string itemid)
        {
            string str = "select name from itemcatalog where catalogid=(select parentid from item where itemid='"+itemid+"')";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            if (ob == null)
                return null;
            else
                return ob.ToString();
        }
        //查询BC类烟没过合流的数量
        public int QueryNotFlowCount()
        {
            string str = "select nvl(sum(requestqty - processedqty),'0') from outrequest where memo='B类烟' and state='Executing' and requestqty>processedqty";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            if (ob == null)
                return 0;
            else
                return int.Parse(ob.ToString());
        }
        //查询A类执行中且处理量为0的数量
        public int QueryExcuNopro(string targettext)
        {
            string str = "select count(1) from outrequest where state='Executing' and processedqty=0 and targetaddresstext='" + targettext + "' and memo='A类烟'";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            if (ob == null)
                return 0;
            else
                return int.Parse(ob.ToString());
        }
        //A类烟 在outrequest最小顺序号
        public string QuerySeqMin()
        {
            //string str = " select nvl(min(requestsequence),'0') from outrequest where state = 'New' and processedqty = 0 and memo='A类烟'";
            string str = " select nvl(min(field3),'0') from outrequest where state = 'New' and processedqty = 0 and memo='A类烟'";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            if (ob == null)
                return "0";
            else
                return ob.ToString();
        }
        //查询outrequest表
        public DataSet QueryOutrequest(string memo)
        {
            string str = "select * from (select requestsequence,itemcode,itemname,itemid,requestqty,processedqty,actualqty,targetaddresstext,state,field3,field4 from outrequest";
            if (memo == "A类烟")
            {
                str += "  where  memo ='" + memo + "' and state = 'New' and requestqty>processedqty order by field3,requestsequence) where rownum<2";
            }
            else
            {
                str += "  where  memo !='A类烟' and state = 'New' and requestqty>processedqty order by field3,requestsequence) where rownum<2";
            }
//            string str = @"select * from (select requestsequence,itemcode,itemname,itemid,requestqty,processedqty,actualqty,targetaddresstext,state,field3 
//                           from outrequest where  memo ='" + memo + "' and state = 'New' and requestqty>processedqty order by field3,requestsequence) where rownum<2 ";
            DataSet ds = ExecuteDataSet(CommandType.Text, str, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                loginfo.WriteLog("QueryOutrequest查询出库实箱信息返回为空");
                return null;
            }
        }
        //查询库存中是否有可用的此物料
        public int QueryCanOutItem(string itemid)
        {
            string str = "select count(1) from location where locationstate='LocationState_Stored' and field1='AutoLocation' and locationflag='LocationFlag_Enabled' and zoneid='806dad1ee0044b2eb2fa426457945ce2' and containerid in (select distinct(containerid) from containerdetail where itemid='"+itemid+"')";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            return int.Parse(ob.ToString());
        }
        //查询此站台出库中任务数量
        public int QueryOutTaskCount(string stationno)
        {
            //5069, 5070, 5075, 5076, 5077, 5078, 5079, 5080
            if (stationno == "5080")
                stationno = "5081";
            if (stationno == "5079")
                stationno = "5082";
            if (stationno == "5078")
                stationno = "5083";
            if (stationno == "5077")
                stationno = "5084";
            if (stationno == "5076")
                stationno = "5086";
            if (stationno == "5075")
                stationno = "5087";
            if (stationno == "5070")
                stationno = "5088";
            if (stationno == "5069")
                stationno = "5089";
            string str = "select count(1) from task where targetaddresstext='" + stationno + "'";
            object ob = ExecuteScalar(CommandType.Text, str, null);
            return int.Parse(ob.ToString());
        }
        //拆垛位要料
        public int CallOutTask(string Itemid,string Station,string seq)
        {
            //5069, 5070, 5075, 5076, 5077, 5078, 5079, 5080
            //89 88 87 76 84 83 82 81
            if (Station == "5069")
                Station = "5089";
            if (Station == "5070")
                Station = "5088";
            if (Station == "5075")
                Station = "5087";
            if (Station == "5076")
                Station = "5086";
            if (Station == "5077")
                Station = "5084";
            if (Station == "5078")
                Station = "5083";
            if (Station == "5079")
                Station = "5082";
            if (Station == "5080")
                Station = "5081";
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                new OracleParameter(@"i_itemid",OracleType.VarChar,32),
                new OracleParameter(@"i_destination", OracleType.VarChar, 50),
                new OracleParameter(@"i_sequence", OracleType.VarChar, 50),
                new OracleParameter(@"o_ifsucceed", OracleType.Int16)//, 
                //new OracleParameter(@"o_result1", OracleType.VarChar,50)
            };

            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = Itemid;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = Station;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = seq;
            parms[4].Direction = ParameterDirection.Output;
            //parms[3].Value = 0;
            //parms[4].Direction = ParameterDirection.Output;
            //parms[4].Value = null;
            //string[] str = ExecuteProcedure(CommandType.StoredProcedure, "I_DIRECToutBOUND_CP", parms);            
            int i = ExecuteNonQuerytwo(CommandType.StoredProcedure, "I_DIRECToutBOUND_CP", parms);

            //if (parms[4].Value.ToString() == "1")
            //{
            //    string str = "update outrequest set state = 'Executing' where requestsequence = '" + seq + "'";
            //    ExecuteNonQuery(CommandType.Text, str, null);
            //    //return i > 0 ? true : false;
            //}
            return int.Parse(parms[4].Value.ToString());
        }
        //记录烟丝批次
        public bool InsertYSBatch(string rfid,int isend)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                 new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                 new OracleParameter(@"i_rfid", OracleType.VarChar, 50),
                 new OracleParameter(@"i_endone", OracleType.Int16),
                 new OracleParameter(@"o_ifsucceed", OracleType.Int32)
          };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Output;

            parms[0].Value = WareHouseID;
            parms[1].Value = rfid;
            parms[2].Value = isend;
            parms[3].Value = 0;

            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_INSERTYSBATCH", parms);
            string Result = str[3].ToString();
            if (Result == "1")
                return true;
            else
                return false;
        }
        //产生出库任务 leo
        public bool YS_CreateOutBoundTask(string SourceStation, int TaskType, string ItemCode, string BatchNo, string BomNO, int BinQty, string BoxType, int ScNo, ref string taskNo)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                 new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                 new OracleParameter(@"i_tasktype", OracleType.Int32),
                 new OracleParameter(@"i_outtypeflag", OracleType.Int32),
                 new OracleParameter(@"i_sourcestation", OracleType.VarChar, 20),
                 new OracleParameter(@"i_prodno", OracleType.VarChar, 16),
                 new OracleParameter(@"i_prodname", OracleType.VarChar, 50),
                 new OracleParameter(@"i_batchno", OracleType.VarChar, 20),
                 new OracleParameter(@"i_bomno", OracleType.VarChar, 50),
                 new OracleParameter(@"i_binqty", OracleType.Int16),
                 new OracleParameter(@"i_userid", OracleType.VarChar, 32),          
                 new OracleParameter(@"i_boxtype", OracleType.VarChar, 32),
                 new OracleParameter(@"i_RunFlag", OracleType.Int32),
                 new OracleParameter(@"i_scno", OracleType.Int32),
                 new OracleParameter(@"o_result", OracleType.Int32),
                 new OracleParameter(@"o_taskno", OracleType.Number)
          };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Input;
            parms[5].Direction = ParameterDirection.Input;
            parms[6].Direction = ParameterDirection.Input;
            parms[7].Direction = ParameterDirection.Input;
            parms[8].Direction = ParameterDirection.Input;
            parms[9].Direction = ParameterDirection.Input;
            parms[10].Direction = ParameterDirection.Input;
            parms[11].Direction = ParameterDirection.Input;
            parms[12].Direction = ParameterDirection.Input;
            parms[13].Direction = ParameterDirection.Output;
            parms[14].Direction = ParameterDirection.Output;

            parms[0].Value = WareHouseID;
            parms[1].Value = TaskType;
            parms[2].Value = 1;
            parms[3].Value = SourceStation;
            parms[4].Value = ItemCode;
            parms[5].Value = "";
            parms[6].Value = BatchNo;
            parms[7].Value = BomNO;
            parms[8].Value = BinQty;
            parms[9].Value = "WCS";
            parms[10].Value = BoxType;
            parms[11].Value = 1;
            parms[12].Value = ScNo;
            parms[13].Value = 0;
            parms[14].Value = 0;

            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_I_CREATEOUTBOUND_P", parms);
            if (string.IsNullOrEmpty(str[13]))
                return false;
            else
            {
                string Result = str[13].ToString();
                if (Result == "1")
                {
                    taskNo = str[14];
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //查询正在出库实箱批次
        public DataSet YS_QueryBillWMSTask(string TaskNo)
        {
            string str = @"select receivinglotnumber from finishedtask where tasknumber in (select wmstaskno from (select wmstaskno from Wcs_Taskinfo_History where taskno = '" + TaskNo + "' and tasktype = 'Task_OutBound' and  warehouseid = :warehouseid order by createtime desc) where rownum < 2) and warehouseid = :warehouseid ";
            OracleParameter parm = new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50);
            parm.Value = WareHouseID;
            DataSet ds = ExecuteDataSet(CommandType.Text, str, parm);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                loginfo.WriteLog("YS_QueryBillWMSTask查询出库实箱信息返回为空");
                return null;
            }
        }

        //查询正在出的烟丝批次
        public DataSet YS_SerchMachineProductPlanExcuting()
        {
            string str = "select itemcode,lotnumber,version,nvl(qty1,0)-nvl(qty2,0) as outqty,machinenumber from machineproductplan where statecode = 'ProductionSchemeHead_Executing' and WareHouseID = '" + WareHouseID + "' and field1 = 1 and qty1 > qty2 and field2 < qty and field5 = 'Equipment_StationOut' order by length(nvl(field3,0)),field3,createtime";
            DataSet ds = ExecuteDataSet(CommandType.Text, str, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loginfo.WriteLog("YS_SerchMachineproductplan查询正在出的烟丝牌号和批次");
                return ds;
            }
            else
            {
                return null;
            }
        }
        //查询此任务物料出库道口
        public DataSet SelectOutLaneway(string Taskno)
        {
            string str = "select e.equipmentnumber,e.field4,e.field5,l.itemid,l.itemcode from lanewayselect l inner join equipment e on l.equipmentid=e.equipmentid where e.equipmentnumber like '%Out%' and l.itemid=(select itemid from task where tasknumber=(select wmstaskno from wcs_taskinfo where taskno='" + Taskno + "')) and e.field5 = (select tostation from wcs_subtaskinfo where taskno = '" + Taskno + "' and equipmenttype = 'STA') order by equipmentnumber";
            DataSet ds = ExecuteDataSet(CommandType.Text, str, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //oginfo.WriteLog("SelectOutLaneway查询正在出的烟丝牌号和批次");
                return ds;
            }
            else
            {
                return null;
            }
        }
        //下发放行后更新此outrequest中记录为Executing
        public bool UpdateOutReqExecuting(string requestsequence,string subseqno)
        {
            DateTime sendtime = DateTime.Now;
            string str = "update outrequest set state = 'Executing',field2='" + sendtime.ToString() + "',field4='"+subseqno+"' where requestsequence = '" + requestsequence + "'";
            int i = ExecuteNonQuery(CommandType.Text, str, null);
            return i > 0 ? true : false;
        }
        //下发放行后更新此outrequest中记录为Executing
        public bool UpdateOutReqTime(string requestsequence)
        {
            DateTime sendtime = DateTime.Now;
            string str = "update outrequest set modifytime='" + sendtime.ToString() + "' where requestsequence = '" + requestsequence + "'";
            int i = ExecuteNonQuery(CommandType.Text, str, null);
            return i > 0 ? true : false;
        }
        //下发放行后更新itempickinventory中放行数量
        public bool UpdatePickOutCount(int requestqty,string itemid)
        {
            string str = "update itempickinventory set sendoutcount = sendoutcount+"+requestqty+" where itemid = '" + itemid + "'";
            int i = ExecuteNonQuery(CommandType.Text, str, null);
            return i > 0 ? true : false;
        }  

        //查询正在出库牌号的任务数
        public int YS_SerchTaskCountByProdNo(string ProdNo)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_PRODNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = ProdNo;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_GETCOUNTBYPRODNO, parms);
            if (ob != null)
                return int.Parse(ob.ToString());
            else
                return 0;
        }
        //产生入库任务请求
        public bool YS_GetInboundTask(string BinType, string SourceStation, string TaskNo1, string TaskNo2, Int32 BinQty,int ScNo, string UserID, ref string reftaskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                 new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                 new OracleParameter(@"i_tasktype", OracleType.VarChar, 10),
                 new OracleParameter(@"i_sourcestation", OracleType.VarChar, 10),
                 new OracleParameter(@"i_binnol", OracleType.VarChar, 50),
                 new OracleParameter(@"i_binnoh", OracleType.VarChar, 50),
                 new OracleParameter(@"i_binqty", OracleType.Int32),
                 new OracleParameter(@"i_userid", OracleType.VarChar, 32),
                 new OracleParameter(@"i_scno", OracleType.Int32),
                 new OracleParameter(@"o_result", OracleType.Int32),
                 new OracleParameter(@"o_taskno", OracleType.Number)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Input;
            parms[5].Direction = ParameterDirection.Input;
            parms[6].Direction = ParameterDirection.Input;
            parms[7].Direction = ParameterDirection.Input;
            parms[8].Direction = ParameterDirection.Output;
            parms[9].Direction = ParameterDirection.Output;

            parms[0].Value = WareHouseID;
            parms[1].Value = BinType;
            parms[2].Value = SourceStation;
            parms[3].Value = TaskNo1;
            parms[4].Value = TaskNo2;
            parms[5].Value = BinQty;
            parms[6].Value = UserID;
            parms[7].Value = ScNo;
            parms[8].Value = 0;
            parms[9].Value = 0;

            try
            {
                string[] str = ExecuteProcedure(CommandType.StoredProcedure, "wcs_i_createinbound_p1", parms);
                string Result = str[8].ToString();
                if (Result == "1")
                {
                    reftaskno = str[9].ToString();
                    return true;
                }
                else
                {
                    loginfo.WriteLog("wcs_i_createinbound_p获取入库任务失败:");
                    return false;
                }
            }
            catch(Exception ee)
            {
                return false;
            }
        }
        //产生入库任务请求
        public bool ChangeLocation(string taskno,string BinType, string SourceStation, string TaskNo1, string TaskNo2, Int32 BinQty, int ScNo, string UserID, ref string reftaskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                 new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                 new OracleParameter(@"i_wcstasknumber", OracleType.VarChar, 50),
                 new OracleParameter(@"i_tasktype", OracleType.VarChar, 10),
                 new OracleParameter(@"i_sourcestation", OracleType.VarChar, 10),
                 new OracleParameter(@"i_binnol", OracleType.VarChar, 50),
                 new OracleParameter(@"i_binnoh", OracleType.VarChar, 50),
                 new OracleParameter(@"i_binqty", OracleType.Int32),
                 new OracleParameter(@"i_userid", OracleType.VarChar, 32),
                 new OracleParameter(@"i_scno", OracleType.Int32),
                 new OracleParameter(@"o_result", OracleType.Int32),
                 new OracleParameter(@"o_taskno", OracleType.Number)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Input;
            parms[5].Direction = ParameterDirection.Input;
            parms[6].Direction = ParameterDirection.Input;
            parms[7].Direction = ParameterDirection.Input;
            parms[8].Direction = ParameterDirection.Input;
            parms[9].Direction = ParameterDirection.Output;
            parms[10].Direction = ParameterDirection.Output;

            parms[0].Value = WareHouseID;
            parms[1].Value = taskno;
            parms[2].Value = BinType;
            parms[3].Value = SourceStation;
            parms[4].Value = TaskNo1;
            parms[5].Value = TaskNo2;
            parms[6].Value = BinQty;
            parms[7].Value = UserID;
            parms[8].Value = ScNo;
            parms[9].Value = 0;
            parms[10].Value = 0;

            try
            {
                string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_CHANGELOCATION_P", parms);
                string Result = str[9].ToString();
                if (Result == "1")
                {
                    reftaskno = str[10].ToString();
                    return true;
                }
                else
                {
                    loginfo.WriteLog("WCS_CHANGELOCATION_P获取入库任务失败:");
                    return false;
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        public string GetTask(string taskno, string barcode, string goodstype, string fromstation, string tostation, string checkinfo, string field1, string field2, string field3, string items)//, string isbctoa)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid",OracleType.VarChar,50),
                new OracleParameter(@"i_taskno",OracleType.VarChar,10),
                new OracleParameter(@"i_barcode",OracleType.VarChar,32),
                new OracleParameter(@"i_goodstype",OracleType.VarChar,10),
                new OracleParameter(@"i_fromstation",OracleType.VarChar,10),
                new OracleParameter(@"i_tostation",OracleType.VarChar,10),
                new OracleParameter(@"i_checkinfo",OracleType.VarChar,10),
                new OracleParameter(@"i_field1",OracleType.VarChar,10),
                new OracleParameter(@"i_field2",OracleType.VarChar,10),
                new OracleParameter(@"i_field3",OracleType.VarChar,10),
                new OracleParameter(@"i_items",OracleType.VarChar,970),
                new OracleParameter(@"i_pallet",OracleType.Int16),
                new OracleParameter(@"i_docktype",OracleType.VarChar,10),
               // new OracleParameter(@"i_isbctoa",OracleType.VarChar,10),
                new OracleParameter(@"o_result",OracleType.VarChar,100)
            };
            for (int i = 0; i < 13; i++)
            {
                parms[i].Direction = ParameterDirection.Input;
            }
            parms[13].Direction = ParameterDirection.Output;
            parms[0].Value = WareHouseID;
            parms[1].Value = taskno;
            parms[2].Value = barcode;
            parms[3].Value = goodstype;
            parms[4].Value = fromstation;
            parms[5].Value = tostation;
            parms[6].Value = checkinfo;
            parms[7].Value = field1;
            parms[8].Value = field2;
            parms[9].Value = field3;
            parms[10].Value = items;
            parms[11].Value = 0;
            parms[12].Value = field3+"."+field2;
            //parms[13].Value = isbctoa;
            parms[13].Value = null;
            string[] tt = ExecuteProcedure(CommandType.StoredProcedure, "WCS_GETTASK_P", parms);
            return tt[13];
        }
        public string GetAllowSend(string itemid)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid",OracleType.VarChar,50),
                new OracleParameter(@"i_itemid",OracleType.VarChar,32),
                new OracleParameter(@"o_result",OracleType.VarChar,100)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;
            parms[0].Value = WareHouseID;
            parms[1].Value = itemid;
            parms[2].Value = null;
            string[] tt = ExecuteProcedure(CommandType.StoredProcedure, "WCS_GetAllowSend", parms);
            return tt[2];
        }
        public bool DeleteSubTask(string taskno)
        {
            int subTaskFinish=0;

            string str = "delete from wcs_subtaskinfo where taskno='" + taskno + "' and warehouseid='" + WareHouseID + "'";
            subTaskFinish = ExecuteNonQuery(CommandType.Text,str,null);

            if (subTaskFinish>0)
            {
                str = "delete from wcs_taskinfo where taskno='" + taskno + "' and warehouseid='" + WareHouseID + "'";
                return ExecuteNonQuery(CommandType.Text, str, null)>0?true:false;
            }

            return false;
        }
        public bool UpdateMiniGetState(string taskno)
        {
            //wcs_subtaskinfo where  equipmenttype='SC'
            string str_Update = "update wcs_subtaskinfo set field3 = '789' where equipmenttype='SC' and taskno = '" + taskno + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_Update, null);
            return i > 0 ? true : false;
        }     
        public bool UpdateSCMode(string sc1in, string sc1out, string sc2in, string sc2out)
        {
            string str_UpdateSC1 = "update equipment set field1 = '" + sc1out + "',field2 = '" + sc1in + "' where equipmentnumber = 'SC1' and  warehouseid = '" + WareHouseID + "'";
            string str_UpdateSC2 = "update equipment set field1 = '" + sc2out + "',field2 = '" + sc2in + "' where equipmentnumber = 'SC2' and  warehouseid = '" + WareHouseID + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_UpdateSC1, null);
            if (i > 0)
                i = ExecuteNonQuery(CommandType.Text, str_UpdateSC2, null);
            return i > 0 ? true : false;
        }       
        public DataTable GetTaskInfo(int tasktype, int taskstatus, string fromtime, string totime, string wmstaskno, string taskno, string fromstation, string tostation, string barcode)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid", OracleType.VarChar, 50),
                new OracleParameter(@"i_tasktype", OracleType.Int16),
                new OracleParameter(@"i_taskstatus", OracleType.Int16),
                new OracleParameter(@"i_fromtime", OracleType.VarChar, 20),
                new OracleParameter(@"i_totime", OracleType.VarChar, 20),
                new OracleParameter(@"i_wmstaskno",OracleType.VarChar,50),
                new OracleParameter(@"i_fromstation",OracleType.VarChar,30),
                new OracleParameter(@"i_tostation",OracleType.VarChar,30),
                new OracleParameter(@"i_taskno",OracleType.VarChar,20),
                new OracleParameter(@"i_barcode",OracleType.VarChar,50),
                new OracleParameter(@"o_cursor", OracleType.Cursor)
            };
            for (int i = 0; i < 10; i++)
            {
                parms[i].Direction = ParameterDirection.Input;
            }
            parms[0].Value = WareHouseID;
            parms[1].Value = tasktype;
            parms[2].Value = taskstatus;
            parms[3].Value = fromtime;
            parms[4].Value = totime;
            parms[5].Value = wmstaskno;
            parms[6].Value = fromstation;
            parms[7].Value = tostation;
            parms[8].Value = taskno;
            parms[9].Value = barcode;

            parms[10].Direction = ParameterDirection.Output;
            parms[10].Value = null;
            DataSet ds = ExecuteDataSet(CommandType.StoredProcedure, "wcs_taskview_p", parms);
            if (ds == null)
            {
                loginfo.WriteLog("GetTaskInfo获取任务列表失败");
                return null;
            }
            else
                return ds.Tables[0];
        }

        public string[] GetTostation(string taskno)
        {
            string[] ss = new string[2];
            OracleParameter[] parms = new OracleParameter[] { new OracleParameter(":WareHouseID", OracleType.VarChar, 50), new OracleParameter(":Taskno", OracleType.VarChar, 50) };
            parms[0].Value = WareHouseID;
            parms[1].Value = taskno;
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            DataSet ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_TOSTATION, parms);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ss[0] = dt.Rows[0][0].ToString();
                    ss[1] = dt.Rows[0][1].ToString();
                }
            }
            return ss;
        }

        public bool AddTask(string fromstation, string tostation)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_WarehouseID", OracleType.VarChar, 50),
                new OracleParameter(@"i_fromstation", OracleType.VarChar, 20),
                new OracleParameter(@"i_tostation", OracleType.VarChar, 20),
                new OracleParameter(@"o_result", OracleType.Int16)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = fromstation;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = tostation;
            parms[3].Direction = ParameterDirection.Output;
            parms[3].Value = 0;
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_INSERTTASKINFO_P1", parms);
            if (str[3] == "1")
                return true;
            return false;
        }

        public string CheckItemCode(string stationno, string taskno, string itemcode)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_WarehouseID",OracleType.VarChar,50),
                new OracleParameter(@"i_station",OracleType.VarChar,10),
                new OracleParameter(@"i_taskno",OracleType.VarChar,10),
                new OracleParameter(@"i_itemcode",OracleType.VarChar,50),
                new OracleParameter(@"o_result",OracleType.VarChar,10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = stationno;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = taskno;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = itemcode;
            parms[4].Direction = ParameterDirection.Output;
            parms[4].Value = "0";
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_I_CHECKBARCODE_P", parms);
            if (System.String.IsNullOrEmpty(str[4]))
                loginfo.WriteLog(stationno + "核对条码失败!" + taskno + "/" + itemcode);
            return str[4];
        }

        public DataTable GetEventLog(string str, DateTime dts, DateTime dte, int ch)
        {
            string sqlstr = "select * from (select eventlog,to_char(createtime,'yyyy-mm-dd hh24:mi:ss') createtime,case when eventlog like '%失败%' or eventlog like '%无法%' then '0' else '1' end logtype from wcs_eventlog where warehouseid='" + WareHouseID + "' and (createtime between to_date('" + dts + "','yyyy-mm-dd hh24:mi:ss')" + " and to_date('" + dte + "','yyyy-mm-dd hh24:mi:ss'))) where '1'='1'";
            if (!string.IsNullOrEmpty(str))
            {
                sqlstr += " and eventlog like '%" + str + "%'";
            }
            if (ch == 1)
            {
                sqlstr += " and logtype='0'";
            }
            sqlstr += " order by createtime desc";
            DataSet ds = ExecuteDataSet(CommandType.Text, sqlstr, null);
            if (ds == null)
            {
                loginfo.WriteLog("GetEventLog获取日志信息失败");
                return null;
            }
            return ds.Tables[0];
        }
        //(int tasktype, int taskstatus, string fromtime, string totime, string wmstaskno, string taskno, string fromstation, string tostation, string barcode)
        public DataTable GetOutrequest(string itemcode,string itemname,string barcode, DateTime dts, DateTime dte)
        {
            string sqlstr = "select requestsequence,itemcode,itemname,requestqty,processedqty,actualqty,targetaddresstext,state,memo,field3,containerbarcode,createtime from (select * from outrequest where (createtime between to_date('" + dts + "','yyyy-mm-dd hh24:mi:ss')" + " and to_date('" + dte + "','yyyy-mm-dd hh24:mi:ss')) and state!='Finished') where '1'='1'";
            if (!string.IsNullOrEmpty(itemcode))
            {
                sqlstr += " and itemcode like '%" + itemcode + "%'";
            }
            if (!string.IsNullOrEmpty(itemname))
            {
                sqlstr += " and itemname like '%" + itemname + "%'";
            }
            if (!string.IsNullOrEmpty(barcode))
            {
                sqlstr += " and barcode like '%" + barcode + "%'";
            }
            sqlstr += " order by field3";
            DataSet ds = ExecuteDataSet(CommandType.Text, sqlstr, null);
            if (ds == null)
            {
                loginfo.WriteLog("GetEventLog获取日志信息失败");
                return null;
            }
            return ds.Tables[0];
        }
        public DataTable GetSCMode()
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            DataSet ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_SCMODE, parms);
            if (ds == null)
            {
                loginfo.WriteLog("获取SCMODE失败");
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }

        public DataTable GetItemCode(string taskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            DataSet ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_ITEMCODE, parms);
            if (ds == null)
            {
                loginfo.WriteLog("获取ItemCode失败");
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }
        public DataTable GetRFID(string rfidNO)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_CASECODESR, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = rfidNO;
            DataSet ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_GETRFID, parms);
            if (ds == null)
            {
                loginfo.WriteLog("获取ItemCode失败");
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }

        public string GetEquipmentNumber(string EquipmentNumber)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_EQUIPMENTNUMBER, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = "SC" + EquipmentNumber;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_EQUIPMENT, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "";
        }
        public string GetBarcodestv(string taskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_BARCODESTV, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetBarcode(string taskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_BARCODE, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetOutStation(string taskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_OUTSTATION, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetQTYStatus(string taskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_QTY, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetWMSTaskNo(string taskno)
        {
            string str = "select wmstaskno from wcs_taskinfo_history where taskno=:TaskNo and warehouseid=:WareHouseID and createtime>sysdate-1";
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, str, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetDestStationHis(string taskno)
        {
            string str = "select field5 from wcs_taskinfo_history where taskno=:TaskNo and warehouseid=:WareHouseID and createtime>sysdate-2";
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, str, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetDestStation(string taskno)
        {
            string str = "select field5 from wcs_taskinfo where taskno=:TaskNo and warehouseid=:WareHouseID";
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID, OracleType.VarChar, 50),
                new OracleParameter(PARAM_TASKNO, OracleType.VarChar, 10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, str, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        public string GetLedText(string taskno)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(PARAM_WAREHOUSE_ID,OracleType.VarChar,50),
                new OracleParameter(PARAM_TASKNO,OracleType.VarChar,10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = taskno;
            object ob = ExecuteScalar(CommandType.Text, SQL_SELECT_LEDTEXT, parms);
            if (ob != null)
                return ob.ToString();
            else
                return "";
        }

        public Int32 GetEmptyPark(String scno, String Flag)//查找空充电位
        {
            String sql_Select = @"Select ws.inline From wcs_shuttleinfo ws Where  ws.type ='PARK' And Flag ='" + Flag + "' And ws.scno ='" + scno + "' And Rownum <=1";
            object ob = ExecuteScalar(CommandType.Text, sql_Select, null);
            if (ob != null)
                return int.Parse(ob.ToString());
            else
                return 0;
        }
        public string GetIsPut(String taskno)//查找空充电位
        {
            String sql_Select = @"select nvl(agvstatus,'0') from wcs_subtaskinfo  where equipmenttype='SC' and  taskno='"+taskno+"'";
            object ob = ExecuteScalar(CommandType.Text, sql_Select, null);
            if (ob != null)
                return ob.ToString();
            else
                return "0";
        }
        //public string GetIsBCtoA()//读取B类烟是否入托盘库
        //{
        //    String sql_Select = @"select field1 from warehouse where warehouseid='8328b8331dbd42599c786684c1e304a3'";
        //    object ob = ExecuteScalar(CommandType.Text, sql_Select, null);
        //    if (ob != null)
        //        return ob.ToString();
        //    else
        //        return "N";
        //}
        public bool UpdateIsBCtoA(string value)
        {
            string str_Update = "update warehouse set field1 = '" + value + "' where warehouseid = '" + WareHouseID + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_Update, null);
            return i > 0 ? true : false;
        }
        public bool UpdateOutrequestProqty(string taskno,string itemcode)
        {
            string barcode = "";
            if (taskno.Length > 4)
            {
                string str_Select = "select barcode  from wcs_taskinfo  where taskno = '" + taskno + "'";
                object ob = ExecuteScalar(CommandType.Text, str_Select, null);
                barcode = ob.ToString();
            }
            else
            {
                barcode = itemcode;
            }        
            string str_Update = "update outrequest set processedqty = processedqty + 1 where containerbarcode = '" + barcode + "'";
            int i = ExecuteNonQuery(CommandType.Text, str_Update, null);
            return i > 0 ? true : false;
        }   
        public Int32 CheckCarry(String scno)//判断当前堆垛机上是否有小车
        {
            String sql_Select = @"Select ws.flag From wcs_shuttleinfo ws Where  ws.type ='SC' And ws.scno ='" + scno + "'";
            object ob = ExecuteScalar(CommandType.Text, sql_Select, null);
            if (ob != null)
                return int.Parse(ob.ToString());
            else
                return 1;
        }

        public DataSet CheckParking(String scno)//判断是否有小车需要充电、所在行
        {
            String temp = "And ws.scno ='" + scno + "'";
            String condition = " order by ws.updatetime";
            String sql_Select = @"Select ws.inline,ws.docommond,ws.taskno,ws.updatetime,ws.scno From wcs_shuttleinfo ws Where ws.type ='SHUTTLE'";
            if (!String.IsNullOrEmpty(scno))
                sql_Select = sql_Select + temp + condition;
            else
                sql_Select = sql_Select + condition;
            return ExecuteDataSet(CommandType.Text, sql_Select, null);
        }

        /// <summary>
        /// 更新充电位状态、小车所在行信息、堆垛机是否载有小车信息
        /// </summary>
        /// <param name="scno"></param>
        /// <param name="line"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool UpdateShuttleInfo(String scno, String line, String taskno, String CmdOrFlag, String Type)
        {
            try
            {
                string scnum = scno.Substring(5, 1);
                scno = "SC" + scnum;
                String sql_UpdateShuttle = @"insert into wcs_shuttleinfo (INLINE, SCNO, TASKNO, DOCOMMOND, UPDATETIME, TYPE, FLAG, WAREHOUSEID)
values ('{0}', '{1}', '{2}', '{3}', sysdate, '{4}', '0', '57274b9c55464753b8bd7a0df96421f2')";

                String sql_DeleteShuttle = @"delete wcs_shuttleinfo ws  where ws.scno ='{1}' and ws.type ='{2}' and ws.inline ='{0}'";

                String sql_UpdateSC = @"Update wcs_shuttleinfo ws set ws.flag ='{0}',ws.updatetime = sysdate where ws.scno ='{1}' and type ='{2}'";

                String sql_UpdatePark = @"Update wcs_shuttleinfo ws set ws.flag ='{0}',ws.updatetime = sysdate where ws.scno ='{1}' and type ='{3}' and inline ='{2}'";
                String sql_Update;
                if (Type.Equals("SC"))
                    sql_Update = String.Format(sql_UpdateSC, CmdOrFlag, scno, Type);
                else if (Type.Equals("PARK"))
                    sql_Update = String.Format(sql_UpdatePark, CmdOrFlag, scno, line, Type);
                else if (!String.IsNullOrEmpty(CmdOrFlag))
                    sql_Update = String.Format(sql_UpdateShuttle, line, scno, taskno, CmdOrFlag, Type);
                else
                    sql_Update = String.Format(sql_DeleteShuttle, line, scno, Type);

                int Flag = ExecuteNonQuery(CommandType.Text, sql_Update, null);
                return Flag > 0 ? true : false;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scno"></param>
        /// <param name="Type">1入2出</param>
        /// <returns></returns>
        public DataSet CheckInOrOutTask(String scno, String Type)
        {
            String TaskType = Type.Equals("1") ? "Task_InBound" : "Task_OutBound";
            String sql_Select = @"Select t.sourceaddresstext,t.targetaddresstext From Task t,Equipment e
Where t.executiveequipmentid = e.equipmentid
And t.warehouseid = e.warehouseid
And t.tasktypecode ='" + TaskType + "'And t.statecode in ('Task_NotExecute','Task_WaitingExecute') and e.equipmentnumber ='" + scno + "'And t.Warehouseid ='57274b9c55464753b8bd7a0df96421f2'";
            return ExecuteDataSet(CommandType.Text, sql_Select, null);
        }

        public DataSet CheckInTask_1073(String taskno)
        {
//            String Check = @"
//select count(1) as qty from wcs_subtaskinfo ws where ws.equipmenttype ='SC' and ws.taskstatus ='0' and ws.fromstation = '1058'";

            String GetInTask = @"
select ws.taskno,ws.fromstation,ws.tostation from wcs_subtaskinfo ws where ws.equipmenttype ='SC' and ws.taskno='"+taskno+"'and ws.taskstatus ='0' and ws.runstatus = '1' and RowNum <= 1";
            //Object obQty = ExecuteScalar(CommandType.Text, Check, null);
            ////if (int.Parse(obQty.ToString()) > 1)//只有当目标站台为1073的入库任务大于1条时才立刻做入库任务
            //if (int.Parse(obQty.ToString()) > 6)//当目标站台1073有入库任务时立刻做入库任务 20140805update 托盘组改为10个后修改
            //{
                return ExecuteDataSet(CommandType.Text, GetInTask, null);
            //}
            //else
            //    return null;
        }


        /// <summary>
        /// 插入条码信息
        /// </summary>
        /// <returns></returns>
        public bool InsertSpInfo(String BarCode, String YBCode, String ItemCode)
        {
            String sqlcount = @"select count(*) from wcs_ybphoto  where CONTAINERBARCODE=" + BarCode;
            String sql = @"
insert into wcs_ybphoto (CONTAINERBARCODE, ITEMBARCODE, ITEMCODE,BINARYIMAGE, CREATETIME, WAREHOUSEID)
values (:BarCode, :YBCode, :ItemCode,null, sysdate, '57274b9c55464753b8bd7a0df96421f2')";

            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(":BarCode",OracleType.VarChar){Value = BarCode},
                 new OracleParameter(":YBCode",OracleType.VarChar){Value = String.IsNullOrEmpty(YBCode)?ItemCode:YBCode},
                   new OracleParameter(":ItemCode",OracleType.VarChar){Value = ItemCode}
            };
            if(ExecuteNonQuery(CommandType.Text,sqlcount,null)==1)
            {
               sql=@"update wcs_ybphoto set ITEMBARCODE=:YBCode,ITEMCODE=:ItemCode,CREATETIME=sysdate where CONTAINERBARCODE=:BarCode";
            }
            int i = ExecuteNonQuery(CommandType.Text, sql, parms);
            return i > 0 ? true : false;
            //return ExecuteNonQuery(CommandType.Text, sql, parms) > 0 ? true : false;
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="YBCode"></param>
        /// <param name="ItemCode"></param>
        /// <param name="Image"></param>
        /// <param name="Flag">1更新图片、2更新物料代码</param>
        /// <returns></returns>
        public bool UpdateSpInfo(String BarCode, String YBCode, String ItemCode, byte[] Image, Int32 Flag)
        {
            String sql_updateimage = @"update wcs_ybphoto set BINARYIMAGE=:Image Where CONTAINERBARCODE =:BarCode";
            String sql_updateitemcode = @"update wcs_ybphoto set ITEMCODE=:ItemCode,ITEMBARCODE=:YBCode Where CONTAINERBARCODE =:BarCode";
            OracleParameter[] parmsimage = new OracleParameter[]
            {
                new OracleParameter(":BarCode",OracleType.VarChar){Value = BarCode},
                     new OracleParameter(":Image",OracleType.Blob){Value = Image},
            };
            OracleParameter[] parmsitemcode = new OracleParameter[]
            {
                  new OracleParameter(":BarCode",OracleType.VarChar){Value = BarCode},
                 new OracleParameter(":YBCode",OracleType.VarChar){Value = String.IsNullOrEmpty(YBCode)?ItemCode:YBCode},
                   new OracleParameter(":ItemCode",OracleType.VarChar){Value = ItemCode}
            };
            return ExecuteNonQuery(CommandType.Text, Flag == 1 ? sql_updateimage : sql_updateitemcode, Flag == 1 ? parmsimage : parmsitemcode) > 0 ? true : false;
        }
        public string[] GetSTATostation(string taskno)
        {
            string[] ss = new string[4];
            OracleParameter[] parms = new OracleParameter[] { new OracleParameter(":WareHouseID", OracleType.VarChar, 50), new OracleParameter(":Taskno", OracleType.VarChar, 50) };
            parms[0].Value = WareHouseID;
            parms[1].Value = taskno;
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            DataSet ds = ExecuteDataSet(CommandType.Text, SQL_SELECT_STATOSTATION, parms);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ss[0] = dt.Rows[0][0].ToString();
                    ss[1] = dt.Rows[0][1].ToString();
                    ss[2] = dt.Rows[0][2].ToString();
                    ss[3] = dt.Rows[0][3].ToString();
                }
                else
                    return null;
            }
            else
                return null;
            return ss;
        }
    }
}
