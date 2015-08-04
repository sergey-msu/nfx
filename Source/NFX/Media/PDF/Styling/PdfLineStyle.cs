using System.Text;
using NFX.Media.PDF.Text;

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

    public override string ToString()
    {
     var styleBuilder = new StringBuilder();
      styleBuilder.AppendFormatLine("{0} w", TextAdapter.FormatFloat(Thickness));
      switch (Type)
      {
        case PdfLineType.OutlinedThin:
          styleBuilder.AppendLine("[2 2] 0 d");
          break;
        case PdfLineType.Outlined:
          styleBuilder.AppendLine("[4 4] 0 d");
          break;
        case PdfLineType.OutlinedBold:
          styleBuilder.AppendLine("[6 6] 0 d");
          break;
      }

      return styleBuilder.ToString();
    }
  }
}