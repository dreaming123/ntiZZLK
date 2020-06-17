using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace DataAccess
{
    public class WCFDATA : DALBase
    {
        public string WCS_I_CreateOutBoudTaskCP_P(string wmstaskno, string sourcetext, string robotlineID, string barcode)
        {
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid",OracleType.VarChar,50),
                new OracleParameter(@"i_wmstaskno",OracleType.VarChar,20),
                new OracleParameter(@"i_sourcetext",OracleType.VarChar,20),
                new OracleParameter(@"i_robotLineID",OracleType.VarChar,10),
                new OracleParameter(@"i_barcode",OracleType.VarChar,50),
                new OracleParameter(@"o_result",OracleType.Int16)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = wmstaskno;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = sourcetext;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = robotlineID;
            parms[4].Direction = ParameterDirection.Input;
            parms[4].Value = barcode;
            parms[5].Direction = ParameterDirection.Output;
            parms[5].Value = 0;
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_I_CreateOutBoudTaskCP_P", parms);
            return str[5];
        }

        public int[] WCS_I_TOPICKSTATION_P(int type, string robotlineID, string barcode)
        {
            int[] value = new int[2] { 0, 0 };
            OracleParameter[] parms = new OracleParameter[]
            {
                new OracleParameter(@"i_warehouseid",OracleType.VarChar,50),
                new OracleParameter(@"i_type",OracleType.Int16),
                new OracleParameter(@"i_lane",OracleType.VarChar,10),
                new OracleParameter(@"i_barcode",OracleType.VarChar,50),
                new OracleParameter(@"o_from",OracleType.VarChar,10),
                new OracleParameter(@"o_to",OracleType.VarChar,10)
            };
            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = WareHouseID;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = type;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = robotlineID;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = barcode;
            parms[4].Direction = ParameterDirection.Output;
            parms[4].Value = "0";
            parms[5].Direction = ParameterDirection.Output;
            parms[5].Value = "0";
            string[] str = ExecuteProcedure(CommandType.StoredProcedure, "WCS_I_TOPICKSTATION_P", parms);
            if (str != null)
            {
                value[0] = int.Parse(str[4]);
                value[1] = int.Parse(str[5]);
            }
            return value;
        }
    }
}
