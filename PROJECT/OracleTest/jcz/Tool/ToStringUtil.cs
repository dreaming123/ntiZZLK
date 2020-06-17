using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace OracleTest.jcz.Tool
{
    public partial class ToStringUtil
    {

        public override string ToString()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            int num = 1;
            settings.NullValueHandling = (NullValueHandling)num;
            return JsonConvert.SerializeObject((object)this, Formatting.Indented, settings);
        }
    }
}
