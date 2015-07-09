using System.Collections.Generic;

namespace NFX.PDF.DocumentModel
{
	/// <summary>
	/// PDF Trailer document object
	/// </summary>
	public class PdfTrailer : PdfDocumentObjectBase
	{
		#region Fields

		private readonly List<string> m_ObjectOffsets;

		#endregion Fields

		#region .ctor

		public PdfTrailer()
		{
			m_ObjectOffsets = new List<string>();
		}

		#endregion .ctor

		#region Properties

		/// <summary>
		/// Id of the last inserted document object
		/// </summary>
		public int LastObjectId { get; set; }

		/// <summary>
		/// Inserted objects offest in PDF format
		/// </summary>
		public List<string> ObjectOffsets
		{
			get { return m_ObjectOffsets; }
		}

		/// <summary>
		/// The offset of the XREF table
		/// </summary>
		public long XRefOffset { get; set; }

		#endregion Properties

		#region Public

		/// <summary>
		/// Add inserted object offset to offsets collection
		/// </summary>
		/// <param name="offset">Offset</param>
		public void AddObjectOffset(long offset)
		{
			m_ObjectOffsets.Add(new string('0', 10 - offset.ToString().Length) + offset);
		}

		#endregion Public
	}
}
