using System;

namespace Cuyahoga.Core
{
	public class SiteNullException : ApplicationException
	{
		public SiteNullException(string message) : base(message)
		{
		}
	}

	public class NodeNullException : ApplicationException
	{
		public NodeNullException(string message) : base(message)
		{
		}
	}

	public class SectionNullException : ApplicationException
	{
		public SectionNullException(string message) : base(message)
		{
		}
	}

	public class AccessForbiddenException : ApplicationException
	{
		public AccessForbiddenException(string message) : base(message)
		{
		}
	}

	public class ActionForbiddenException : ApplicationException
	{
		public ActionForbiddenException(string message) : base(message)
		{
		}
	}

	public class DeleteForbiddenException : ApplicationException
	{
		public DeleteForbiddenException(string message) : base(message)
		{
		}
	}

	public class EmailException : ApplicationException
	{
		public EmailException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class InvalidPackageException : ApplicationException
	{
		public InvalidPackageException(string message) : base (message)
		{}
	}

	public class SearchException : ApplicationException
	{
		public SearchException(string message) : base(message)
		{}

		public SearchException(string message, Exception innerException) : base(message, innerException)
		{}
	}
}
