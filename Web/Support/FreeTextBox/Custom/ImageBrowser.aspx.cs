using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using Cuyahoga.Core;

namespace Cuyahoga.Web.Support.FreeTextBox.Custom
{
	/// <summary>
	/// Popup image browser for FreeTextbox 2.0.x.
	/// TODO: fine-tuning (delete images?, create and delete folders?)
	/// </summary>
	public class ImageBrowser : System.Web.UI.Page
	{
		private string[] _supportedExtensions = new string[] {".gif", ".jpg", ".png"};
		private ArrayList _browserItems;
		private string _currentVirtualPath;
		private string _currentPhysicalPath;

		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.Button btnUpload;
		protected System.Web.UI.WebControls.Label lblFolder;
		protected System.Web.UI.WebControls.TextBox txtWidth;
		protected System.Web.UI.WebControls.TextBox txtHeight;
		protected System.Web.UI.WebControls.Image imgPreview;
		protected System.Web.UI.WebControls.TextBox txtAlt;
		protected System.Web.UI.HtmlControls.HtmlInputFile uplImage;
		protected System.Web.UI.WebControls.Repeater rptItems;

		public ImageBrowser()
		{
			this._browserItems = new ArrayList();
		}

		private void UploadImage()
		{
			if (uplImage.PostedFile != null)
			{
				bool allowedType = ExtensionSupported(System.IO.Path.GetExtension(uplImage.Value));

				if (allowedType)
				{
					uplImage.PostedFile.SaveAs(this._currentPhysicalPath + System.IO.Path.GetFileName(uplImage.PostedFile.FileName));
					ReadFolder();
					BindBrowserItems();
				}
			}
		}
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Read image dir
			ReadFolder();
			BindBrowserItems();
		}

		private bool ExtensionSupported(string extension)
		{
			foreach (string supportedExtension in this._supportedExtensions)
			{
				if (supportedExtension.CompareTo(extension.ToLower()) == 0)
				{
					return true;
				}
			}
			return false;
		}

		private void ReadFolder()
		{
			this._browserItems.Clear();
			if (Context.Request.QueryString["imagedir"] != null && Context.Request.QueryString["imagedir"] != "")
			{
				this._currentVirtualPath = Context.Request.QueryString["imagedir"] + this.lblFolder.Text;
			}
			else
			{
				this._currentVirtualPath = this.ResolveUrl(Core.Util.Config.GetConfiguration()["ImageDir"] + this.lblFolder.Text);
			}
			this._currentPhysicalPath = Context.Server.MapPath(this._currentVirtualPath);

			DirectoryInfo dir = new DirectoryInfo(this._currentPhysicalPath);
			// Move up
			if (this.lblFolder.Text != "/")
			{
				this._browserItems.Add(dir.Parent);
			}

			// Subdirectories
			DirectoryInfo[] subDirs = dir.GetDirectories();
			foreach (DirectoryInfo subDir in subDirs)
			{
				this._browserItems.Add(subDir);
			}

			// Files
			FileInfo[] images = dir.GetFiles();
			foreach (FileInfo image in images)
			{
				if (ExtensionSupported(image.Extension))
				{
					this._browserItems.Add(image);
				}
			}
		}

		private void BindBrowserItems()
		{
			this.rptItems.DataSource = this._browserItems;
			this.rptItems.DataBind();
		}

		private void SetImageProperties(string fileName)
		{
			System.Drawing.Image imageFile = System.Drawing.Image.FromFile(this._currentPhysicalPath + fileName);
			this.txtWidth.Text = imageFile.Width.ToString();
			this.txtHeight.Text = imageFile.Height.ToString();
			imageFile.Dispose();
			this.imgPreview.ImageUrl = String.Format("Thumbnail.aspx?image={0}&width=100&height=100", this._currentVirtualPath + fileName);
			this.imgPreview.Visible = true;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.rptItems.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptItems_ItemDataBound);
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptItems_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			System.Web.UI.WebControls.Image icon = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgIcon");
			if (e.Item.DataItem is DirectoryInfo)
			{
				if (this.lblFolder.Text != "/" && e.Item.ItemIndex == 0)
				{
					icon.ImageUrl = "images/folderup.gif";
					LinkButton lbtDirectory = (LinkButton)e.Item.FindControl("lbtItem");
					lbtDirectory.Text = "..";
					lbtDirectory.CommandName = "moveup";
					lbtDirectory.CommandArgument = ((DirectoryInfo)e.Item.DataItem).FullName;
					lbtDirectory.Command += new CommandEventHandler(lbtDirectory_Command);
				}
				else
				{
					icon.ImageUrl = "images/folder.gif";
					LinkButton lbtDirectory = (LinkButton)e.Item.FindControl("lbtItem");
					lbtDirectory.CommandName = "move";
					lbtDirectory.CommandArgument = ((DirectoryInfo)e.Item.DataItem).FullName;
					lbtDirectory.Command += new CommandEventHandler(lbtDirectory_Command);
				}
			}
			if (e.Item.DataItem is FileInfo)
			{
				icon.ImageUrl = "images/image.gif";
				LinkButton lbtImage = (LinkButton)e.Item.FindControl("lbtItem");
				lbtImage.CommandName = "select";
				lbtImage.CommandArgument = ((FileInfo)e.Item.DataItem).Name;
				lbtImage.Command += new CommandEventHandler(lbtImage_Command);
			}
		}

		private void lbtImage_Command(object sender, CommandEventArgs e)
		{
			// Filename is the CommandArgument
			string fileName = e.CommandArgument.ToString();
			if (fileName != null)
			{
				string imgUrl = this._currentVirtualPath + fileName;
				this.txtUrl.Text = imgUrl;
				SetImageProperties(fileName);
			}
		}

		private void lbtDirectory_Command(object sender, CommandEventArgs e)
		{
			// PhysicalPath is the CommandArgument
			string newPhysicalPath = e.CommandArgument.ToString();
			if (this._currentPhysicalPath.Length > newPhysicalPath.Length)
			{
				this.lblFolder.Text = this.lblFolder.Text.Substring(0, this.lblFolder.Text.Length - (this._currentPhysicalPath.Length - newPhysicalPath.Length) + 1);
			}
			else
			{
				this.lblFolder.Text += newPhysicalPath.Substring(this._currentPhysicalPath.Length, (newPhysicalPath.Length - this._currentPhysicalPath.Length)) + "/";
			}
			ReadFolder();
			BindBrowserItems();
		}

		private void btnUpload_Click(object sender, System.EventArgs e)
		{
			UploadImage();
		}
	}
}
