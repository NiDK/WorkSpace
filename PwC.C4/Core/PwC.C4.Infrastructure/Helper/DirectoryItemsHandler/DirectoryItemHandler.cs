namespace PwC.C4.Infrastructure.Helper.DirectoryItemsHandler
{
	/// <summary>
	///		<para>Represents a method that handles each file found by 
	///		<see cref="FileSystemIterator.GetDirectoryItems"/>.</para>
	/// </summary>
	/// <param name="path">
	///		<para>The fully qualified path of the directory item found.
	///		Never <see langword="null"/>.</para>
	/// </param>
	/// <returns>
	///		<para><see langword="true"/> if iteration should continue;
	///		<see langword="false"/> to stop iteration.</para>
	/// </returns>
	public delegate bool DirectoryItemHandler(string path);
}
