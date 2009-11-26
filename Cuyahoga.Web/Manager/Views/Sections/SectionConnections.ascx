<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SectionViewData>" %>
<script type="text/javascript">

</script>
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
</div>