using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ParamLoginUser
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public Guid application_id { get; set; }
    }
}