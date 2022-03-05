using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class UserRole
    {
        public Int64 user_role_id { get; set; }
        public Guid user_id { get; set; }
        public Int64 organization_id { get; set; }
        public Guid application_id { get; set; }
        public Guid role_id { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
        public string modified_by { get; set; }
        public Nullable<DateTime> modified_date { get; set; }
    }

    public class Result_Data_UserRole
    {
        private List<UserRole> lists = new List<UserRole>();
        [JsonProperty("data")]
        public List<UserRole> list { get { return lists; } }
    }
}