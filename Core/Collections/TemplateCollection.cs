using System;
using System.Collections;

namespace Cuyahoga.Core.Collections
{
	/// <summary>
	/// Summary description for TemplateCollection.
	/// </summary>
	public class TemplateCollection : CollectionBase
	{
		public TemplateCollection()
		{
		}

		public void Add(Template template)
		{
			this.List.Add(template);
		}

		public void Remove(Template template)
		{
			this.List.Remove(template);
		}

		public Template this[int index]
		{
			get { return (Template)this.List[index]; }
		}
	}
}
