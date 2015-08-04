namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF document header
  /// </summary>
  public abstract class PdfObject : IPdfObject
  {
    /// <summary>
    /// Document-wide unique object Id
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Returns PDF object indirect reference
    /// </summary>
    public string GetReference()
    {
      return string.Format("{0} 0 R", ObjectId);
    }
  }
}
