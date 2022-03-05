using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewLoginUser
    {
        public Int64 organization_id { get; set; }
        public Int64 hope_organization_id { get; set; }
        public Guid mobile_organization_id { get; set; }
        public string ax_organization_id { get; set; }

        public Guid role_id { get; set; }
        public string role_name { get; set; }

        public Guid user_id { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        public Int64 hope_user_id { get; set; }
        public string email { get; set; }
        public Nullable<DateTime> birthday { get; set; }
        public string hp { get; set; }

        public Int64 user_role_id { get; set; }
    }

    public class Result_login_user
    {
        private List<ViewLoginUser> lists = new List<ViewLoginUser>();
        [JsonProperty("data")]
        public List<ViewLoginUser> list { get { return lists; } }
    }
}