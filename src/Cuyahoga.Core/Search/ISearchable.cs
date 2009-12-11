using System;

namespace Cuyahoga.Core.Search
{
	/// <summary>
	/// The ISearchable interface defines the contract for modules that need to have their content indexed.
	/// </summary>
	public interface ISearchable
	{
		event IndexEventHandler ContentCreated;
		event IndexEventHandler ContentUpdated;
		event IndexEventHandler ContentDeleted;

		SearchContent[] GetAllSearchableContent();
	}
}
