using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.Elements
{
  public class RectangleElement : PdfElement
  {
    #region .ctor 

    public RectangleElement(float x1, float y1, float x2, float y2, PdfColor fill)
      : this(x1, y1, x2, y2, fill, new PdfLineStyle())
    {
    }

    public RectangleElement(float x1, float y1, float x2, float y2, PdfColor fill, PdfLineStyle borderStyle)
    {
      X = x1;
      Y = y1;
      X1 = x2;
      Y1 = y2;
      Fill = fill;
      BorderStyle = borderStyle;
    }

    #endregion .ctor

    public float X1 { get; set; }

    public float Y1 { get; set; }

    public PdfColor Fill { get; set; }

    public PdfLineStyle BorderStyle { get; set; }

    public override void Write(PdfWriter writer)
    {
      writer.Write(this);
    }
  }
}