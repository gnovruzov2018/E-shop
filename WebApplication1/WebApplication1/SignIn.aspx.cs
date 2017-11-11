using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebApplication1
{
    public partial class SignIn : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=GADIR\SQLEXPRESS;Initial Catalog=ResponsiveWebsite;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.Cookies["UNAME"] != null && Request.Cookies["PWD"] != null)
            {
                UserName.Text = Request.Cookies["UNAME"].Value;
                Password.Attributes["value"] = Request.Cookies["PWD"].Value;
                CheckBox1.Checked = true;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("UsersCheckLogin", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", UserName.Text.Trim());
            sqlCommand.Parameters.AddWithValue("@Password", Password.Text.Trim());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if(dataTable.Rows.Count != 0)
            {
                if (CheckBox1.Checked)
                {
                    Response.Cookies["UNAME"].Value = UserName.Text;
                    Response.Cookies["PWD"].Value = Password.Text;

                    Response.Cookies["UNAME"].Expires = DateTime.Now.AddDays(10);
                    Response.Cookies["PWD"].Expires = DateTime.Now.AddDays(10);
                }
                else
                {
                    Response.Cookies["UNAME"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["PWD"].Expires = DateTime.Now.AddDays(-1);
                }
                String UserType = dataTable.Rows[0][5].ToString().Trim();
                if (UserType == "A")
                {
                    Session["USERNAMEADMIN"] = UserName.Text;
                    Response.Redirect("~/AdminHome.aspx");
                }
                else
                {
                    Session["USERNAME"] = UserName.Text;
                    if (Request.QueryString["rurl"] != null)
                    {
                        String page = Request.QueryString["rurl"];
                        Response.Redirect("~/"+page+".aspx");
                    }
                    else
                    {
                        Response.Redirect("~/UserHome.aspx");
                    }
                }
            }
            else
            {
                lblError.Text = "Invalid Username or Password!";
            }
        }
    }
}