using System;

namespace PwC.C4.Infrastructure.Helper.DirectoryItemsHandler
{
	/// <summary>
	///		<para>Provides enumerated values indicating a combination of
	///		file system items to be returned by <see cref="FileSystemIterator"/>.</para>
	/// </summary>
	[Flags]
	public enum FileSystemItemTypes
	{
		/// <summary>
		///		<para>Include file paths in the enumeration.</para>
		/// </summary>
		File = 0x1,
		/// <summary>
		///		<para>Include subdirectory paths in the enumeration.</para>
		/// </summary>
		Directory = 0x2,
		/// <summary>
		///		<para>Include both file and subdirectory paths in the enumeration.</para>
		/// </summary>
		All = File | Directory
	}
}
