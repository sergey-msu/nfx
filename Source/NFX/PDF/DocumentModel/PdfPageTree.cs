using System.Collections.Generic;

namespace NFX.PDF.DocumentModel
{
	/// <summary>
	/// PDF Page Tree document object
	/// </summary>
	internal class PdfPageTree : PdfDocumentObjectBase
	{
		#region Fields

		private readonly List<PdfPage> m_Pages;

		#endregion Fields

		#region .ctor

		public PdfPageTree()
		{
			m_Pages = new List<PdfPage>();
		}

		#endregion .ctor

		#region Properties

		/// <summary>
		/// Pages
		/// </summary>
		public List<PdfPage> Pages
		{
			get { return m_Pages; }
		}

		#endregion Properties

		#region Public

		/// <summary>
		/// Creates new page and adds it to the tree
		/// </summary>
		/// <returns></returns>
		public PdfPage CreatePage(double height = Constants.DEFAULT_PAGE_HEIGHT, double width = Constants.DEFAULT_PAGE_WIDTH)
		{
			var page = new PdfPage(this, height, width);
			m_Pages.Add(page);

			return page;
		}

		#endregion Public
	}
}
