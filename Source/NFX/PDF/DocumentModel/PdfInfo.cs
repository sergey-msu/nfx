namespace NFX.PDF.DocumentModel
{
	/// <summary>
	/// PDF document info
	/// </summary>
	public class PdfInfo : PdfDocumentObjectBase
	{
		#region .ctor

		public PdfInfo(string title, string author, string creator = null)
		{
			Title = title;
			Author = author;
			Creator = creator;
		}

		#endregion .ctor

		#region Properties

		/// <summary>
		/// Document title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Document author
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Document creator
		/// </summary>
		public string Creator { get; set; }

		#endregion Properties
	}
}
