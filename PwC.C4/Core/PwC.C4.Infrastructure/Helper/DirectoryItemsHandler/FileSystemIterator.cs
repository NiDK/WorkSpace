using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace PwC.C4.Infrastructure.Helper.DirectoryItemsHandler
{
	/// <summary>
	///		<para>Provides high performance enumeration of the contents of a directory.</para>
	/// </summary>
	public static class FileSystemIterator
	{
		#region GetDirectoryItems(String, String, Boolean)
		/// <summary>
		/// 	<para>Gets all subdirectories and files within a root directory</para>
		/// </summary>
		/// <param name="rootPath">
		/// 	<para>The full path of the root directory within which to find the 
		/// 	subdirectories and files.</para>
		/// </param>
		/// <param name="filter">
		/// 	<para>The name filter to limit the set of items
		/// 	returned.  For example, "*" returns all items, while "*.txt"
		/// 	returns all items with an extension of .txt.</para>
		/// </param>
		/// <param name="itemTypes">
		/// 	<para>Specifies whether to return files only, subdirectories only, or both.</para>
		/// </param>
		/// <param name="recursive">
		/// 	<para>Whether items of all subdirectories underneath <paramref name="rootPath"/>
		/// 	should be returned as well as those directly under <paramref name="rootPath"/>.</para>
		/// </param>
		/// <param name="handler">
		/// 	<para>A delegate that is called every time an item is found and is passed 
		/// 	the fully qualified path of the item.</para>
		/// </param>
		/// <returns>
		///		<para><see langword="true"/> if enumeration was completed; <see langword="false"/>
		///		if the method existed as a result of the handler's request to terminate the enumeration
		///		prematurely.</para>
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// 	<para>The argument <paramref name="rootPath"/> is <see langword="null"/>.</para>
		/// 	<para>-or-</para>
		/// 	<para>The argument <paramref name="filter"/> is <see langword="null"/>.</para>
		/// 	<para>-or-</para>
		/// 	<para>The argument <paramref name="handler"/> is <see langword="null"/>.</para>
		/// </exception>
		public static bool GetDirectoryItems(string rootPath, string filter, 
			FileSystemItemTypes itemTypes, bool recursive,
			PwC.C4.Infrastructure.Helper.DirectoryItemsHandler.DirectoryItemHandler handler)
		{
			if (rootPath == null) throw new ArgumentNullException("rootPath");
			if (filter == null) throw new ArgumentNullException("filter");
			if (handler == null) throw new ArgumentNullException("handler");

			IntPtr handle = IntPtr.Zero;
			try
			{
				FindData data;
				handle = GetHandle(FindFirstFileEx(
					Path.Combine(rootPath, filter), 
					0, // Standard info
					out data, 
					itemTypes == FileSystemItemTypes.Directory ? 1 : 0,
					IntPtr.Zero,
					0));
				if (handle == IntPtr.Zero) return true;

				string fullPath = Path.Combine(rootPath, data.fileName);

				if (IsIgnoredItem(data, fullPath, itemTypes) == false)
				{
					if (handler(fullPath) == false) return false;
				}

				while (FindNextFile(handle, out data))
				{
					fullPath = Path.Combine(rootPath, data.fileName);

					if (IsIgnoredItem(data, fullPath, itemTypes) == false)
					{
						if (handler(fullPath) == false) return false;
					}
				}

				CloseHandle(ref handle); // Prevent handle from being held open during recursion.

				// Recursion
				// Rewrite iteratively in the future?
				// [tchow 09/11/2006]
				if (recursive == true)
				{
					if (GetDirectoryItems(rootPath, "*", FileSystemItemTypes.Directory, false,
						delegate(string subdirectory)
						{
							if (GetDirectoryItems(subdirectory, filter, itemTypes, recursive,
								delegate(string subItem)
								{
									if (handler(subItem) == false) return false;
									return true;
								}) == false)
							{
								return false;
							}
							else
							{
								return true;
							}
						}) == false)
					{
						return false;
					}
				}
			}
			finally
			{
				if (handle != IntPtr.Zero)
				{
					CloseHandle(ref handle);
				}
			}

			return true;
		}
		#endregion

		private static IntPtr GetHandle(IntPtr handle)
		{
			if (handle != IntPtr.Zero)
			{
				Interlocked.Increment(ref handleCount);
			}
			return handle;
		}

		private static void CloseHandle(ref IntPtr handle)
		{
			if (handle != IntPtr.Zero)
			{
				Interlocked.Decrement(ref handleCount);
				FindClose(handle);
				handle = IntPtr.Zero;
			}
		}

		private static int handleCount = 0;

		#region IsIgnoredItem(FindData, String, FileSystemItemTypes)
		private static bool IsIgnoredItem(FindData data, string fullPath, FileSystemItemTypes itemTypes)
		{
			if (data.fileName.Length == 0 || data.fileName == "." || data.fileName == "..")
			{
				return true;
			}
			else if (itemTypes == FileSystemItemTypes.Directory
				&& (data.fileAttributes & FILE_ATTRIBUTE_DIRECTORY) == 0)
			{
				return true;
			}
			else if (itemTypes == FileSystemItemTypes.File
				&& (data.fileAttributes & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region Win32 Constants
		private const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		#endregion

		#region struct FindData
		[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
		private struct FindData
		{
			public int fileAttributes;
			public long creationTime;
			public long lastAccessTime;
			public long lastWriteTime;
			public int nFileSizeHigh;
			public int nFileSizeLow;
			public int dwReserved0;
			public int dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string fileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string alternateFileName;
		}
		#endregion

		#region Imports
		[DllImport("KERNEL32.DLL", EntryPoint = "FindFirstFileEx")]
		private static extern IntPtr FindFirstFileEx(string fileName, int infoLevelId, out FindData data, int searchOp, IntPtr searchFilter, int additionalFlags);

		[DllImport("KERNEL32.DLL", EntryPoint = "FindNextFile")]
		private static extern bool FindNextFile(IntPtr handle, out FindData data);

		[DllImport("KERNEL32.DLL", EntryPoint = "FindClose")]
		private static extern bool FindClose(IntPtr handle);
		#endregion
	}
}
