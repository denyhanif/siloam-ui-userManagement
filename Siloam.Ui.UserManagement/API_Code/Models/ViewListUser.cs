using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewListUser
    {
        public Int64 user_role_id { get; set; }
        public Guid user_id { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
    }

    public class Result_Data_User_List
    {
        private List<ViewListUser> lists = new List<ViewListUser>();
        [JsonProperty("data")]
        public List<ViewListUser> list { get { return lists; } }
    }
}