using System;
using System.IO;

namespace Cuyahoga.Core.Util
{
	public static class IOUtil
	{
		/// <summary>
		/// Recursively copies a directory to given location. Overwrites any existing files. Skips hidden files and directories.
		/// </summary>
		/// <param name="sourceDirectory"></param>
		/// <param name="targetDirectory"></param>
		static public void CopyDirectory(string sourceDirectory, string targetDirectory)
		{
			if (!Directory.Exists(targetDirectory))
			{
				Directory.CreateDirectory(targetDirectory);
			}
			string[] files = Directory.GetFiles(sourceDirectory);
			foreach (string file in files)
			{
				FileAttributes attributes = File.GetAttributes(file);
				if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
				{
					string name = Path.GetFileName(file);
					string dest = Path.Combine(targetDirectory, name);
					File.Copy(file, dest, true); // overwrite any existing files.
				}
			}
			string[] directories = Directory.GetDirectories(sourceDirectory);
			foreach (string directory in directories)
			{
				FileAttributes attributes = File.GetAttributes(directory);
				if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
				{
					string name = Path.GetFileName(directory);
					string dest = Path.Combine(targetDirectory, name);
					CopyDirectory(directory, dest);
				}
			}
		}

		/// <summary>
		/// Gets the name of the directory in the given path. Only returns the name of the deepest directory.
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <returns></returns>
		public static string GetDirectoryName(string directoryPath)
		{
			int lastSlashIndex = directoryPath.LastIndexOf(Path.DirectorySeparatorChar);
			return directoryPath.Substring(lastSlashIndex + 1);
		}

		public static bool CheckIfDirectoryIsWritable(string physicalDirectory)
		{
			// Check if the given directory is writable by creating a dummy file.
			string fileName = Path.Combine(physicalDirectory, "dummy.txt");

			try
			{
				using (StreamWriter sw = new StreamWriter(fileName))
				{
					// Add some text to the file.
					sw.WriteLine("DUMMY");
					sw.Flush();
				}
				File.Delete(fileName);
				return true;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
		}
	}
}
