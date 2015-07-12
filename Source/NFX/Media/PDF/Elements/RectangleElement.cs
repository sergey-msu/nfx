using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.Elements
{
  public class RectangleElement : PdfElementBase
  {
    #region .ctor 

    public RectangleElement(float x1, float y1, float x2, float y2)
      : this(x1, y1, x2, y2, PdfColor.Gray, Constants.DEFAULT_BORDER_THICKNESS, PdfColor.White, PdfLineStyle.Normal)
    {
    } 

    public RectangleElement(float x1, float y1, float x2, float y2, PdfColor fill)
      : this(x1, y1, x2, y2, fill, Constants.DEFAULT_BORDER_THICKNESS, PdfColor.White, PdfLineStyle.Normal)
    {
    }

    public RectangleElement(float x1, float y1, float x2, float y2, PdfColor fill, float borderThickness)
      : this(x1, y1, x2, y2, fill, borderThickness, PdfColor.White, PdfLineStyle.Normal)
    {
    }

    public RectangleElement(float x1, float y1, float x2, float y2, PdfColor fill, float borderThickness, PdfColor borderColor)
      : this(x1, y1, x2, y2, fill, borderThickness, borderColor, PdfLineStyle.Normal)
    {
    }

    public RectangleElement(float x1, float y1, float x2, float y2, PdfColor fill, float borderThickness, PdfColor borderColor, PdfLineStyle borderStyle)
    {
      X = x1;
      Y = y1;
      X1 = x2;
      Y1 = y2;
      Fill = fill;
      BorderThickness = borderThickness;
      BorderColor = borderColor;
      BorderStyle = borderStyle;
    }

    #endregion .ctor

    public float X1 { get; set; }

    public float Y1 { get; set; }

    public PdfColor Fill { get; set; }

    public float BorderThickness { get; set; }

    public PdfColor BorderColor { get; set; }

    public PdfLineStyle BorderStyle { get; set; }

    public override long Write(PdfWriter writer)
    {
      return writer.Write(this);
    }
  }
}