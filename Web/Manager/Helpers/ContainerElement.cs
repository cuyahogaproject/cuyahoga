using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Cuyahoga.Web.Manager.Helpers
{
	public class ContainerElement : IDisposable
	{
		private bool _disposed;
		private readonly HttpResponseBase _httpResponse;
		private readonly string _tagName;

		public ContainerElement(HttpResponseBase httpResponse, string tagName)
		{
			if (httpResponse == null)
			{
				throw new ArgumentNullException("httpResponse");
			}
			_httpResponse = httpResponse;
			_tagName = tagName;
		}

		public void Dispose()
		{
			Dispose(true /* disposing */);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;
				_httpResponse.Write(string.Format("</{0}>", this._tagName));
			}
		}

		public void End()
		{
			Dispose(true);
		}
	}
}
