using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewListRole
    {
        public Guid role_id { get; set; }
        public string role_name { get; set; }
        public bool is_active { get; set; }
    }

    public class Result_Data_Role_List
    {
        private List<ViewListRole> lists = new List<ViewListRole>();
        [JsonProperty("data")]
        public List<ViewListRole> list { get { return lists; } }
    }
}