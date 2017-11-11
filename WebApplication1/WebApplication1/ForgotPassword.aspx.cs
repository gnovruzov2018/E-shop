using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace WebApplication1
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=GADIR\SQLEXPRESS;Initial Catalog=ResponsiveWebsite;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btPassRec_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("UsersCheckEmail", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Email", tbEmailId.Text.Trim());
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if(dataTable.Rows.Count != 0)
            {
                String myGUID = Guid.NewGuid().ToString();
                int Uid = Convert.ToInt32(dataTable.Rows[0][0]);
                SqlCommand sqlCommand1 = new SqlCommand("ForgotPassRequestsCreate", sqlConnection);
                sqlCommand1.CommandType = CommandType.StoredProcedure;
                sqlCommand1.Parameters.AddWithValue("@ID", myGUID);
                sqlCommand1.Parameters.AddWithValue("@Uid", Uid);
                sqlCommand1.Parameters.AddWithValue("@RequestDateTime", DateTime.Now);
                sqlCommand1.ExecuteNonQuery();

                //Send Mail

                String toEmailAddress = dataTable.Rows[0][3].ToString();
                String username = dataTable.Rows[0][1].ToString();
                String messageBody = "Hi, dear " + username + ". <br/> <br/> Please, click the link below in order to reset your password. <br/><br/> http://localhost:63986/RecoverPassword.aspx?Uid="+myGUID;
                MailMessage mailMessage = new MailMessage("youremail@address.com", toEmailAddress);
                mailMessage.Body = messageBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Reset Password";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential("email", "password");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                lblPassRec.Text = "Check Your Email to Reset Your Password!";
                lblPassRec.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblPassRec.Text = "OOps This Email Doesn't Exist in Our Database !";
                lblPassRec.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}