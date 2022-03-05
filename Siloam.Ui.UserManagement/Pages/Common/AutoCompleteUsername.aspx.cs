using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Siloam.Ui.UserManagement.Pages.Common
{
    public partial class AutoCompleteUsername : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string term = Request.QueryString["term"];

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";

            DataTable dt = new DataTable();
            List<UsernameModel> filtersuggestions = new List<UsernameModel>();

            if (term == "")
            {
                dt = ((DataTable)Session["dataUserUMSfiltered"]).Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UsernameModel x = new UsernameModel();
                    x.user_id = dt.Rows[i]["user_id"].ToString();
                    x.user_name = dt.Rows[i]["user_name"].ToString();
                    x.full_name = dt.Rows[i]["full_name"].ToString();

                    filtersuggestions.Add(x);
                }
            }
            else
            {
                try
                {
                    dt = ((DataTable)Session["dataUserUMSfiltered"]).Select("user_name like '%" + term + "%'").Take(100).CopyToDataTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        UsernameModel x = new UsernameModel();
                        x.user_id = dt.Rows[i]["user_id"].ToString();
                        x.user_name = dt.Rows[i]["user_name"].ToString();
                        x.full_name = dt.Rows[i]["full_name"].ToString();

                        filtersuggestions.Add(x);
                    }
                }
                catch
                {
                    UsernameModel x = new UsernameModel();
                    x.user_id = "";
                    x.user_name = "No Match Found";
                    x.full_name = "";

                    filtersuggestions.Add(x);
                }
            }

            string responseJson = JsonConvert.SerializeObject(filtersuggestions);

            Response.Write(responseJson);
            Response.End();

        }

        public class UsernameModel
        {
            public string user_id;
            public string user_name;
            public string full_name;
        }
    }
}