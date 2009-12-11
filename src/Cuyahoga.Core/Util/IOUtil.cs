using System;
using System.Globalization;
using System.IO;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// Utility methods for file and directory operations.
	/// </summary>
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
					File.Copy(file, dest, false); // overwrite any existing files.
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
		/// Gets the name of the last fragment in the given path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string GetLastPathFragment(string path)
		{
			int lastSlashIndex;
			if (path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
			{
				lastSlashIndex = path.Substring(0, path.Length -1).LastIndexOf(Path.DirectorySeparatorChar);
			}
			else
			{
				lastSlashIndex = path.LastIndexOf(Path.DirectorySeparatorChar);
			}
			return path.Substring(lastSlashIndex + 1);
		}

		/// <summary>
		/// Ensures a unique file path name. If there already is a file with the name, a number suffix is added.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>The file path with a unique file name.</returns>
		public static string EnsureUniqueFilePath(string filePath)
		{
			string fileNameTemplate = Path.GetFileNameWithoutExtension(filePath) + "({0})" + Path.GetExtension(filePath);

			int count = 0;
			while (File.Exists(filePath))
			{
				count++;
				string newFileName = String.Format(fileNameTemplate, count);
				filePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
			}
			return filePath;
		}

		/// <summary>
		/// Checks if the given directory is writable.
		/// </summary>
		/// <param name="physicalDirectory"></param>
		/// <returns></returns>
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
