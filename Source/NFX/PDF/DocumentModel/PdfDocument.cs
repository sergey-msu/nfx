using System;
using System.Collections.Generic;
using System.IO;
using NFX.PDF.Elements;
using NFX.PDF.Styling;

namespace NFX.PDF.DocumentModel
{
	/// <summary>
	/// Model for PDF document
	/// </summary>
	public class PdfDocument
	{
		#region .ctor

		public PdfDocument(string title, string author)
		{
			m_Fonts = new List<Font>();
			m_Info = new PdfInfo(title, author);
			m_OutLines = new PdfOutlines();
			m_Header = new PdfHeader();
			m_PageTree = new PdfPageTree();
			m_Trailer = new PdfTrailer();
		}

		#endregion .ctor
		
		#region Fields

		private readonly List<Font> m_Fonts;

		private readonly PdfHeader m_Header;

		private readonly PdfInfo m_Info;

		private readonly PdfOutlines m_OutLines;

		private readonly PdfPageTree m_PageTree;

		private readonly PdfTrailer m_Trailer;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Used fonts
		/// </summary>
		public List<Font> Fonts
		{
			get { return m_Fonts; }
		}

		/// <summary>
		/// Document info
		/// </summary>
		public PdfInfo Info
		{
			get { return m_Info; }
		}

		/// <summary>
		/// Document outlines
		/// </summary>
		public PdfOutlines Outlines
		{
			get { return m_OutLines; }
		}

		/// <summary>
		/// Document header
		/// </summary>
		internal PdfHeader Header
		{
			get { return m_Header; }
		}

		/// <summary>
		/// Document pages
		/// </summary>
		public List<PdfPage> Pages
		{
			get { return m_PageTree.Pages; }
		}

		/// <summary>
		/// Document trailer
		/// </summary>
		public PdfTrailer Trailer
		{
			get { return m_Trailer; }
		}

		/// <summary>
		/// Document page tree
		/// </summary>
		internal PdfPageTree PageTree
		{
			get { return m_PageTree; }
		}

		#endregion Properties

		#region Public

		/// <summary>
		/// Adds new page to document
		/// </summary>
		/// <returns>Page</returns>
		public PdfPage AddPage(double height = Constants.DEFAULT_PAGE_HEIGHT, double width = Constants.DEFAULT_PAGE_WIDTH)
		{
			return m_PageTree.CreatePage(height, width);
		}

		/// <summary>
		/// Save document to file
		/// </summary>
		/// <param name="filePath">File path</param>
		public void Save(string filePath)
		{
			using (var file = new FileStream(filePath, FileMode.Create))
			{
				Prepare();

				var writer = new PdfWriter(file);
				writer.Write(this);
			}
		}

		#endregion Public

		#region .pvt

		/// <summary>
		/// Supplies document objects with unique sequential ids
		/// </summary>
		private void Prepare()
		{
			int objectId = 1;

			m_Header.ObjectId = objectId++;

			m_Info.ObjectId = objectId++;
			m_Header.InfoId = m_Info.ObjectId;

			m_OutLines.ObjectId = objectId++;
			m_Header.OutlinesId = m_OutLines.ObjectId;

			foreach (var font in Fonts)
			{
				font.ObjectId = objectId++;
			}

			m_PageTree.ObjectId = objectId++;
			m_Header.PageTreeId = m_PageTree.ObjectId;

			foreach (var page in Pages)
			{
				page.ObjectId = objectId++;
				page.Fonts.AddRange(Fonts);

				foreach (var element in page.Elements)
				{
					element.ObjectId = objectId++;
					if (element is ImageElement)
					{
						throw new NotImplementedException();
					}
				}
			}

			m_Trailer.LastObjectId = objectId - 1;
		}

		#endregion .pvt
	}
}
