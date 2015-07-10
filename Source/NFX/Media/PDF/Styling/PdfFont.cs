namespace NFX.Media.PDF.Styling
{
  /// <summary>
  /// PDF Font
  /// </summary>
  public class PdfFont : IPdfDocumentObject
  {
    private PdfFont(string name, int number)
    {
      m_Name = name;
      m_Number = number;
    }

    private readonly string m_Name;

    private readonly int m_Number;

    /// <summary>
    /// Document-wide unique object id
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Font name
    /// </summary>
    public string Name
    {
      get { return m_Name; }
    }

    /// <summary>
    /// Font unique number
    /// </summary>
    public int Number
    {
      get { return m_Number; }
    }

    #region Predefined

    private static readonly PdfFont s_Helvetica = new PdfFont(Constants.HELVETICA, 1);
    public static PdfFont Helvetica
    {
      get { return s_Helvetica; }
    }

    private static readonly PdfFont s_HelveticaBold = new PdfFont(Constants.HELVETICA_BOLD, 2);
    public static PdfFont HelveticaBold
    {
      get { return s_HelveticaBold; }
    }

    private static readonly PdfFont s_HelveticaOblique = new PdfFont(Constants.HELVETICA_OBLIQUE, 3);
    public static PdfFont HelveticaOblique
    {
      get { return s_HelveticaOblique; }
    }

    private static readonly PdfFont s_HelvetivaBoldOblique = new PdfFont(Constants.HELVETICA_BOLDOBLIQUE, 4);
    public static PdfFont HelvetivaBoldOblique
    {
      get { return s_HelvetivaBoldOblique; }
    }

    private static readonly PdfFont s_Courier = new PdfFont(Constants.COURIER, 5);
    public static PdfFont Courier
    {
      get { return s_Courier; }
    }

    private static readonly PdfFont s_CourierBold = new PdfFont(Constants.COURIER_BOLD, 6);
    public static PdfFont CourierBold
    {
      get { return s_CourierBold; }
    }

    private static readonly PdfFont s_CourierOblique = new PdfFont(Constants.COURIER_OBLIQUE, 7);
    public static PdfFont CourierOblique
    {
      get { return s_CourierOblique; }
    }

    private static readonly PdfFont s_CourierBoldOblique = new PdfFont(Constants.COURIER_BOLDOBLIQUE, 8);
    public static PdfFont CourierBoldOblique
    {
      get { return s_CourierBoldOblique; }
    }

    private static readonly PdfFont s_Times = new PdfFont(Constants.TIMES, 9);
    public static PdfFont Times
    {
      get { return s_Times; }
    }

    private static readonly PdfFont s_TimesBold = new PdfFont(Constants.TIMES_BOLD, 10);
    public static PdfFont TimesBold
    {
      get { return s_TimesBold; }
    }

    private static readonly PdfFont s_TimesItalic = new PdfFont(Constants.TIMES_ITALIC, 11);
    public static PdfFont TimesItalic
    {
      get { return s_TimesItalic; }
    }

    private static readonly PdfFont s_TimesBoldItalic = new PdfFont(Constants.TIMES_BOLDITALIC, 12);
    public static PdfFont TimesBoldItalic
    {
      get { return s_TimesBoldItalic; }
    }

    #endregion Predefined
  }
}
