using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class User
    {
        public Guid user_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
        public Int64? hope_user_id { get; set; }
        public string email { get; set; }
        public Nullable<DateTime> birthday { get; set; }
        public string handphone { get; set; }
        public Byte lock_counter { get; set; }
        public Nullable<DateTime> last_login_date { get; set; }
        public Nullable<DateTime> exp_pass_date { get; set; }
        public bool is_internal { get; set; }
        public bool is_ad { get; set; }
        public bool is_proint { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public Nullable<DateTime> modified_date { get; set; }
    }

    public class Result_Data_user
    {
        private List<User> lists = new List<User>();
        [JsonProperty("data")]
        public List<User> list { get { return lists; } }
    }
}