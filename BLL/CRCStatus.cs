using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class CRCStatus
    {
        private string crcnum;//卫星小车编号
        private string cranemode;//卫星小车模式
        private string alarminfo;//报警信息
        private string alarmmessage;//报警信息（翻译）
        private int zalarm;//总报警
        private bool change;//信息是否有变化
        private string oldlabelinfo;//上一次信息
        private string labelinfo;//显示信息
        private string crcstatus;//卫星小车状态(Equipment_OK可用,Equipment_Error不可用)
        private string crctaskflag = "0";//获取任务标记(0不可获取,1可获取非转轨任务,2可获取转轨任务)
        private string taskno = "0";//任务号
        private string fromstation;//起始地址
        private string tostation;//目标地址
        private string statostation;//输送机目标地址
        private string position;//当前位置
        private string functionreport;//函数类型 低位
        private string functionreporth;//函数类型 高位
        private string functionmode;//函数模式
        private string actionpoint;//输送机地址编号
        private string aislenumber;//巷道号 01--09
        private string rackside;//巷道左右边 01左 02右
        private string rackposition;//列值 001--063
        private string rackplace;//货位内左右/1,2
        private string racklevel="0";//层值 01--08
        private string rackdepth;//深度 01--02
        private string tasktype;//任务类型
        //private string field3;//最后一垛标识,null--1

        public string CRCNum
        {
            set { crcnum = value; }
            get { return crcnum; }
        }
        public string CraneMode
        {
            get { return cranemode; }
            set { cranemode = value; }
        }
        public string AlarmInfo
        {
            get { return alarminfo; }
            set { alarminfo = value; }
        }
        public string AlarmMessage
        {
            get { return alarmmessage; }
            set { alarmmessage = value; }
        }
        public int ZAlarm
        {
            get { return zalarm; }
            set { zalarm = value; }
        }
        public bool Change
        {
            get { return change; }
            set { change = value; }
        }
        public string OldLabelInfo
        {
            get { return oldlabelinfo; }
            set { oldlabelinfo = value; }
        }
        public string LabelInfo
        {
            get { return labelinfo; }
            set { labelinfo = value; }
        }
        public string CrcStatus
        {
            get { return crcstatus; }
            set { crcstatus = value; }
        }
        public string CrcTaskFlag
        {
            get { return crctaskflag; }
            set { crctaskflag = value; }
        }
        public string TaskNo
        {
            get { return taskno; }
            set { taskno = value; }
        }
        public string Fromstation
        {
            get { return fromstation; }
            set { fromstation = value; }
        }
        public string Tostation
        {
            get { return tostation; }
            set { tostation = value; }
        }
        public string STAToStation
        {
            get { return statostation; }
            set { statostation = value; }
        }
        public string Position
        {
            get { return position; }
            set { position = value; }
        }
        public string FunctionReport
        {
            get { return functionreport; }
            set { functionreport = value; }
        }
        public string FunctionReportH
        {
            get { return functionreporth; }
            set { functionreporth = value; }
        }
        public string FunctionMode
        {
            get { return functionmode; }
            set { functionmode = value; }
        }
        public string ActionPoint
        {
            get { return actionpoint; }
            set { actionpoint = value; }
        }
        public string AisleNumber
        {
            get { return aislenumber; }
            set { aislenumber = value; }
        }
        public string RackSide
        {
            get { return rackside; }
            set { rackside = value; }
        }
        public string RackPosition
        {
            get { return rackposition; }
            set { rackposition = value; }
        }
        public string RackPlace
        {
            get { return rackplace; }
            set { rackplace = value; }
        }
        public string RackLevel
        {
            get { return racklevel; }
            set { racklevel = value; }
        }
        public string RackDepth
        {
            get { return rackdepth; }
            set { rackdepth = value; }
        }
        public string TaskType
        {
            get { return tasktype; }
            set { tasktype = value; }
        }
        //public string Field3
        //{
        //    get { return field3; }
        //    set { field3 = value; }
        //}
    }
}
