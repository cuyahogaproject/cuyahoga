using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cuyahoga.Core.Communication;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class SectionViewData
	{
		private readonly Section _section;

		public Section Section
		{
			get { return _section; }
		}

		public ModuleActionCollection OutboundActions { get; set; }

		public ModuleActionCollection InboundActions { get; set; }

		public SectionViewData(Section section)
		{
			this._section = section;
			this.OutboundActions = new ModuleActionCollection();
			this.InboundActions = new ModuleActionCollection();
		}
	}
}
