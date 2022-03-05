using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class Pagee
    {
        public Guid page_id { get; set; }
        public Guid application_id { get; set; }
        public string page_name { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
        public string modified_by { get; set; }
        public Nullable<DateTime> modified_date { get; set; }
    }

    public class Result_Data_Page
    {
        private List<Pagee> lists = new List<Pagee>();
        [JsonProperty("data")]
        public List<Pagee> list { get { return lists; } }
    }
}