using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class UserTicketing
    {
        public long user_ticketing_id { get; set; }
        public Boolean is_internal { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string handphone { get; set; }
        public Guid application_id { get; set; }
        public long hope_organization_id { get; set; }
        public Guid mobile_organization_id { get; set; }
        public string ax_organization_id { get; set; }
        public Guid role_id { get; set; }
        public Boolean is_rejected { get; set; }
        public Boolean is_validated { get; set; }
        public DateTime? created_date { get; set; }
        public string remark { get; set; }
        public DateTime? rejected_date { get; set; }
        public string rejected_by { get; set; }
        public DateTime? validated_date { get; set; }
        public string validated_by { get; set; }
    }

    public class Result_Data_UserTicketing
    {
        private List<UserTicketing> lists = new List<UserTicketing>();
        [JsonProperty("data")]
        public List<UserTicketing> list { get { return lists; } }
    }
}