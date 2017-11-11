using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERNAMEADMIN"] != null)
            {
                BtAdminLogout.Visible = true;
                btnSignIn.Visible = false;
            }
            else
            {
                BtAdminLogout.Visible = false;
                btnSignIn.Visible = true;
                Response.Redirect("~/SignIn.aspx");
            }
        }
        protected void BtAdminLogout_Click(object sender, EventArgs e)
        {
            Session["USERNAME"] = null;
            Response.Redirect("~/Default.aspx");
        }
        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SignIn.aspx");
        }
    }
}