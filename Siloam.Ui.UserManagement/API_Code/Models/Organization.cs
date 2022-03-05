using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class Organization
    {
        public Int64 organization_id { get; set; }
        public string organization_name { get; set; }
        public Int64? hope_organization_id { get; set; }
        public Guid? mobile_organization_id { get; set; }
        public string ax_organization_id { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
        public string modified_by { get; set; }
        public Nullable<DateTime> modified_date { get; set; }
    }

    public class Result_Data_Organization
    {
        private List<Organization> lists = new List<Organization>();
        [JsonProperty("data")]
        public List<Organization> list { get { return lists; } }
    }
}