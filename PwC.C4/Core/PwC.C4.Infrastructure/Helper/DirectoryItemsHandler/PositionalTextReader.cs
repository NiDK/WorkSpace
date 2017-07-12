using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PwC.C4.Infrastructure.Helper.DirectoryItemsHandler
{
	/// <summary>
	///		<para>A line-by-line reader for a text-based <see cref="Stream"/>, capable of
	///		returning to a previous position in the <see cref="Stream"/> or <see cref="String"/> 
	///		being read from, as well as bufferring of read lines to minimize memory allocation.</para>
	/// </summary>
	public sealed class PositionalTextReader: IDisposable
	{
		#region Private Fields
		private List<string> readLines = new List<string>();
		private TextReader innerReader;
		private int trueReaderIndex = 0;
		#endregion

		#region Ctor(Stream)
		/// <summary>
		/// 	<para>Initializes an instance of the <see cref="PositionalTextReader"/> 
		///		class that reads from a <see cref="Stream"/>.</para>
		/// </summary>
		/// <param name="stream">
		/// 	<para>The text-based stream to read.  This reader will begin reading
		/// 	at the current position of of this stream and halt at an arbitrary point.
		/// 	Will be disposed when this instance is disposed.  Must not be 
		///		<see langword="null"/>.</para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// 	<para>The argument <paramref name="stream"/> is <see langword="null"/>.</para>
		/// </exception>
		public PositionalTextReader(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException("stream");

			this.innerReader = new StreamReader(stream);
		}
		#endregion

		#region Ctor(String)
		/// <summary>
		/// 	<para>Initializes an instance of the <see cref="PositionalTextReader"/> class
		///		that reads an in-memory <see cref="String"/>.</para>
		/// </summary>
		/// <param name="str">
		/// 	<para>The <see cref="String"/> to be read by this reader.</para>
		/// </param>
		public PositionalTextReader(string str)
		{
			if (str == null) throw new ArgumentNullException("str");

			this.innerReader = new StringReader(str);
		}
		#endregion

		#region NextLineIndex {get;}
		/// <summary>
		/// 	<para>Gets or sets the zero-based index of the line to
		/// 	be next returned by <see cref="ReadLine"/>; setting this 
		/// 	value allows the calling code to return to a previous read 
		/// 	line.  However, this property will not allow advancing to
		/// 	a previously unread line.</para>
		/// </summary>
		/// <value>
		/// 	<para>A <see cref="int"/> that is the zero-based index 
		/// 	of the next line; 0 by default; must not be less than 0.</para>
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">
		/// 	<para>The argument <paramref name="value"/> is less than 0, or
		///		greater than the number of lines returned by <see cref="ReadLine"/>.</para>
		/// </exception>
		public int NextLineIndex
		{
			[DebuggerStepThrough]
			get
			{
				return nextLineIndex;
			}
			set
			{
				if (value < 0 || value > trueReaderIndex)
				{
					throw new ArgumentOutOfRangeException("value", "Must not be less than 0 and greater than the number of lines returned by ReadLine.");
				}

				nextLineIndex = value;
			}
		}
		private int nextLineIndex = 0;
		#endregion

		#region ReadLine()
		/// <summary>
		/// 	<para>Reads the next line in the stream, identified
		///		<see cref="NextLineIndex"/>.</para>
		/// </summary>
		/// <returns>
		/// 	<para>If there are lines left in the stream, the <see cref="String"/> 
		///		containing the next line; <see langword="null"/> if the EOF has
		///		been reached.</para>
		/// </returns>
		/// <remarks>
		///		<para>A return value that is not <see langword="null"/> is 
		///		accompanied by the advancing of <see cref="NextLineIndex"/> by 1.</para>
		/// </remarks>
		public string ReadLine()
		{
			if (nextLineIndex < readLines.Count)
			{
				// Read from cache
				return readLines[nextLineIndex++];
			}
			else
			{
				// Read from stream
				string line = innerReader.ReadLine();

				if (line != null)
				{
					readLines.Add(line);
					nextLineIndex++;
					trueReaderIndex++;
				}

				return line;
			}
		}
		#endregion

		#region Dispose(Boolean)
		/// <summary>
		/// 	<para>Performs application-defined tasks associated with freeing, 
		///		releasing, or resetting unmanaged resources.</para>
		/// </summary>
		public void Dispose()
		{
			innerReader.Dispose();
		}
		#endregion
	}
}
