namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF document header
  /// </summary>
  internal class PdfHeader : PdfObjectBase
  {
    /// <summary>
    /// Document outlines' object Id
    /// </summary>
    public int OutlinesId { get; set; }

    /// <summary>
    /// Document info's object Id
    /// </summary>
    public int InfoId { get; set; }

    /// <summary>
    /// Document page tree's object Id
    /// </summary>
    public int PageTreeId { get; set; }
  }
}
