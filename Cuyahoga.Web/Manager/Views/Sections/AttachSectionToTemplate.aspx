<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<AttachSectionTemplateViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= String.Format(GlobalResources.AttachSectionToTemplatesPageTitle, Model.Section.Title) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h2><%= String.Format(GlobalResources.AttachSectionToTemplatesPageTitle, Model.Section.Title) %></h2>
	<% using (Html.BeginForm("UpdateSectionTemplateAttachment", "Sections", FormMethod.Post, new { id = "attachform" })) { %>
		<%= Html.Hidden("SectionId", Model.Section.Id) %>
		<table class="grid" style="width:100%">
			<thead>
				<tr>
					<th><%= GlobalResources.TemplateLabel %></th>
					<th><%= GlobalResources.IsAttachedLabel %></th>
					<th><%= GlobalResources.PlaceholderLabel %></th>
				</tr>
			</thead>
			<tbody>
				<%
					int index = 0;
					foreach (var template in Model.Templates) { 
				%>
					<tr>
						<td><%= template.Name %></td>
						<td class="center">
							<%= Html.CheckBox("SectionTemplates[" + index + "].IsAttached", Model.SectionTemplates[template.Id].IsAttached) %>
						</td>
						<td>
							<input type="hidden" name="SectionTemplates[<%= index %>].TemplateId" value="<%= template.Id %>" />
							<select name="SectionTemplates[<%= index %>].Placeholder">
								<% foreach (var placeholder in Model.PlaceHoldersByTemplate[template]) { %>
									<option value="<%= placeholder %>"<%= template.Sections.ContainsKey(placeholder) && template.Sections[placeholder] == Model.Section ? " selected=\"selected\"" : String.Empty %>><%= placeholder %></option>
								<% } %>
							</select>
							
						</td>
					</tr>
				<%
					index++;
				} %>
			</tbody>
		</table>
		<div id="buttonpanel">
		    <input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
		    <%= GlobalResources.Or %>
		    <%= Html.ActionLink(GlobalResources.BackLabel, "Index", "Sections", new { id = Model.Section.Id }, null) %>
		</div>

	<% } %>
</asp:Content>
