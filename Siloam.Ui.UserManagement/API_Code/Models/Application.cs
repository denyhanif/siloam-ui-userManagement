using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class Application
    {
        public Guid application_id { get; set; }
        public string application_name { get; set; }
        public string url { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
        public string modified_by { get; set; }
        public Nullable<DateTime> modified_date { get; set; }
    }

    public class Result_Data_Application
    {
        private List<Application> lists = new List<Application>();
        [JsonProperty("data")]
        public List<Application> list { get { return lists; } }
    }
}