using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class XOrgMOBILE
    {
      public Guid? hospital_id { get; set; }
      public string name { get; set; }
    }

    public class Result_Data_XorgMobile
    {
        private List<XOrgMOBILE> lists = new List<XOrgMOBILE>();
        [JsonProperty("data")]
        public List<XOrgMOBILE> list { get { return lists; } }
    }
}
