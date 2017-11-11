using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

namespace WebApplication1
{
    public partial class AddProduct : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=GADIR\SQLEXPRESS;Initial Catalog=ResponsiveWebsite;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBrands();
                BindCategories();
                BindGenders();
                ddlSubCategory.Enabled = false;
                ddlGender.Enabled = false;
            }
        }
        private void BindBrands()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("BrandsSelect", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count != 0)
            {
                ddlBrands.DataSource = dataTable;
                ddlBrands.DataTextField = "Name";
                ddlBrands.DataValueField = "BrandID";
                ddlBrands.DataBind();
                ddlBrands.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        private void BindCategories()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("CategoriesSelect", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count != 0)
            {
                ddlCategory.DataSource = dataTable;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        private void BindGenders()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("GendersSelect", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count != 0)
            {
                ddlGender.DataSource = dataTable;
                ddlGender.DataTextField = "Gender";
                ddlGender.DataValueField = "GenderID";
                ddlGender.DataBind();
                ddlGender.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int mainCategoryId = Convert.ToInt32(ddlCategory.SelectedItem.Value);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SubCategoriesSelectByCategoryID", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CategoryID", mainCategoryId);
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count != 0)
            {
                ddlSubCategory.DataSource = dataTable;
                ddlSubCategory.DataTextField = "SubCategoryName";
                ddlSubCategory.DataValueField = "SubCategoryID";
                ddlSubCategory.DataBind();
                ddlSubCategory.Items.Insert(0, new ListItem("-- Select --", "0"));
                ddlSubCategory.Enabled = true;
            }
        }
        protected void ddlGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SizesSelectByForeignIDs", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@BrandID", ddlBrands.SelectedItem.Value);
            sqlCommand.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedItem.Value);
            sqlCommand.Parameters.AddWithValue("@SubCategoryID", ddlSubCategory.SelectedItem.Value);
            sqlCommand.Parameters.AddWithValue("@GenderID", ddlGender.SelectedItem.Value);
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count != 0)
            {
                cblSize.DataSource = dataTable;
                cblSize.DataTextField = "SizeName";
                cblSize.DataValueField = "SizeID";
                cblSize.DataBind();

            }
        }
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubCategory.SelectedIndex != 0)
            {
                ddlGender.Enabled = true;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtPName.Text != "" && txtPrice.Text != "" && txtQuantity.Text != "" && txtSelPrice.Text != "" && txtDesc.Text != "" && txtMatCare.Text != "" &&
                txtPDetails.Text != "" && ddlBrands.SelectedIndex != 0 && ddlCategory.SelectedIndex != 0 && ddlGender.SelectedIndex != 0 &&
                ddlSubCategory.SelectedIndex != 0 && fuImg01.HasFile && fuImg02.HasFile && fuImg03.HasFile && fuImg04.HasFile && fuImg05.HasFile)
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("ProductsCreate", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Name", txtPName.Text.Trim());
                sqlCommand.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
                sqlCommand.Parameters.AddWithValue("@SelPrice", txtSelPrice.Text.Trim());
                sqlCommand.Parameters.AddWithValue("@BrandID", ddlBrands.SelectedItem.Value);
                sqlCommand.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedItem.Value);
                sqlCommand.Parameters.AddWithValue("@SubCategoryID", ddlSubCategory.SelectedItem.Value);
                sqlCommand.Parameters.AddWithValue("@GenderID", ddlGender.SelectedItem.Value);
                sqlCommand.Parameters.AddWithValue("@Description", txtDesc.Text.Trim());
                sqlCommand.Parameters.AddWithValue("@Details", txtPDetails.Text.Trim());
                sqlCommand.Parameters.AddWithValue("@MaterialCare", txtMatCare.Text.Trim());
                if (cbFD.Checked == true)
                {
                    sqlCommand.Parameters.AddWithValue("@FreeDelivery", 1);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@FreeDelivery", 0);
                }
                if (cb30Ret.Checked == true)
                {
                    sqlCommand.Parameters.AddWithValue("@ThirtyDayRet", 1);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@ThirtyDayRet", 0);
                }
                if (cbCOD.Checked == true)
                {
                    sqlCommand.Parameters.AddWithValue("@COD", 1);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@COD", 0);
                }
                sqlCommand.ExecuteNonQuery();

                SqlCommand sqlCommand2 = new SqlCommand("ProductLastInsertionID", sqlConnection);
                sqlCommand2.CommandType = CommandType.StoredProcedure;
                Int32 lastInsertedID = Convert.ToInt32(sqlCommand2.ExecuteScalar());

                //Insert Size Quantity

                for (int i = 0; i < cblSize.Items.Count; i++)
                {
                    if (cblSize.Items[i].Selected = true)
                    {
                        Int64 sizeID = Convert.ToInt64(cblSize.Items[i].Value);
                        int quantity = Convert.ToInt32(txtQuantity.Text);
                        SqlCommand sqlCommand1 = new SqlCommand("SizeQuantitesCreate", sqlConnection);
                        sqlCommand1.CommandType = CommandType.StoredProcedure;
                        sqlCommand1.Parameters.AddWithValue("@ProductID", lastInsertedID);
                        sqlCommand1.Parameters.AddWithValue("@SizeID", sizeID);
                        sqlCommand1.Parameters.AddWithValue("@Quantity", quantity);
                        sqlCommand1.ExecuteNonQuery();
                    }
                }

                //Insert Images

                if (fuImg01.HasFile)
                {
                    string savePath = Server.MapPath("~/Images/ProductImages/") + lastInsertedID;
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string extention = Path.GetExtension(fuImg01.PostedFile.FileName);
                    fuImg01.SaveAs(savePath + "\\" + txtPName.Text.ToString().Trim() + "01"+extention);
                    SqlCommand sqlCommand3 = new SqlCommand("ImagesCreate", sqlConnection);
                    sqlCommand3.CommandType = CommandType.StoredProcedure;
                    sqlCommand3.Parameters.AddWithValue("@ProductID", lastInsertedID);
                    sqlCommand3.Parameters.AddWithValue("@Name", txtPName.Text.ToString().Trim() + "01");
                    sqlCommand3.Parameters.AddWithValue("@Extention", extention);
                    sqlCommand3.ExecuteNonQuery();
                }
                if (fuImg02.HasFile)
                {
                    string savePath = Server.MapPath("~/Images/ProductImages/") + lastInsertedID;
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string extention = Path.GetExtension(fuImg02.PostedFile.FileName);
                    fuImg02.SaveAs(savePath + "\\" + txtPName.Text.ToString().Trim() + "02"+extention);
                    SqlCommand sqlCommand3 = new SqlCommand("ImagesCreate", sqlConnection);
                    sqlCommand3.CommandType = CommandType.StoredProcedure;
                    sqlCommand3.Parameters.AddWithValue("@ProductID", lastInsertedID);
                    sqlCommand3.Parameters.AddWithValue("@Name", txtPName.Text.ToString().Trim() + "02");
                    sqlCommand3.Parameters.AddWithValue("@Extention", extention);
                    sqlCommand3.ExecuteNonQuery();
                }
                if (fuImg03.HasFile)
                {
                    string savePath = Server.MapPath("~/Images/ProductImages/") + lastInsertedID;
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string extention = Path.GetExtension(fuImg03.PostedFile.FileName);
                    fuImg03.SaveAs(savePath + "\\" + txtPName.Text.ToString().Trim() + "03"+ extention);
                    SqlCommand sqlCommand3 = new SqlCommand("ImagesCreate", sqlConnection);
                    sqlCommand3.CommandType = CommandType.StoredProcedure;
                    sqlCommand3.Parameters.AddWithValue("@ProductID", lastInsertedID);
                    sqlCommand3.Parameters.AddWithValue("@Name", txtPName.Text.ToString().Trim() + "03");
                    sqlCommand3.Parameters.AddWithValue("@Extention", extention);
                    sqlCommand3.ExecuteNonQuery();
                }
                if (fuImg04.HasFile)
                {
                    string savePath = Server.MapPath("~/Images/ProductImages/") + lastInsertedID;
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string extention = Path.GetExtension(fuImg04.PostedFile.FileName);
                    fuImg04.SaveAs(savePath + "\\" + txtPName.Text.ToString().Trim() + "04"+ extention);
                    SqlCommand sqlCommand3 = new SqlCommand("ImagesCreate", sqlConnection);
                    sqlCommand3.CommandType = CommandType.StoredProcedure;
                    sqlCommand3.Parameters.AddWithValue("@ProductID", lastInsertedID);
                    sqlCommand3.Parameters.AddWithValue("@Name", txtPName.Text.ToString().Trim() + "04");
                    sqlCommand3.Parameters.AddWithValue("@Extention", extention);
                    sqlCommand3.ExecuteNonQuery();
                }
                if (fuImg05.HasFile)
                {
                    string savePath = Server.MapPath("~/Images/ProductImages/") + lastInsertedID;
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string extention = Path.GetExtension(fuImg05.PostedFile.FileName);
                    fuImg05.SaveAs(savePath + "\\" + txtPName.Text.ToString().Trim() + "05"+extention );
                    SqlCommand sqlCommand3 = new SqlCommand("ImagesCreate", sqlConnection);
                    sqlCommand3.CommandType = CommandType.StoredProcedure;
                    sqlCommand3.Parameters.AddWithValue("@ProductID", lastInsertedID);
                    sqlCommand3.Parameters.AddWithValue("@Name", txtPName.Text.ToString().Trim() + "05");
                    sqlCommand3.Parameters.AddWithValue("@Extention", extention);
                    sqlCommand3.ExecuteNonQuery();
                }
                txtDesc.Text = txtMatCare.Text = txtPDetails.Text = txtPName.Text = txtPrice.Text = txtQuantity.Text = txtSelPrice.Text = "";
                ddlBrands.ClearSelection();
                ddlBrands.Items.FindByValue("0").Selected = true;
                ddlCategory.ClearSelection();
                ddlCategory.Items.FindByValue("0").Selected = true;
                ddlGender.ClearSelection();
                ddlGender.Items.FindByValue("0").Selected = true;
                ddlSubCategory.ClearSelection();
                ddlSubCategory.Items.FindByValue("0").Selected = true;
                foreach (ListItem item in cblSize.Items)
                {
                    item.Selected = false;
                }
                cbCOD.Checked = false;
                cbFD.Checked = false;
                cb30Ret.Checked = false;
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = "Product Added!";
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Please, Fill the All Fields";
            }
        }
    }
}