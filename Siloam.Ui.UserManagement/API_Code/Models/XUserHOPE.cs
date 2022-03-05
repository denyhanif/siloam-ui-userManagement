using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class XUserHOPE
    {
        public Int64 userId { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
    }

    public class Result_Data_XUserHope
    {
        private List<XUserHOPE> lists = new List<XUserHOPE>();
        [JsonProperty("data")]
        public List<XUserHOPE> list { get { return lists; } }
    }
}