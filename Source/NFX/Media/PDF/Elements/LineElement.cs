using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.Elements
{
  public class LineElement : PdfElementBase
  {
    #region .ctor 

    public LineElement(float x1, float y1, float x2, float y2)
      : this(x1, y1, x2, y2, Constants.DEFAULT_LINE_THICKNESS, PdfColor.Black, PdfLineStyle.Normal)
    {
    }

    public LineElement(float x1, float y1, float x2, float y2, float thickness)
      : this(x1, y1, x2, y2, thickness, PdfColor.Black, PdfLineStyle.Normal)
    {
    }

    public LineElement(float x1, float y1, float x2, float y2, float thickness, PdfColor color)
      : this(x1, y1, x2, y2, thickness, color, PdfLineStyle.Normal)
    {
    }

    public LineElement(float x1, float y1, float x2, float y2, float thickness, PdfColor color, PdfLineStyle style)
    {
      X = x1;
      Y = y1;  
      X1 = x2;
      Y1 = y2;
      Thickness = thickness;
      Color = color;
      Style = style;
    }

    #endregion .ctor

    public float X1 { get; set; }

    public float Y1 { get; set; }

    public float Thickness { get; set; }

    public PdfColor Color { get; set; }

    public PdfLineStyle Style { get; set; }

    public override long Write(PdfWriter writer)
    {
      return writer.Write(this);
    }
  }
}