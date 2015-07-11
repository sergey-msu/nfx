namespace NFX.Media.PDF.Elements
{
  /// <summary>
  /// Reresents a single paragraph line
  /// </summary>
  internal class ParagraphLine
  {
    public ParagraphLine(string content, float topMargin = 0, float leftMargin = 0)
    {
      Content = content;
      TopMargin = topMargin;
      LeftMargin = leftMargin;
    }

    /// <summary>
    /// A string content of the line
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Line top margin
    /// </summary>
    public float TopMargin { get; set; }

    /// <summary>
    /// Line left margin
    /// </summary>
    public float LeftMargin { get; set; }
  }
}