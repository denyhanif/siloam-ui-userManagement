using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ParamChangePass
    {
        public string user_name { get; set; }
        public string old_pass { get; set; }
        public string new_pass { get; set; }
        public string modified_by { get; set; }
    }
}