<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="iso-8859-1" %>
<script language="C#" runat="server">
// Thumbnail script by Khaos, see http://www.freetextbox.com/forums/ShowPost.aspx?PostID=1066
void Page_Load(Object sender, EventArgs e)
{
	try{
		Response.Cache.VaryByParams["Image;Width;Height"] = true;
		Response.ContentType = "image/jpeg";
		System.Collections.Hashtable imageOutputFormatsTable = new System.Collections.Hashtable();
		imageOutputFormatsTable.Add(System.Drawing.Imaging.ImageFormat.Gif.Guid, System.Drawing.Imaging.ImageFormat.Gif);
		imageOutputFormatsTable.Add(System.Drawing.Imaging.ImageFormat.Jpeg.Guid, System.Drawing.Imaging.ImageFormat.Jpeg);
		imageOutputFormatsTable.Add(System.Drawing.Imaging.ImageFormat.Bmp.Guid, System.Drawing.Imaging.ImageFormat.Gif);
		imageOutputFormatsTable.Add(System.Drawing.Imaging.ImageFormat.Tiff.Guid, System.Drawing.Imaging.ImageFormat.Jpeg);
		imageOutputFormatsTable.Add(System.Drawing.Imaging.ImageFormat.Png.Guid, System.Drawing.Imaging.ImageFormat.Jpeg);
	
		string imageLocation;
		bool forceaspect = false;
		int newHeight;
		int newWidth;
		int reqHeight = 100;
		int reqWidth = 100;
		int origHeight;
		int origWidth;
		
		imageLocation = Server.MapPath(Request.QueryString["Image"]);
		if (Request.QueryString["Height"] != null){
			reqHeight = Convert.ToInt32(Request.QueryString["Height"]);
		}
		if(Request.QueryString["Width"] != null){
			reqWidth = Convert.ToInt32(Request.QueryString["Width"]);
		}
		if (Request.QueryString["ForceAspect"] == "true"){
			forceaspect = true;
		}
		
		System.Drawing.Bitmap origBitmap = new System.Drawing.Bitmap(imageLocation);
		origHeight = origBitmap.Height;
		origWidth = origBitmap.Width;
		
		if (forceaspect){
			//Force Aspect Change
			//Response.Write("Forced ");
			newHeight = reqHeight;
			newWidth = reqWidth;
		}		
		else if (origBitmap.Height >= origBitmap.Width){
			//Portrait
			//Response.Write("Portrait ");	
			newHeight = reqHeight;
			newWidth = (int)(((double)origBitmap.Width / (double)origBitmap.Height) * reqHeight);
		}
		else{
			//Landscape
			//Response.Write("Landscape ");
			newWidth = reqWidth;
			newHeight = (int)(((double)origBitmap.Height / (double)origBitmap.Width) * reqWidth);
		}		
		//Response.Write("Width: " + newWidth +  " Height: " + newHeight);
		
		System.Drawing.Bitmap outputImage = new System.Drawing.Bitmap(origBitmap, newWidth, newHeight);
		outputImage.SetResolution(24, 24);
		
		//Response.Write(origBitmap.HorizontalResolution + "x" + origBitmap.VerticalResolution + " " + outputImage.HorizontalResolution + "x" + outputImage.VerticalResolution);
		
		//outputImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
		System.Drawing.Imaging.ImageFormat outputFormat = (System.Drawing.Imaging.ImageFormat)imageOutputFormatsTable[origBitmap.RawFormat.Guid];
		
		outputImage.Save(Response.OutputStream, outputFormat);
		outputImage.Dispose();
		origBitmap.Dispose();
	}
	catch{
		Response.Redirect("images/thumberror.gif");
	}
}
</script>
