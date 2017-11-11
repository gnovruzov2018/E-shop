<%@ Page Title="" Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="WebApplication1.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="padding-top: 50px">
        <asp:Repeater ID="rptrProducts" runat="server">
            <ItemTemplate>
                <div class="col-sm-3 col-md-3">
                   <a style="text-decoration:none;" href="ProductView.aspx?ProductID=<%#Eval("ProductID") %>">
                    <div class="thumbnail">
                        <img src="Images/ProductImages/<%#Eval("ProductID") %>/<%#Eval("ImageName") %><%#Eval("Extention") %>" alt="<%#Eval("ImageName") %>">
                        <div class="caption">
                            <div class="probrand"><%#Eval("BrandName") %></div>
                            <div class="proName"><%#Eval("Name") %></div>
                            <div class="proPrice"><span class="proOgPrice"><%#Eval("Price") %></span> <%#Eval("SelPrice") %> <span class="proPriceDiscount">(<%#Eval("DiscAmount") %>  Off)</span></div>
                        </div>
                    </div>
                   </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
