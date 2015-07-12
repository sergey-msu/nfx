using NFX.Media.PDF.DocumentModel;

namespace NFX.Media.PDF.Elements
{
  /// <summary>
  /// Base class for all PDF primitives
  /// </summary>
  public abstract class PdfElementBase : PdfObjectBase
  {
    /// <summary>
    /// X-coordinate
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Y-coordinate
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Writes element into file stream
    /// </summary>
    /// <param name="writer">PDF writer</param>
    /// <returns>Written bytes count</returns>
    public abstract long Write(PdfWriter writer);
  }
}
