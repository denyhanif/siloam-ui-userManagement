using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siloam.Ui.UserManagement.API_Code.Models
{
    public class ViewUserSIP
    {
        public User model_user { get; set; }
        public List<DoctorSIP> model_sip { get; set; }
    }
}