using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewListPage
    {
        public Int64 role_access_id { get; set; }
        public Guid page_id { get; set; }
        public string page_name { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<DateTime> created_date { get; set; }
    }

    public class Result_Data_Page_List
    {
        private List<ViewListPage> lists = new List<ViewListPage>();
        [JsonProperty("data")]
        public List<ViewListPage> list { get { return lists; } }
    }
}