using System.Collections.Generic;
using NFX.PDF.Elements;
using NFX.PDF.Styling;

namespace NFX.PDF.DocumentModel
{
	/// <summary>
	/// PDF Page
	/// </summary>
	public class PdfPage : PdfDocumentObjectBase
	{
		#region .ctor

		internal PdfPage(PdfPageTree pageTree, double height = Constants.DEFAULT_PAGE_HEIGHT, double width = Constants.DEFAULT_PAGE_WIDTH)
		{
			Height = height;
			Width = width;

			m_PageTree = pageTree;
			m_Fonts = new List<Font>();
			m_Elements = new List<PdfElementBase>();
		}

		#endregion .ctor
		
		#region Fields

		private readonly List<PdfElementBase> m_Elements;

		private readonly PdfPageTree m_PageTree;

		private readonly List<Font> m_Fonts;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Page's height
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		/// Page's width
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// Page elements
		/// </summary>
		public List<PdfElementBase> Elements
		{
			get { return m_Elements; }
		}

		/// <summary>
		/// Page tree
		/// </summary>
		internal PdfPageTree PageTree
		{
			get { return m_PageTree; }
		}

		internal List<Font> Fonts
		{
			get { return m_Fonts; }
		}

		#endregion Properties

		#region Public

		/// <summary>
		/// Add raw text to the page
		/// </summary>
		/// <param name="text">Text</param>
		/// <returns>Added PDF text element</returns>
		public TextElement AddText(string text)
		{
			return this.AddText(text, Constants.DEFAULT_FONT_SIZE, Font.Courier);
		}

		/// <summary>
		/// Add raw text to the page
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="fontSize">Font size</param>
		/// <param name="font">PDF font</param>
		/// <returns>Added PDF text element</returns>
		public TextElement AddText(string text, int fontSize, Font font)
		{
			return this.AddText(text, fontSize, font, Color.Black);
		}

		/// <summary>
		/// Add raw text to the page
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="fontSize">Font size</param>
		/// <param name="font">PDF font</param>
		/// <param name="foreground">Text color</param>
		/// <returns>Added PDF text element</returns>
		public TextElement AddText(string text, int fontSize, Font font, Color foreground)
		{
			var element = new TextElement(text, fontSize, font, foreground);
			m_Elements.Add(element);

			return element;
		}

		#endregion Public
	}
}