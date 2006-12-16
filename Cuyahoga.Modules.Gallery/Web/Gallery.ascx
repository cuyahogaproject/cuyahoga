<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Gallery.ascx.cs" Inherits="Cuyahoga.Modules.Gallery.Web.Gallery" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="pnlGalleries" visible="True" Width="100%" runat="server">
	<table cellspacing="2" cellpadding="2" width="100%" border="0">
		<asp:Repeater id="rptGalleries" Runat="server">
			<ItemTemplate>
				<tr>
					<td valign="middle" align="center" rowspan="2" style="width:20%">
						<img src='<%# DataBinder.Eval(Container.DataItem, "ThumbImage") %>' class="highlightOff" onmouseover="this.className='highlightOn'" onmouseout="this.className='highlightOff'" alt="" />
					</td>
					<td valign="top" align="left">
						<asp:HyperLink ID="hplGallery" CssClass="xpGalleryLink" Runat="server"></asp:HyperLink>
						<br>
						<%= base.GetText("ARTIST") %>
						-
						<%# DataBinder.Eval(Container.DataItem, "Artist") %>
						<p class="xpDescription">
							<%# DataBinder.Eval(Container.DataItem, "CurrentDescription") %>
						</p>
						<div style="position: relative;">
							<div style="position: absolute; top: 0px; left:0px; width: 83px; height: 18px;">
								<img id="imgBack" height="18" runat="server" alt="" />
							</div>
							<div style="position: absolute; top: 0px; left:0px; width: 83px; height: 18px; z-index: 1;">
								<img src='<%= ResolveUrl("~/Modules/xpGallery/Images/rating.gif")%>' height="18" width="83" alt="rating" />
							</div>
						</div>
						<asp:Literal ID="litRating" Runat="server"></asp:Literal>
						</p>
					</td>
				</tr>
				<tr>
					<td>
						<asp:hyperlink id="hplComments" runat="server"></asp:hyperlink>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<hr class="xpGalleryDivider">
						<br>
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater></table>
</asp:panel>
