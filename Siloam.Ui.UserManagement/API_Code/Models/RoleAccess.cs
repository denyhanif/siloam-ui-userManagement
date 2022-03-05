using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class RoleAccess
    {
        public Int64 role_access_id { get; set; }
        public Guid role_id { get; set; }
        public Guid page_id { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
        public string modified_by { get; set; }
        public Nullable<DateTime> modified_date { get; set; }
    }

    public class Result_Data_RoleAccess
    {
        private List<RoleAccess> lists = new List<RoleAccess>();
        [JsonProperty("data")]
        public List<RoleAccess> list { get { return lists; } }
    }
}