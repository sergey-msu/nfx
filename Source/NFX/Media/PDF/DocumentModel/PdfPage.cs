using System.Collections.Generic;
using NFX.Media.PDF.Elements;
using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF Page
  /// </summary>
  public class PdfPage : PdfDocumentObjectBase
  {
    internal PdfPage(PdfPageTree pageTree, double height = Constants.DEFAULT_PAGE_HEIGHT, double width = Constants.DEFAULT_PAGE_WIDTH)
    {
      Height = height;
      Width = width;

      m_PageTree = pageTree;
      m_Fonts = new List<PdfFont>();
      m_Elements = new List<PdfElementBase>();
    }

    private readonly List<PdfElementBase> m_Elements;

    private readonly PdfPageTree m_PageTree;

    private readonly List<PdfFont> m_Fonts;

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

    internal List<PdfFont> Fonts
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
      return this.AddText(text, Constants.DEFAULT_FONT_SIZE, PdfFont.Courier);
    }

    /// <summary>
    /// Add raw text to the page
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="fontSize">Font size</param>
    /// <param name="font">PDF font</param>
    /// <returns>Added PDF text element</returns>
    public TextElement AddText(string text, int fontSize, PdfFont font)
    {
      return this.AddText(text, fontSize, font, PdfColor.Black);
    }

    /// <summary>
    /// Add raw text to the page
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="fontSize">Font size</param>
    /// <param name="font">PDF font</param>
    /// <param name="foreground">Text color</param>
    /// <returns>Added PDF text element</returns>
    public TextElement AddText(string text, int fontSize, PdfFont font, PdfColor foreground)
    {
      var element = new TextElement(text, fontSize, font, foreground);
      m_Elements.Add(element);

      return element;
    }

    #endregion Public
  }
}