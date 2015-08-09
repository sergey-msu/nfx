namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF Font
  /// </summary>
  public class PdfFont : PdfObject, IPdfResource
  {
    #region CONSTS

    private const string FONT_REFERENCE_FORMAT = "/F{0}";

    private const string HELVETICA = "Helvetica";
    private const string HELVETICA_BOLD = "Helvetica-Bold";
    private const string HELVETICA_OBLIQUE = "Helvetica-Oblique";
    private const string HELVETICA_BOLDOBLIQUE = "Helvetica-BoldOblique";
    private const string COURIER = "Courier";
    private const string COURIER_BOLD = "Courier-Bold";
    private const string COURIER_OBLIQUE = "Courier-Oblique";
    private const string COURIER_BOLDOBLIQUE = "Courier-BoldOblique";
    private const string TIMES = "Times-Roman";
    private const string TIMES_BOLD = "Times-Bold";
    private const string TIMES_ITALIC = "Times-Italic";
    private const string TIMES_BOLDITALIC = "Times-BoldItalic";

    #endregion CONSTS

    public PdfFont(string name)
    {
      m_Name = name;
    }

    private readonly string m_Name;

    private int m_Number;

    /// <summary>
    /// Font name
    /// </summary>
    public string Name
    {
      get { return m_Name; }
    }

    /// <summary>
    /// Font unique resource number
    /// </summary>
    public int Number
    {
      get { return m_Number; }
    }

    /// <summary>
    /// Document-wide unique resource Id
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// Returns PDF object indirect reference
    /// </summary>
    public string GetResourceReference()
    {
      return FONT_REFERENCE_FORMAT.Args(ResourceId);
    }

    #region Predefined

    private static readonly PdfFont s_Helvetica = new PdfFont(HELVETICA);
    public static PdfFont Helvetica
    {
      get { return s_Helvetica; }
    }

    private static readonly PdfFont s_HelveticaBold = new PdfFont(HELVETICA_BOLD);
    public static PdfFont HelveticaBold
    {
      get { return s_HelveticaBold; }
    }

    private static readonly PdfFont s_HelveticaOblique = new PdfFont(HELVETICA_OBLIQUE);
    public static PdfFont HelveticaOblique
    {
      get { return s_HelveticaOblique; }
    }

    private static readonly PdfFont s_HelveticaBoldOblique = new PdfFont(HELVETICA_BOLDOBLIQUE);
    public static PdfFont HelveticaBoldOblique
    {
      get { return s_HelveticaBoldOblique; }
    }

    private static readonly PdfFont s_Courier = new PdfFont(COURIER);
    public static PdfFont Courier
    {
      get { return s_Courier; }
    }

    private static readonly PdfFont s_CourierBold = new PdfFont(COURIER_BOLD);
    public static PdfFont CourierBold
    {
      get { return s_CourierBold; }
    }

    private static readonly PdfFont s_CourierOblique = new PdfFont(COURIER_OBLIQUE);
    public static PdfFont CourierOblique
    {
      get { return s_CourierOblique; }
    }

    private static readonly PdfFont s_CourierBoldOblique = new PdfFont(COURIER_BOLDOBLIQUE);
    public static PdfFont CourierBoldOblique
    {
      get { return s_CourierBoldOblique; }
    }

    private static readonly PdfFont s_Times = new PdfFont(TIMES);
    public static PdfFont Times
    {
      get { return s_Times; }
    }

    private static readonly PdfFont s_TimesBold = new PdfFont(TIMES_BOLD);
    public static PdfFont TimesBold
    {
      get { return s_TimesBold; }
    }

    private static readonly PdfFont s_TimesItalic = new PdfFont(TIMES_ITALIC);
    public static PdfFont TimesItalic
    {
      get { return s_TimesItalic; }
    }

    private static readonly PdfFont s_TimesBoldItalic = new PdfFont(TIMES_BOLDITALIC);
    public static PdfFont TimesBoldItalic
    {
      get { return s_TimesBoldItalic; }
    }

    #endregion Predefined
  }
}
