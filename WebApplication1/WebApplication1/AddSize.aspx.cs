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
    public partial class AddSize : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=GADIR\SQLEXPRESS;Initial Catalog=ResponsiveWebsite;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBrands();
                BindMainCategories();
                BindGenders();
                BindSizes();
            }
        }
        private void BindSizes()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SizesBrandsCategoriesGendersJoin", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            rptrCategory.DataSource = dataTable;
            rptrCategory.DataBind();
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
                ddlBrand.DataSource = dataTable;
                ddlBrand.DataTextField = "Name";
                ddlBrand.DataValueField = "BrandID";
                ddlBrand.DataBind();
                ddlBrand.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        private void BindMainCategories()
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
                ddlSubCat.DataSource = dataTable;
                ddlSubCat.DataTextField = "SubCategoryName";
                ddlSubCat.DataValueField = "SubCategoryID";
                ddlSubCat.DataBind();
                ddlSubCat.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SizesCreate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SizeName", txtSizeName.Text.Trim());
            sqlCommand.Parameters.AddWithValue("@BrandID", ddlBrand.SelectedItem.Value);
            sqlCommand.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedItem.Value);
            sqlCommand.Parameters.AddWithValue("@SubCategoryID", ddlSubCat.SelectedItem.Value);
            sqlCommand.Parameters.AddWithValue("@GenderID", ddlGender.SelectedItem.Value);
            sqlCommand.ExecuteNonQuery();
            BindSizes();
            txtSizeName.Text = "";
            ddlCategory.ClearSelection();
            ddlBrand.ClearSelection();
            ddlGender.ClearSelection();
            ddlSubCat.ClearSelection();
            ddlCategory.Items.FindByValue("0").Selected = true;
            ddlGender.Items.FindByValue("0").Selected = true;
            ddlBrand.Items.FindByValue("0").Selected = true;
            ddlSubCat.Items.FindByValue("0").Selected = true;
        }
    }
}