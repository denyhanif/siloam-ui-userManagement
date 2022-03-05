using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class XOrgAX
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Result_Data_XorgAX
    {
        private List<XOrgAX> lists = new List<XOrgAX>();
        [JsonProperty("data")]
        public List<XOrgAX> list { get { return lists; } }
    }
}