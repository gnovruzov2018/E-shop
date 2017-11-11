using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SignUp : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=GADIR\SQLEXPRESS;Initial Catalog=ResponsiveWebsite;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSignup_Click(object sender, EventArgs e)
        {
            if (tbUname.Text != "" && tbPass.Text != "" && tbName.Text != "" & tbEmail.Text != "" && tbCPass.Text != "")
            {
                if (tbCPass.Text == tbPass.Text)
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("UsersCreate", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Username", tbUname.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@Password", tbPass.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@Name", tbName.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@Utype", "U");
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    tbCPass.Text = tbEmail.Text = tbPass.Text = tbUname.Text = tbName.Text = "";
                    lblMsg.Text = "Registration Successfull!";
                }
                else
                {
                    tbPass.Text = tbCPass.Text = "";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Passwords Don't Match!";
                }
            }
            else
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "All Fields Are Mandatory!";
            }
            
        }

    }
}