<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Task_5_new._Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding:15">
        <div align="center" style="font-size:30px">User Info Report</div>
        <div align="center">
            <asp:TextBox ID="TextBoxInput" runat="server" Width="200px" Placeholder="Enter User Filter"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Load Report" BackColor="#66CCFF" Font-Size="Larger" ForeColor="#660033" Height="70px" OnClick="Button1_Click" Width="234px" />
            <br />
            <br />
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" style="margin-left: 0px" Width="1290px"></rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
