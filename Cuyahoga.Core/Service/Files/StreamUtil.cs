using System;
using System.IO;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// Utility class for Stream actions.
	/// </summary>
	public class StreamUtil
	{
		private const int _bufferSize = 1024;

		private StreamUtil()
		{
		}

		/// <summary>
		/// Copy the contents from one stream into another one.
		/// </summary>
		/// <param name="fromStream"></param>
		/// <param name="toStream"></param>
		public static void Copy(Stream fromStream, Stream toStream)
		{
			if (fromStream.CanSeek)
			{
				fromStream.Position = 0;
			}
			if (toStream.CanSeek)
			{
				toStream.Position = 0;
			}

			byte[] buffer = new byte[_bufferSize];
			int len;
			while ((len = fromStream.Read(buffer, 0, _bufferSize)) > 0)
			{
				toStream.Write(buffer, 0, len);
			}
		}
	}
}
