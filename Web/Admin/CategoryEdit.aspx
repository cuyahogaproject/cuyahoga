<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CategoryEdit.aspx.cs" Inherits="Cuyahoga.Web.Admin.CategoryEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Categories</title>
</head>
<body>
    <form id="form1" runat="server">
                <table>
                    <tr>
                        <td style="width: 100px">
                            Key
                        </td>
                        <td>
                            <asp:TextBox ID="txtKey" runat="server" Width="150px" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvKey" runat="server" ErrorMessage="Key is required"
                                Display="Dynamic" CssClass="validator" ControlToValidate="txtKey" EnableClientScript="False"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblParentCategoryId" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required"
                                Display="Dynamic" CssClass="validator" ControlToValidate="txtName" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            &nbsp;Description
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server"
                                Width="250px" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description is required"
                                Display="Dynamic" CssClass="validator" ControlToValidate="txtDescription" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Path
                        </td>
                        <td>
                            <asp:TextBox ID="txtPath" Enabled="false" runat="server" Width="250px" MaxLength="80"></asp:TextBox>

                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnSave" runat="server" Text="Save"  OnClick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
                <asp:Button ID="btnBack" runat="server" Text="Back"  OnClick="btnCancel_Click" />
    </form>
</body>
</html>
