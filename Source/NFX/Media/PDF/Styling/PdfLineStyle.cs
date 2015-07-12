namespace NFX.Media.PDF.Styling
{
  /// <summary>
  /// PDF line's style
  /// </summary>
  public class PdfLineStyle
  {
    public PdfLineStyle()
      : this(Constants.DEFAULT_LINE_THICKNESS, PdfColor.Black, PdfLineType.Normal)
    {
    }
       
    public PdfLineStyle(float thickness)
      : this(thickness, PdfColor.Black, PdfLineType.Normal)
    {
    }

    public PdfLineStyle(float thickness, PdfColor color)
      : this(thickness, color, PdfLineType.Normal)
    {
    }

    public PdfLineStyle(float thickness, PdfColor color, PdfLineType type)
    {
      Thickness = thickness;
      Color = color;
      Type = type;
    }

    public float Thickness { get; set; }

    public PdfColor Color { get; set; }

    public PdfLineType Type { get; set; }
  }
}