using System;
using System.Web.UI.WebControls;
using System.IO;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Downloads.Util;

namespace Cuyahoga.Modules.Downloads.Web
{
	/// <summary>
	///		Summary description for Downloads.
	/// </summary>
	public class Downloads : BaseModuleControl<DownloadsModule>
	{
		private const int BufferSize = 8192;
		protected Repeater rptFiles;

		private void Page_Load(object sender, System.EventArgs e)
		{

			if (! this.IsPostBack)
			{
				if (this.Module.CurrentAction == DownloadsModuleActions.Download 
					&& this.Module.CurrentFileId > 0)
				{
					DownloadCurrentFile();
				}
				else
				{
					BindFiles();
				}
			}
		}

		private void BindFiles()
		{
			this.rptFiles.DataSource = this.Module.GetAllFiles();
			this.rptFiles.DataBind();
		}

		private void DownloadCurrentFile()
		{
			FileResource file = this.Module.GetFileById(this.Module.CurrentFileId);
			Stream fileStream = this.Module.GetFileAsStream(file);
			try
			{
				byte[] buffer = new byte[BufferSize];

				// Total bytes to read:
				long dataToRead = fileStream.Length;

				// Support for resuming downloads
				long position = 0;
				if (Request.Headers["Range"] != null)
				{
					Response.StatusCode = 206;
					Response.StatusDescription = "Partial Content";
					position = long.Parse(Request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
				}
				if (position != 0)
				{
					Response.AddHeader("Content-Range", "bytes " + position.ToString() + "-" + ((long)(dataToRead - 1)).ToString() + "/" + dataToRead.ToString());
				}
				Response.ContentType = file.MimeType;
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + file.FileName);
				// The content length depends on the amount that is already transfered in an earlier request.
				Response.AppendHeader("Content-Length", (fileStream.Length - position).ToString());
			
				// Stream the actual content
				bool isInterrupted = false;
				while (dataToRead > 0 && ! isInterrupted)
				{
					// Verify that the client is connected.
					if (Response.IsClientConnected)
					{
						// Read the data in buffer.
						int length = fileStream.Read(buffer, 0, BufferSize);

						// Write the data to the current output stream.
						Response.OutputStream.Write(buffer, 0, length);

						// Flush the data to the HTML output.
						Response.Flush();

						buffer = new byte[BufferSize];
						dataToRead = dataToRead - length;
					}
					else
					{
						//prevent infinite loop if user disconnects
						isInterrupted = true;
					}
				}

				// Only update download statistics if the download is succeeded.
				if (! isInterrupted)
				{
					this.Module.IncreaseNrOfDownloads(file);
				}
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream.Dispose();
				}
				Response.End();
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rptFiles.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptFiles_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			FileResource file = e.Item.DataItem as FileResource;
			
			if (file != null && file.IsViewAllowed(base.Page.User))
			{
				string downloadUrl = UrlUtil.GetApplicationPath() + file.GetContentUrl();
				string fileDetails = file.FileName + " (" + file.Length + " bytes)";
				HyperLink hplFileImg = e.Item.FindControl("hplFileImg") as HyperLink;
				hplFileImg.NavigateUrl = downloadUrl;
				hplFileImg.ImageUrl = base.Page.ResolveUrl("~/Modules/Downloads/Images/" 
					+ FileTypesMap.GetIconFilename(Path.GetExtension(file.FileName)));
				hplFileImg.Text = fileDetails;

				HyperLink hplFile = e.Item.FindControl("hplFile") as HyperLink;
				hplFile.NavigateUrl = downloadUrl;
				hplFile.ToolTip = fileDetails;

				Panel pnlFileDetails = e.Item.FindControl("pnlFileDetails") as Panel;
				pnlFileDetails.Visible = (this.Module.ShowDateModified || this.Module.ShowPublisher || this.Module.ShowNumberOfDownloads);
				if (this.Module.ShowDateModified)
				{
					Label lblDateModified = e.Item.FindControl("lblDateModified") as Label;
					lblDateModified.Visible = true;
					lblDateModified.Text = TimeZoneUtil.AdjustDateToUserTimeZone(
						file.PublishedAt.Value, this.Page.User.Identity).ToString();
				}
				if (this.Module.ShowPublisher)
				{
					Label lblPublisher = e.Item.FindControl("lblPublisher") as Label;
					lblPublisher.Visible = true;
					lblPublisher.Text += base.GetText("PUBLISHEDBY") + " " + file.ModifiedBy.FullName;
				}
				if (this.Module.ShowNumberOfDownloads)
				{
					Label lblNumberOfDownloads = e.Item.FindControl("lblNumberOfDownloads") as Label;
					lblNumberOfDownloads.Visible = true;
					lblNumberOfDownloads.Text += base.GetText("NUMBEROFDOWNLOADS") + ": " + file.DownloadCount;
				}
			}
			else
			{
				e.Item.Visible = false;
			}
		}
	}
}
