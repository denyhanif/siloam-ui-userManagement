using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class XOrgHOPE
    {
        public Int64 organizationId { get; set; }
        public string name { get; set;  }
    }

    public class Result_Data_XorgHope
    {
        private List<XOrgHOPE> lists = new List<XOrgHOPE>();
        [JsonProperty("data")]
        public List<XOrgHOPE> list { get { return lists; } }
    }
}