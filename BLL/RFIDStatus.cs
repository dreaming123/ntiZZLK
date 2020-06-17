using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RFIDStatus
    {
        private int upBoxNo;//上层烟箱编号

        public int UpBoxNo
        {
            get { return upBoxNo; }
            set { upBoxNo = value; }
        }
        private int upBoxType;//上层烟丝类型

        public int UpBoxType
        {
            get { return upBoxType; }
            set { upBoxType = value; }
        }
        private int upBoxStatus;//上层烟箱状态(1:空烟箱烟箱,2:实烟箱)

        public int UpBoxStatus
        {
            get { return upBoxStatus; }
            set { upBoxStatus = value; }
        }
        private float upBoxInitialweight;//上层空箱初始化重量

        public float UpBoxInitialweight
        {
            get { return upBoxInitialweight; }
            set { upBoxInitialweight = value; }
        }
        private float upBoxActualBinweight;//上层空箱实际重量

        public float UpBoxActualBinweight
        {
            get { return upBoxActualBinweight; }
            set { upBoxActualBinweight = value; }
        }
        private string upBatchID;//上层烟箱批次号

        public string UpBatchID
        {
            get { return upBatchID; }
            set { upBatchID = value; }
        }
        private string upBlendCode;//上层烟箱牌号

        public string UpBlendCode
        {
            get { return upBlendCode; }
            set { upBlendCode = value; }
        }
        private float upTobaccoweight;//上层烟丝净重

        public float UpTobaccoweight
        {
            get { return upTobaccoweight; }
            set { upTobaccoweight = value; }
        }
        private int downBoxNo;//下层烟箱编号

        public int DownBoxNo
        {
            get { return downBoxNo; }
            set { downBoxNo = value; }
        }

        private int downBoxType;//下层烟丝类型

        public int DownBoxType
        {
            get { return downBoxType; }
            set { downBoxType = value; }
        }
        private int downBoxStatus;//下层烟箱状态

        public int DownBoxStatus
        {
            get { return downBoxStatus; }
            set { downBoxStatus = value; }
        }
        private float downBoxInitialweight;//下层空箱初始重量

        public float DownBoxInitialweight
        {
            get { return downBoxInitialweight; }
            set { downBoxInitialweight = value; }
        }
        private float downBoxActualBinweight;//下层空箱实际重量

        public float DownBoxActualBinweight
        {
            get { return downBoxActualBinweight; }
            set { downBoxActualBinweight = value; }
        }
        private string downBatchID;//下层烟箱批次

        public string DownBatchID
        {
            get { return downBatchID; }
            set { downBatchID = value; }
        }
        private string downBlendCode;//下层烟箱牌号

        public string DownBlendCode
        {
            get { return downBlendCode; }
            set { downBlendCode = value; }
        }
        private float downTobaccoweight;//下层烟丝净重

        public float DownTobaccoweight
        {
            get { return downTobaccoweight; }
            set { downTobaccoweight = value; }
        }

       
    }
}
