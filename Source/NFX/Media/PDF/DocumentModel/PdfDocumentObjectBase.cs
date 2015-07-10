namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF document header
  /// </summary>
  public abstract class PdfDocumentObjectBase : IPdfDocumentObject
  {
    /// <summary>
    /// Document-wide unique object Id
    /// </summary>
    public int ObjectId { get; set; }
  }
}
