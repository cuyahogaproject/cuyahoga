using System;
using System.Collections;

namespace Cuyahoga.Core.Collections
{
	/// <summary>
	/// Summary description for SectionCollection.
	/// </summary>
	public class SectionCollection : CollectionBase
	{
		public SectionCollection()
		{
		}

		public void Add(Section section)
		{
			this.List.Add(section);
		}

		public void Remove(Section section)
		{
			this.List.Remove(section);
		}

		public Section this[int index]
		{
			get { return (Section)this.List[index]; }
			set { this.List[index] = value; }
		}
	}
}
