using System;
using System.Globalization;
using NFX.Media.PDF.Text;

namespace NFX.Media.PDF.Styling
{
  /// <summary>
  /// PDF Color
  /// </summary>
  public class PdfColor
  {
    private const string TO_STRING_FORMAT = "{0} {1} {2}";

    private PdfColor(byte r, byte g, byte b)
    {
      m_R = r;
      m_G = g;
      m_B = b;
    }

    private readonly byte m_R;

    private readonly byte m_G;

    private readonly byte m_B;

    public byte R
    {
      get { return m_R; }
    }

    public byte G
    {
      get { return m_G; }
    }

    public byte B
    {
      get { return m_B; }
    }

    public static PdfColor FromRgb(byte r, byte g, byte b)
    {
      return new PdfColor(r, g, b);
    }

    #region Predefined

    private static readonly PdfColor s_Black = new PdfColor(0, 0, 0);
    public static PdfColor Black
    {
      get { return s_Black; }
    }

    private static readonly PdfColor s_White = new PdfColor(255, 255, 255);
    public static PdfColor White
    {
      get { return s_White; }
    }

    private static readonly PdfColor s_Red = new PdfColor(255, 0, 0);
    public static PdfColor Red
    {
      get { return s_Red; }
    }

    private static readonly PdfColor s_LightRed = new PdfColor(255, 192, 192);
    public static PdfColor LightRed
    {
      get { return s_LightRed; }
    }

    private static readonly PdfColor s_DarkRed = new PdfColor(128, 0, 0);
    public static PdfColor DarkRed
    {
      get { return s_DarkRed; }
    }

    private static readonly PdfColor s_Orange = new PdfColor(255, 128, 0);
    public static PdfColor Orange
    {
      get { return s_Orange; }
    }

    private static readonly PdfColor s_LightOrange = new PdfColor(255, 192, 0);
    public static PdfColor LightOrange
    {
      get { return s_LightOrange; }
    }

    private static readonly PdfColor s_DarkOrange = new PdfColor(255, 64, 0);
    public static PdfColor DarkOrange
    {
      get { return s_DarkOrange; }
    }

    private static readonly PdfColor s_Yellow = new PdfColor(255, 255, 64);
    public static PdfColor Yellow
    {
      get { return s_Yellow; }
    }

    private static readonly PdfColor s_LightYellow = new PdfColor(255, 255, 192);
    public static PdfColor LightYellow
    {
      get { return s_LightYellow; }
    }

    private static readonly PdfColor s_DarkYellow = new PdfColor(255, 255, 0);
    public static PdfColor DarkYellow
    {
      get { return s_DarkYellow; }
    }

    private static readonly PdfColor s_Blue = new PdfColor(0, 0, 255);
    public static PdfColor Blue
    {
      get { return s_Blue; }
    }

    private static readonly PdfColor s_LightBlue = new PdfColor(26, 77, 192);
    public static PdfColor LightBlue
    {
      get { return s_LightBlue; }
    }

    private static readonly PdfColor s_DarkBlue = new PdfColor(0, 0, 128);
    public static PdfColor DarkBlue
    {
      get { return s_DarkBlue; }
    }

    private static readonly PdfColor s_Green = new PdfColor(0, 255, 0);
    public static PdfColor Green
    {
      get { return s_Green; }
    }

    private static readonly PdfColor s_LightGreen = new PdfColor(192, 255, 192);
    public static PdfColor LightGreen
    {
      get { return s_LightGreen; }
    }

    private static readonly PdfColor s_DarkGreen = new PdfColor(0, 128, 0);
    public static PdfColor DarkGreen
    {
      get { return s_DarkGreen; }
    }

    private static readonly PdfColor s_Cyan = new PdfColor(0, 128, 255);
    public static PdfColor Cyan
    {
      get { return s_Cyan; }
    }

    private static readonly PdfColor s_LightCyan = new PdfColor(51, 205, 255);
    public static PdfColor LghtCyan
    {
      get { return s_LightCyan; }
    }

    private static readonly PdfColor s_DarkCyan = new PdfColor(0, 102, 205);
    public static PdfColor DarkCyan
    {
      get { return s_DarkCyan; }
    }

    private static readonly PdfColor s_Purple = new PdfColor(128, 0, 255);
    public static PdfColor Purple
    {
      get { return s_Purple; }
    }

    private static readonly PdfColor s_LightPurple = new PdfColor(192, 115, 243);
    public static PdfColor LightPurple
    {
      get { return s_LightPurple; }
    }

    private static readonly PdfColor s_DarkPurple = new PdfColor(102, 26, 128);
    public static PdfColor DarkPurple
    {
      get { return s_DarkPurple; }
    }

    private static readonly PdfColor s_Gray = new PdfColor(128, 128, 128);
    public static PdfColor Gray
    {
      get { return s_Gray; }
    }

    private static readonly PdfColor s_LightGray = new PdfColor(191, 191, 191);
    public static PdfColor LghtGray
    {
      get { return s_LightGray; }
    }

    private static readonly PdfColor s_DarkGray = new PdfColor(64, 64, 64);
    public static PdfColor DarkGray
    {
      get { return s_DarkGray; }
    }

    #endregion Predefined

    public override string ToString()
    {
      var r = TextAdapter.FormatFloat((float)R / byte.MaxValue);
      var g = TextAdapter.FormatFloat((float)G / byte.MaxValue);
      var b = TextAdapter.FormatFloat((float)B / byte.MaxValue);

      return TO_STRING_FORMAT.Args(r, g, b);
    }
  }
}
