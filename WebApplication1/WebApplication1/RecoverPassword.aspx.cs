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
    public partial class RecoverPassword : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=GADIR\SQLEXPRESS;Initial Catalog=ResponsiveWebsite;Integrated Security=True");
        String Guid;
        DataTable dataTable = new DataTable();
        int Uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            Guid = Request.QueryString["Uid"];
            if (Guid != null)
            {
                SqlCommand sqlCommand = new SqlCommand("ForgotPassRequestsGetByGuid", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ID", Guid);
                sqlCommand.ExecuteNonQuery();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count != 0)
                {
                    Uid = Convert.ToInt32(dataTable.Rows[0][1]);
                }
                else
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Your Password Reset Link is Expired or Invalid!";
                }
            }
            else
            {
                Response.Redirect("~/SignIn.aspx");
            }
            if (!IsPostBack)
            {
                if(dataTable.Rows.Count != 0)
                {
                    tbNewPass.Visible = true;
                    tbConfirmPass.Visible = true;
                    lblPassword.Visible = true;
                    lblRetypePass.Visible = true;
                    btRecPass.Visible = true;
                }
                else
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Your Password Reset Link is Expired or Invalid!";
                }
            }
        }

        protected void btRecPass_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            if (tbNewPass.Text == tbConfirmPass.Text && tbNewPass.Text != "" && tbConfirmPass.Text != "")
            {
                SqlCommand sqlCommand = new SqlCommand("UsersUpdatePassword", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Uid", Uid);
                sqlCommand.Parameters.AddWithValue("@Password", tbNewPass.Text.Trim());
                sqlCommand.ExecuteNonQuery();
                SqlCommand sqlCommand1 = new SqlCommand("ForgotPassRequestsDeleteByUid", sqlConnection);
                sqlCommand1.CommandType = CommandType.StoredProcedure;
                sqlCommand1.Parameters.AddWithValue("@Uid", Uid);
                sqlCommand1.ExecuteNonQuery();
                sqlConnection.Close();
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}