using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewSimpleMapping
    {
        public Int64 user_role_id { get; set; }
        public Guid application_id { get; set; }
        public string application_name { get; set; }
        public Guid role_id { get; set; }
        public string role_name { get; set; }
        public bool is_active { get; set; }
    }

    public class ViewSimpleOrg
    {
        public Int64 organization_id { get; set; }
        public string organization_name { get; set; }
    }

    public class Result_Data_SImpleOrg
    {
        private List<ViewSimpleOrg> lists = new List<ViewSimpleOrg>();
        [JsonProperty("data")]
        public List<ViewSimpleOrg> list { get { return lists; } }
    }

    public class Result_Data_SImpleMapping
    {
        private List<ViewSimpleMapping> lists = new List<ViewSimpleMapping>();
        [JsonProperty("data")]
        public List<ViewSimpleMapping> list { get { return lists; } }
    }
}