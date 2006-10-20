<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Page language="c#" Codebehind="AdminForum.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Forum.AdminForum" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Forum management</h1>
				<h1>Categories</h1>
				<table class="tbl">
					<asp:repeater id="rptForumCategories" runat="server">
						<HeaderTemplate>
							<tr>
								<th>
									Title</th>
								<th>
									&nbsp;</th>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td>
									<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink></td>
							</tr>
						</ItemTemplate>
					</asp:repeater>
				</table>
				<br>
				<div class="pager">
					<cc1:pager id="pgrForumCategories" runat="server" controltopage="rptForumCategories" cachedatasource="True"
						pagesize="10" cacheduration="30" cachevarybyparams="SectionId"></cc1:pager>
				</div>
				<br>
				&nbsp;
				<asp:Button id="btnNewCategory" runat="server" Text="New category"></asp:Button>
				<h1>Forums</h1>
				<table class="tbl">
					<asp:repeater id="rptForumlist" runat="server">
						<HeaderTemplate>
							<tr>
								<th>
									Title</th>
								<th>
									&nbsp;</th>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td>
									<asp:hyperlink id="hplForumEdit" runat="server">Edit</asp:hyperlink></td>
							</tr>
						</ItemTemplate>
					</asp:repeater>
				</table>
				<br>
				<div class="pager">
					<cc1:pager id="Pager1" runat="server" controltopage="rptForumlist" cachedatasource="True" pagesize="10"
						cacheduration="30" cachevarybyparams="SectionId"></cc1:pager>
				</div>
				<br>
				&nbsp;
				<asp:Button id="btnNewForum" runat="server" Text="New Forum"></asp:Button>
				<h1>Emoticons</h1>
				<table class="tbl">
					<asp:repeater id="rptEmoticons" runat="server">
						<HeaderTemplate>
							<tr>
								<th>
									Title</th>
								<th>
									&nbsp;</th>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "TextVersion") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "ImageName") %></td>
								<td>
									<asp:hyperlink id="hplEditEmoticon" runat="server">Edit</asp:hyperlink></td>
							</tr>
						</ItemTemplate>
					</asp:repeater>
				</table>
				<br>
				<div class="pager">
					<cc1:pager id="pgEmoticons" runat="server" controltopage="rptEmoticons" cachedatasource="True"
						pagesize="10" cacheduration="30" cachevarybyparams="SectionId"></cc1:pager>
				</div>
				<br>
				&nbsp;
				<asp:Button id="btnNewEmoticon" runat="server" Text="New Emoticon"></asp:Button>
				<h1>Tags</h1>
				<table class="tbl">
					<asp:repeater id="rptTags" runat="server">
						<HeaderTemplate>
							<tr>
								<th>
									Forum start code</th>
								<th>
									Forum end code</th>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "ForumCodeStart") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "ForumCodeEnd") %></td>
								<td>
									<asp:hyperlink id="hplTagEdit" runat="server">Edit</asp:hyperlink></td>
							</tr>
						</ItemTemplate>
					</asp:repeater>
				</table>
				<br>
				<div class="pager">
					<cc1:pager id="pgrForumTags" runat="server" controltopage="rptTags" cachedatasource="True"
						pagesize="10" cacheduration="30" cachevarybyparams="SectionId"></cc1:pager>
				</div>
				<br>
				&nbsp;
				<asp:Button id="btnNewTag" runat="server" Text="New Tag"></asp:Button>
		</form>
		</DIV>
	</body>
</HTML>
