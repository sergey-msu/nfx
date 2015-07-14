using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.Elements
{
  /// <summary>
  /// PDF circle element
  /// </summary>
  public class CircleElement : PdfElement
  {
    #region .ctor 

    public CircleElement(float x, float y, float r, PdfColor fill)
      : this(x, y, r, fill, new PdfLineStyle())
    {
    }

    public CircleElement(float x, float y, float r, PdfColor fill, PdfLineStyle borderStyle)
    {
      X = x;
      Y = y;
      R = r;
      Fill = fill;
      BorderStyle = borderStyle;
    }

    #endregion .ctor

    /// <summary>
    /// Circle center's X-coordinate
    /// </summary>
    public float CenterX
    {
      get { return X + R; }
    }
    
    /// <summary>
    /// Circle center's Y-coordinate
    /// </summary>
    public float CenterY
    {
      get { return Y - R; }
    }

    public float R { get; set; }

    public PdfColor Fill { get; set; }

    public PdfLineStyle BorderStyle { get; set; }

    public override long Write(PdfWriter writer)
    {
      return writer.Write(this);
    }
  }
}