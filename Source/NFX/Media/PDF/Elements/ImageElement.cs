using NFX.Media.PDF.Processing;

namespace NFX.Media.PDF.Elements
{
  /// <summary>
  /// PDF image element
  /// </summary>
  public class ImageElement : PdfElementBase
  {
    /// <summary>
    /// Writes element into file stream
    /// </summary>
    /// <param name="writer">PDF writer</param>
    /// <returns>Written bytes count</returns>
    public override long Write(PdfWriter writer)
    {
      return writer.Write(this);
    }
  }
}
