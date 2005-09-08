namespace Cuyahoga.Modules.Downloads.Web
{
	using System;
	using System.Data;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.IO;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Util;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Downloads.Domain;
	using Cuyahoga.Modules.Downloads.Util;

	/// <summary>
	///		Summary description for Downloads.
	/// </summary>
	public class Downloads : BaseModuleControl
	{
		private const int BUFFER_SIZE = 8192;

		private DownloadsModule _downloadsModule;

		protected System.Web.UI.WebControls.Repeater rptFiles;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._downloadsModule = this.Module as DownloadsModule;

			if (! this.IsPostBack)
			{
				if (this._downloadsModule.CurrentAction == DownloadsModuleActions.Download 
					&& this._downloadsModule.CurrentFileId > 0)
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
			this.rptFiles.DataSource = this._downloadsModule.GetAllFiles();
			this.rptFiles.DataBind();
		}

		private void DownloadCurrentFile()
		{
			Domain.File file = this._downloadsModule.GetFileById(this._downloadsModule.CurrentFileId);
			if (file.IsDownloadAllowed(this.Page.User.Identity))
			{
				string physicalFilePath = Path.Combine(this._downloadsModule.FileDir, file.FilePath);
				if (System.IO.File.Exists(physicalFilePath))
				{
					Stream fileStream = null;
					try
					{
						byte[] buffer = new byte[BUFFER_SIZE];
						// Open the file.
						fileStream = new System.IO.FileStream(physicalFilePath, System.IO.FileMode.Open,
							System.IO.FileAccess.Read, System.IO.FileShare.Read);

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
						Response.ContentType = file.ContentType;
						Response.AppendHeader("Content-Disposition", "attachment; filename=" + file.FilePath);
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
								int length = fileStream.Read(buffer, 0, BUFFER_SIZE);

								// Write the data to the current output stream.
								Response.OutputStream.Write(buffer, 0, length);

								// Flush the data to the HTML output.
								Response.Flush();

								buffer = new byte[BUFFER_SIZE];
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
							file.NrOfDownloads++;
							this._downloadsModule.SaveFile(file);
						}
					}
					finally
					{
						if (fileStream != null)
						{
							fileStream.Close();
						}
						Response.End();
					}
				}
				else
				{
					throw new Exception("The physical file was not found on the server.");
				}
			}
			else
			{
				throw new Exception("You are not allowed to download the file.");
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

		private void rptFiles_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			Domain.File file = e.Item.DataItem as Domain.File;
			
			if (file.IsDownloadAllowed(base.Page.User.Identity))
			{
				string downloadUrl = UrlHelper.GetUrlFromSection(this._downloadsModule.Section) + "/download/" + file.Id.ToString();
				string fileDetails = file.FilePath + " (" + file.Size.ToString() + " bytes)";
				HyperLink hplFileImg = e.Item.FindControl("hplFileImg") as HyperLink;
				hplFileImg.NavigateUrl = downloadUrl;
				hplFileImg.ImageUrl = base.Page.ResolveUrl("~/Modules/Downloads/Images/" 
					+ FileTypesMap.GetIconFilename(System.IO.Path.GetExtension(file.FilePath)));
				hplFileImg.Text = fileDetails;

				HyperLink hplFile = e.Item.FindControl("hplFile") as HyperLink;
				hplFile.NavigateUrl = downloadUrl;
				hplFile.ToolTip = fileDetails;

				Panel pnlFileDetails = e.Item.FindControl("pnlFileDetails") as Panel;
				pnlFileDetails.Visible = (this._downloadsModule.ShowDateModified || this._downloadsModule.ShowPublisher || this._downloadsModule.ShowNumberOfDownloads);
				if (this._downloadsModule.ShowDateModified)
				{
					Label lblDateModified = e.Item.FindControl("lblDateModified") as Label;
					lblDateModified.Visible = true;
					lblDateModified.Text = TimeZoneUtil.AdjustDateToUserTimeZone(
						file.DatePublished, this.Page.User.Identity).ToString();
				}
				if (this._downloadsModule.ShowPublisher)
				{
					Label lblPublisher = e.Item.FindControl("lblPublisher") as Label;
					lblPublisher.Visible = true;
					lblPublisher.Text += base.GetText("PUBLISHEDBY") + " " + file.Publisher.FullName;
				}
				if (this._downloadsModule.ShowNumberOfDownloads)
				{
					Label lblNumberOfDownloads = e.Item.FindControl("lblNumberOfDownloads") as Label;
					lblNumberOfDownloads.Visible = true;
					lblNumberOfDownloads.Text += base.GetText("NUMBEROFDOWNLOADS") + ": " + file.NrOfDownloads.ToString();
				}
			}
			else
			{
				e.Item.Visible = false;
			}
		}
	}
}
