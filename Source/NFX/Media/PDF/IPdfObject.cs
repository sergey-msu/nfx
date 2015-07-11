namespace NFX.Media.PDF
{
  /// <summary>
  /// Object that can be placed in PDF document
  /// </summary>
  internal interface IPdfObject
  {
    /// <summary>
    /// Document-wide unique object Id
    /// </summary>
    int ObjectId { get; set; }
  }
}