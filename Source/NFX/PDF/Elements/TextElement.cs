using NFX.PDF.Styling;

namespace NFX.PDF.Elements
{
	/// <summary>
	/// PDF text element
	/// </summary>
	public class TextElement : PdfElementBase
	{
		#region .ctor

		public TextElement(string content)
			:this(content, Constants.DEFAULT_FONT_SIZE, Font.Courier, Color.Black)
		{
		}

		public TextElement(string content, int fontSize, Font font)
			:this(content, fontSize, font, Color.Black)
		{
		}

		public TextElement(string content, int fontSize, Font font, Color color)
		{
			Content = content;
			FontSize = fontSize;
			Font = font;
			Color = color;
		}

		#endregion .ctor

		#region Properties

		/// <summary>
		/// Text content
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Font size
		/// </summary>
		public int FontSize { get; set; }

		/// <summary>
		/// PDF Font
		/// </summary>
		public Font Font { get; set; }

		/// <summary>
		/// PDF Color
		/// </summary>
		public Color Color { get; set; }

		#endregion Properties

		#region Public

		/// <summary>
		/// Writes element into file stream
		/// </summary>
		/// <param name="writer">PDF writer</param>
		/// <returns>Written bytes count</returns>
		public override long Write(PdfWriter writer)
		{
				return writer.Write(this);
		}

		#endregion Public
	}
}
