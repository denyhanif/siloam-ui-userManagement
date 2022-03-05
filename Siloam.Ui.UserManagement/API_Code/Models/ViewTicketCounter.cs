using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewTicketCounter
    {
        public Int64 organization_id { get; set; }
        public string organization_name { get; set; }
        public int counter { get; set; }
    }

    public class Result_Data_TicketCount
    {
        private List<ViewTicketCounter> lists = new List<ViewTicketCounter>();
        [JsonProperty("data")]
        public List<ViewTicketCounter> list { get { return lists; } }
    }
}