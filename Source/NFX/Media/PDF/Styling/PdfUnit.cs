namespace NFX.Media.PDF.Styling
{
  public class PdfUnit
  {
    #region CONSTS

    private const float POINTS_IN_INCH = 72;
    private const float POINTS_IN_MILLIMETER = 72 / 25.4F;
    private const float POINTS_IN_CENTIMETER = 72 / 2.54F;

    #endregion CONSTS

    public PdfUnit(float points)
    {
      m_Points = points;
    }

    private readonly float m_Points;

    /// <summary>
    /// Number of default user space units in current unit
    /// (1 default user space unit is 1/72 inch)
    /// </summary>
    public float Points
    {
      get { return m_Points; }
    }

    #region Predefined

    public static PdfUnit Default
    {
      get { return Point; }
    }

    private static readonly PdfUnit s_Point = new PdfUnit(1);
    public static PdfUnit Point
    {
      get { return s_Point; }
    }

    private static readonly PdfUnit s_Inch = new PdfUnit(POINTS_IN_INCH);
    public static PdfUnit Inch
    {
      get { return s_Inch; }
    }

    private static readonly PdfUnit s_Millimeter = new PdfUnit(POINTS_IN_MILLIMETER);
    public static PdfUnit Millimeter
    {
      get { return s_Millimeter; }
    }

    private static readonly PdfUnit s_Centimeter = new PdfUnit(POINTS_IN_CENTIMETER);
    public static PdfUnit Centimeter
    {
      get { return s_Centimeter; }
    }

    #endregion Predefined
  }
}