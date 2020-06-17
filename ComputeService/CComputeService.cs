using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compute;
using DataAccess;
using System.ServiceModel;

namespace ComputeService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]

    public class CComputeService : ICompute
    {
        private WCFDATA dal = new WCFDATA();
        public delegate void Notify(int type, string robotlineID, object ob);
        public static event Notify eNotify;

        private void sNotify(int type, string robotlineID, object ob)
        {
            if (eNotify != null)
                eNotify(type, robotlineID, ob);
        }

        #region ICompute 成员

        public string WCS_I_CreateOutBoudTaskCP_P(string wmstaskno, string sourcetext, string robotlineid, string barcode)
        {
            return dal.WCS_I_CreateOutBoudTaskCP_P(wmstaskno, sourcetext, robotlineid, barcode);
        }

        public void WCS_I_BufferToPick_P(string robotlineID, string barcode)
        {
            int[] value = dal.WCS_I_TOPICKSTATION_P(1, robotlineID, barcode);
            sNotify(1, robotlineID, value);
        }

        public void WCS_I_DisToPick_P(string robotlineID, string barcode)
        {
            int[] value = dal.WCS_I_TOPICKSTATION_P(2, robotlineID, barcode);
            sNotify(2, robotlineID, value);
        }

        public void ConnectCheck()
        {
        }

        public int WCS_I_PickCount_P(string robotLineID, int count)
        {
            int[] value = new int[2] { int.Parse(robotLineID), count };
            sNotify(4, robotLineID, value);
            return 0;
        }

        public int WCS_I_ResidualBack_P(string robotLineID, string barcode)
        {
            throw new NotImplementedException();
        }

        public int WCS_I_PalletBack_P(String robotLineID, string barcode)
        {
            throw new NotImplementedException();
        }

        public void WCS_I_PalletToDis_P(string robotLineID, string barcode)
        {
            int[] value = dal.WCS_I_TOPICKSTATION_P(2, robotLineID, barcode);
            sNotify(3, robotLineID, value);
        }

        #endregion
    }
}
