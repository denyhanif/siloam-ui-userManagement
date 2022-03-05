using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class DoctorSIP
    {
        public Guid doctor_sip_id { get; set; }
        public Guid user_id { get; set; }
        public long organization_id { get; set; }
        public string organization_name { get; set; }
        public string sip_no { get; set; }
        public int is_delete { get; set; }
    }

    public class ResultSIP
    {
        private List<DoctorSIP> lists = new List<DoctorSIP>();
        [JsonProperty("data")]
        public List<DoctorSIP> list { get { return lists; } }
    }
}