using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewLoginPage
    {
        public Guid role_id { get; set; }
        public string role_name { get; set; }
        public Guid page_id { get; set; }
        public string page_name { get; set; }
    }

    public class Result_Data_LoginPage
    {
        private List<ViewLoginPage> lists = new List<ViewLoginPage>();
        [JsonProperty("data")]
        public List<ViewLoginPage> list { get { return lists; } }
    }
}