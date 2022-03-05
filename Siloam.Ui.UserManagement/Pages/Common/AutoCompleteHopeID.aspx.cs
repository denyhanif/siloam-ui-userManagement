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
    public partial class AutoCompleteHopeID : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string term = Request.QueryString["term"];

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";

            DataTable dt = new DataTable();
            List<HopeIDModel> filtersuggestions = new List<HopeIDModel>();

            if (term == "")
            {
                dt = ((DataTable)Session["dataUserHope"]).Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HopeIDModel x = new HopeIDModel();
                    x.userId = dt.Rows[i]["userId"].ToString();
                    x.userName = dt.Rows[i]["userName"].ToString();
                    x.name = dt.Rows[i]["name"].ToString();

                    filtersuggestions.Add(x);
                }
            }
            else
            {
                try
                {
                    dt = ((DataTable)Session["dataUserHope"]).Select("userName like '%" + term + "%'").Take(100).CopyToDataTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        HopeIDModel x = new HopeIDModel();
                        x.userId = dt.Rows[i]["userId"].ToString();
                        x.userName = dt.Rows[i]["userName"].ToString();
                        x.name = dt.Rows[i]["name"].ToString();

                        filtersuggestions.Add(x);
                    }
                }
                catch
                {
                    HopeIDModel x = new HopeIDModel();
                    x.userId = "No Match Found";
                    x.userName = "";
                    x.name = "";

                    filtersuggestions.Add(x);
                }
            }

            string responseJson = JsonConvert.SerializeObject(filtersuggestions);

            Response.Write(responseJson);
            Response.End();

        }

        public class HopeIDModel
        {
            public string userId;
            public string userName;
            public string name;
        }
    }
}