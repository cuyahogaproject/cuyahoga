using System;

namespace Cuyahoga.Core
{
	public class DeleteForbiddenException : ApplicationException
	{
		public DeleteForbiddenException(string message) : base(message)
		{
		}
	}

	public class SectionNullException : ApplicationException
	{
		public SectionNullException(string message) : base(message)
		{
		}
	}

	public class ActionForbiddenException : ApplicationException
	{
		public ActionForbiddenException(string message) : base(message)
		{
		}
	}
}
