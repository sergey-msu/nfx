using System;

namespace NFX.Media.PDF.Styling
{
  /// <summary>
  /// PDF size with its units
  /// </summary>
  public class PdfSize
  {
    public PdfSize(PdfUnit unit, float width, float height)
    {
      if (unit == null)
        throw new ArgumentException("unit should not be null");

      m_Unit = unit;
      Width = width;
      Height = height;
    }

    private PdfUnit m_Unit;

    /// <summary>
    /// Pdf unit measured in default points (1 pt = 1/72 inch)
    /// </summary>
    public PdfUnit Unit
    {
      get { return m_Unit; }
    }

    /// <summary>
    /// Height measured in values calculated according to Unit property
    /// (by default: 1 pt = 1/72 inch)
    /// </summary>
    public float Height { get; private set; }

    /// <summary>
    /// Width measured in values calculated according to Unit property
    /// (by default: 1 pt = 1/72 inch)
    /// </summary>
    public float Width { get; private set; }

    /// <summary>
    /// Change unit of the size and recalculate height and width
    /// </summary>
    /// <param name="unit">New unit</param>
    public PdfSize ChangeUnits(PdfUnit unit)
    {
      var ratio = Unit.Points / unit.Points;
      var result = new PdfSize(unit, Height * ratio, Width * ratio);

      return result;
    }
  }
}