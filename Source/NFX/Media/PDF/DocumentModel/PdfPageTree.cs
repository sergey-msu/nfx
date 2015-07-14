using System.Collections.Generic;

namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF Page Tree document object
  /// </summary>
  internal class PdfPageTree : PdfObject
  {
    public PdfPageTree()
    {
      m_Pages = new List<PdfPage>();
    }

    private readonly List<PdfPage> m_Pages;

    public List<PdfPage> Pages
    {
      get { return m_Pages; }
    }

    /// <summary>
    /// Creates new page and adds it to the page tree
    /// </summary>
    /// <returns></returns>
    public PdfPage CreatePage(float height = Constants.DEFAULT_PAGE_HEIGHT, float width = Constants.DEFAULT_PAGE_WIDTH)
    {
      var page = new PdfPage(this, height, width);
      m_Pages.Add(page);

      return page;
    }
  }
}
