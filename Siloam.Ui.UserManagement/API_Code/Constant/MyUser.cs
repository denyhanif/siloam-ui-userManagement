using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Siloam.Ui.UserManagement.Pages.Common;
using Siloam.Ui.UserManagement.API_Code.Models;

/// <summary>
/// Summary description for MyUser
/// </summary>
public class MyUser
{
    //public static PharmacyCentralLoginModel Data { get; set; }

    //private static Login Data;
    public static string GetUsername()
    {
        if (HttpContext.Current.Session[Helper.Session_DataLogin] != null)
        {
            DataTable Data = (DataTable)HttpContext.Current.Session[Helper.Session_DataLogin];
            if (Data != null)
            {
                return Data.Rows[0]["user_name"].ToString();
            }
            else
            {
                return "";
            }
            
        }
        else
        {
            return "";
        }
    }

    public static string GetOrgId()
    {
        if (HttpContext.Current.Session[Helper.Session_DataLogin] != null)
        {
            DataTable Data = (DataTable)HttpContext.Current.Session[Helper.Session_DataLogin];
            if (Data != null)
            {
                return Data.Rows[0]["organization_id"].ToString();
            }
            else
            {
                return "-1";
            }

        }
        else
        {
            return "-1";
        }
    }
}