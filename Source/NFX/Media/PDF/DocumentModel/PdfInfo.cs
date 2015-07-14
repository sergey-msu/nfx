namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF document info
  /// </summary>
  public class PdfInfo : PdfObject
  {
    public PdfInfo(string title, string author, string creator = null)
    {
      Title = title;
      Author = author;
      Creator = creator;
    }

    /// <summary>
    /// Document title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Document author
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Document creator
    /// </summary>
    public string Creator { get; set; }
  }
}
