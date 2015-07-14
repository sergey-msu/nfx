using System.Collections.Generic;
using System.Text;
using NFX.Media.PDF.Styling;
using NFX.Media.PDF.Text;

namespace NFX.Media.PDF.Elements
{
  /// <summary>
  /// PDF paragraph element
  /// </summary>
  public class ParagraphElement : PdfElement
  {
    #region .ctor

    public ParagraphElement(string content, float width)
      : this(content, width, Constants.DEFAULT_PARAGRAPH_LINE_HEIGHT, Constants.DEFAULT_FONT_SIZE, PdfFont.Courier, PdfColor.Black, PdfHorizontalAlign.Left)
    {
    }

    public ParagraphElement(string content, float width, float lineHeight)
      : this(content, width, lineHeight, Constants.DEFAULT_FONT_SIZE, PdfFont.Courier, PdfColor.Black, PdfHorizontalAlign.Left)
    {
    }

    public ParagraphElement(string content, float width, float lineHeight, int fontSize)
      : this(content, width, lineHeight, fontSize, PdfFont.Courier, PdfColor.Black, PdfHorizontalAlign.Left)
    {
    }

    public ParagraphElement(string content, float width, float lineHeight, int fontSize, PdfFont font)
      : this(content, width, lineHeight, fontSize, font, PdfColor.Black, PdfHorizontalAlign.Left)
    {
    }

    public ParagraphElement(string content, float width, float lineHeight, int fontSize, PdfFont font, PdfColor color)
      : this(content, width, lineHeight, fontSize, font, color, PdfHorizontalAlign.Left)
    {
    }

    public ParagraphElement(string content, float width, float lineHeight, int fontSize, PdfFont font, PdfColor color, PdfHorizontalAlign align)
    {
      m_RawContent = content;
      m_Width = width;
      m_LineHeight = lineHeight;
      m_FontSize = fontSize;
      m_Font = font;
      m_Color = color;
      m_HorizontalAlignment = align;
      initializeLines();
    }

    #endregion .ctor

    #region Fields

    private string m_RawContent;

    private int m_FontSize;

    private PdfFont m_Font;

    private PdfColor m_Color;

    private float m_Width;

    private float m_LineHeight;

    private PdfHorizontalAlign m_HorizontalAlignment;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Text content
    /// </summary>
    internal List<ParagraphLine> Lines { get; private set; }

    /// <summary>
    /// Raw one-string paragraph content
    /// </summary>
    public string RawContent
    {
      get { return m_RawContent; }
      set
      {
        m_RawContent = value;
        initializeLines();
      }
    }

    /// <summary>
    /// Font size
    /// </summary>
    public int FontSize
    {
      get { return m_FontSize; }
      set
      {
        m_FontSize = value;
        initializeLines();
      }
    }

    /// <summary>
    /// PDF Font
    /// </summary>
    public PdfFont Font
    {
      get { return m_Font; }
      set
      {
        m_Font = value;
        initializeLines();
      }
    }

    /// <summary>
    /// PDF Color
    /// </summary>
    public PdfColor Color
    {
      get { return m_Color; }
      set
      {
        m_Color = value;
        initializeLines();
      }
    }

    /// <summary>
    /// Paragraph width
    /// </summary>
    public float Width
    {
      get { return m_Width; }
      set
      {
        m_Width = value;
        initializeLines();
      }
    }

    /// <summary>
    /// Paragraph line height
    /// </summary>
    public float LineHeight
    {
      get { return m_LineHeight; }
      set
      {
        m_LineHeight = value;
        initializeLines();
      }
    }

    /// <summary>
    /// Text horizontal alignment
    /// </summary>
    public PdfHorizontalAlign HorizontalAlignment
    {
      get { return m_HorizontalAlignment; }
      set
      {
        m_HorizontalAlignment = value;
        initializeLines();
      }
    }

    #endregion Properties

    /// <summary>
    /// Writes element into file stream
    /// </summary>
    /// <param name="writer">PDF writer</param>
    /// <returns>Written bytes count</returns>
    public override long Write(PdfWriter writer)
    {
      return writer.Write(this);
    }

    #region .pvt

    private void initializeLines()
    {
      var result = new List<ParagraphLine>();

      var rawLines = RawContent.Split(Constants.CARRIAGE_RETURN);
      foreach (var rawLine in rawLines)
      {
        var lineBuilder = new StringBuilder();
        float lineLength = 0;

        var words = rawLine.Split(Constants.SPACE);
        foreach (var word in words)
        {
          var wordWidth = TextAdapter.GetWordWidth(word + Constants.SPACE, FontSize, Font);
          if (wordWidth + lineLength > Width)
          {
            var line = createLine(lineBuilder, lineLength);
            result.Add(line);

            lineBuilder.Clear();
            lineLength = 0;
          }
          else
          {
            lineBuilder.Append(word + Constants.SPACE);
            lineLength += wordWidth;
          }
        }

        if (lineLength > 0)
        {
          var line = createLine(lineBuilder, lineLength);
          result.Add(line);
        }
      }

      Lines = result;
    }

    private ParagraphLine createLine(StringBuilder lineBuilder, float lineLength)
    {
      float leftMargin;
      switch (HorizontalAlignment)
      {
        case PdfHorizontalAlign.Right:
          leftMargin = Width - lineLength;
          break;
        case PdfHorizontalAlign.Center:
          leftMargin = (Width - lineLength) / 2;
          break;
        case PdfHorizontalAlign.Left:
        default:
          leftMargin = 0.0F;
          break;
      }

      return new ParagraphLine(lineBuilder.ToString(0, lineBuilder.Length - 1), LineHeight, leftMargin);
    }

    #endregion .pvt
  }
}
