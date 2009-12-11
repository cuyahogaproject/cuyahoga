using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Versioning
{
	/// <summary>
	/// Interface for marking Content that should be versioned
	/// By default, only simple properties will be versioned
	/// </summary>
    public interface IVersionableContent
    {
		/// <summary>
		/// Returns paths to complex child properties that
		/// should be versioned as well, e.g.
		/// Foo
		/// Foo.Bar
		/// Foo.Bar.Baz
		/// Where Foo is a property of the object implementing IVersionableContent
		/// </summary>
		IList<string> CustomVersioningInfo { get; set;}
    }
}
