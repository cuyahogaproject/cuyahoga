<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SectionViewData>" %>
<div id="sectionconnections">
	<% if (Model.Section.Connections.Count > 0) { %>
		<ul>
			<% foreach (var conn in Model.Section.Connections) { %>
				<li>
					<p>
					<%= GlobalResources.PageLabel %>: <strong><%= conn.Value.Node != null ? conn.Value.Node.Title : String.Empty %></strong><br />
					<%= GlobalResources.SectionLabel %>: <strong><%= conn.Value.Title %></strong><br />
					<%= GlobalResources.ActionLabel %>: <strong><%= conn.Key %></strong>
					<% using (Html.BeginForm("DeleteConnection", "Sections", new { sectionid = Model.Section.Id, actionname = conn.Key }, FormMethod.Post)) { %>
						<a class="deletelink" href="#"><%= GlobalResources.DeleteButtonLabel %></a>					
					<% } %>
					</p>
				</li>
			<% } %>
		</ul>
	<% } %>
	<% if (Model.Section.Connections.Count < Model.OutboundActions.Count) { %>
		<p><%= GlobalResources.AddConnectionLabel %></p>
		<% using (Html.BeginForm("AddConnection", "Sections", new { sectionid = Model.Section.Id }, FormMethod.Post)) { %>
			<label for="actionname"><%= GlobalResources.ActionLabel %></label><br />
			<%= Html.DropDownList("actionname", new SelectList(Model.UnconnectedActions), String.Format("--- {0} ---", GlobalResources.SelectActionOption)) %><br />
			<label for="connecttoid"><%= GlobalResources.ConnectToLabel %></label><br />
			<select id="connecttoid" name="connecttoid"></select><br />
			<input type="submit" value="<%= GlobalResources.AddConnectionLabel %>" />
		<% } %>
	<% } %>
</div>