using System;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.SiteStructure;

namespace Cuyahoga.Web.Mvc.Controllers
{
	/// <summary>
	/// Base class for module admin controllers.
	/// </summary>
	public class ModuleAdminController : SecureController
	{
		private Node _currentNode;
		private Section _currentSection;
		private INodeService _nodeService;
		private ISectionService _sectionService;

		/// <summary>
		/// Sets the node service.
		/// </summary>
		public INodeService NodeService
		{
			set { this._nodeService = value; }
		}

		/// <summary>
		/// Sets the section service.
		/// </summary>
		public ISectionService SectionService
		{
			set { this._sectionService = value; }
		}

		/// <summary>
		/// Gets the current node.
		/// </summary>
		protected Node CurrentNode
		{
			get { return this._currentNode; }
		}

		/// <summary>
		/// Gets the current section.
		/// </summary>
		protected Section CurrentSection
		{
			get { return this._currentSection; }
		}

		protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
		{
			// try to set currentNode and/or currentSection
			if (Request.Params["NodeId"] != null)
			{
				this._currentNode =
					this._nodeService.GetNodeById(Int32.Parse(Request.Params["nodeId"]));
			}
			if (Request.Params["SectionId"] != null)
			{
				this._currentSection = this._sectionService.GetSectionById(Int32.Parse(Request.Params["SectionId"]));
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
