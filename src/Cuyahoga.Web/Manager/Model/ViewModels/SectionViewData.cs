using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

		public bool ExpandConnections { get; set; }

		public string[] UnconnectedActions
		{
			get
			{
				return
					OutboundActions.OfType<ModuleAction>()
						.Select(m => m.Name)
						.Except(_section.Connections.Select(c => c.Key))
						.ToArray();
			}
		}

		public SectionViewData(Section section)
		{
			this._section = section;
			this.OutboundActions = new ModuleActionCollection();
			this.InboundActions = new ModuleActionCollection();
		}
	}
}
