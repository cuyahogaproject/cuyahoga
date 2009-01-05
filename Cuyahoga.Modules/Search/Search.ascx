<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Search.ascx.cs" Inherits="Cuyahoga.Modules.Search.Search" %>
<%@ Register TagPrefix="csc" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<asp:Panel ID="pnlCriteria" runat="server">
    <asp:TextBox ID="txtSearchText" runat="server" Width="200px"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"></asp:Button>
    <br />
    <br />
</asp:Panel>
<asp:Panel ID="pnlFilter" runat="server" Visible="false">
 <%= base.GetText("CATEGORY_FILTER") %> <asp:Label ID="lblFilter" runat="server" /><asp:LinkButton ID="lnkBtnRemoveFilter" Text="[x]" runat="server" OnClick="lnkBtnRemoveFilter_Click" />
   <br />
</asp:Panel>
<asp:Panel ID="pnlResults" runat="server" Visible="False">
    <%= base.GetText("DISPLAYING_RESULTS") %>
    <asp:Label ID="lblFrom" runat="server" Font-Bold="True"></asp:Label>
    -
    <asp:Label ID="lblTo" runat="server" Font-Bold="True"></asp:Label>
    <%= base.GetText("OF") %>
    <asp:Label ID="lblTotal" runat="server" Font-Bold="True"></asp:Label>
    <asp:Literal ID="litFor" runat="server" />
    <asp:Label ID="lblQueryText" runat="server" Font-Bold="True"></asp:Label>
    (<asp:Label ID="lblDuration" runat="server"></asp:Label>
    <%= base.GetText("SECONDS") %>
    )
    <ul class="searchresults">
        <asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
            <ItemTemplate>
                <li>
                    <h4>
                        <asp:HyperLink ID="hplPath" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Path") %>'>
							<%# DataBinder.Eval(Container.DataItem, "Title") %>
                        </asp:HyperLink></h4>
                    <div class="summary">
                        <%# DataBinder.Eval(Container.DataItem, "Summary") %>
                    </div>
                    <div class="category">
						<%# DataBinder.Eval(Container.DataItem, "Category") %>
                    </div>
                    <div class="sub">
                        <%# DataBinder.Eval(Container.DataItem, "Path") %>
                        -
                        <asp:Literal ID="litDateCreated" runat="server"></asp:Literal>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="pager">
        <csc:Pager id="pgrResults" runat="server" controltopage="rptResults" cachedatasource="False">
        </csc:Pager></div>
</asp:Panel>
<asp:Panel ID="pnlNotFound" runat="server" Visible="False" EnableViewState="False">
    <%= base.GetText("NOTFOUND") %>
</asp:Panel>
