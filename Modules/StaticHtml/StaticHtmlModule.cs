using System;

using Cuyahoga.Core;
using Cuyahoga.Modules.DAL;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	/// The StaticHtmlModule provides the content of simple static page. It needs at least its 
	/// Section to be set to do something with the content (load, update, delete).
	/// </summary>
	public class StaticHtmlModule : Module
	{
		private StaticHtmlContent _staticHtmlContent;

		/// <summary>
		/// Property StaticHtml (lazy loaded)
		/// </summary>
		public StaticHtmlContent StaticHtmlContent
		{
			get 
			{ 
				if (this._staticHtmlContent == null && this.Section != null)
				{
					this._staticHtmlContent = new StaticHtmlContent();
					IModulesDataProvider dp = ModulesDataProvider.GetInstance();
					dp.GetStaticHtmlContentBySectionId(this.Section.Id, this._staticHtmlContent);
				}
				return this._staticHtmlContent; 
			}
			set { this._staticHtmlContent = value; }
		}

		public StaticHtmlModule()
		{
			base.Section = null;
			this._staticHtmlContent = null;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void LoadContent()
		{
		}

		public override void DeleteContent()
		{

		}
	}
}
