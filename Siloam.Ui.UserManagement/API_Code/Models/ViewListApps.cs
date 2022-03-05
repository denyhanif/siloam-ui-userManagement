using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewListApps
    {
        public Guid application_id { get; set; }
        public string application_name { get; set; }
    }

    public class Result_Data_Apps_List
    {
        private List<ViewListApps> lists = new List<ViewListApps>();
        [JsonProperty("data")]
        public List<ViewListApps> list { get { return lists; } }
    }
}