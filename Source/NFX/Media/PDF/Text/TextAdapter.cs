using System;
using System.Globalization;
using System.Text;
using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.Text
{
  /// <summary>
  /// Utility class for operations with text 
  /// </summary>
  public static class TextAdapter
  {
    private static Encoding s_UnicodeEncoding;

    private static Encoding s_TrivialEncoding;

    public static Encoding UnicodeEncoding
    {
      get
      {
        if (s_UnicodeEncoding == null)
          s_UnicodeEncoding = new PdfUnicodeEncoding();

        return s_UnicodeEncoding;
      }
    }

    public static Encoding TrivialEncoding
    {
      get
      {
        if (s_TrivialEncoding == null)
          s_TrivialEncoding = new PdfTrivialEncoding();

        return s_TrivialEncoding;
      }
    }

    /// <summary>
    /// Fixes escape symbols if needed
    /// </summary>
    /// <param name="text">Input text</param>
    /// <returns>Result text</returns>
    public static string CheckText(string text)
    {
      text = text.Replace("(", @"\(");
      text = text.Replace(")", @"\)");

      return text;
    }

    /// <summary>
    /// Converts the specified byte array into a byte array representing a unicode hex string literal.
    /// </summary>
    /// <param name="bytes">The bytes of the string.</param>
    /// <returns>The PDF bytes.</returns>
    public static byte[] FormatHexStringLiteral(byte[] bytes)
    {
      var builder = new StringBuilder();
      builder.Append(Constants.HEX_OPEN);
      for (int i = 0; i < bytes.Length; i += 2)
      {
        builder.AppendFormat(Constants.HEX_STRING_PAIR, bytes[i], bytes[i + 1]);
        if (i != 0 && (i % 48) == 0)
          builder.Append(Constants.CARRIAGE_RETURN);
      }
      builder.Append(Constants.HEX_CLOSE);
      var content = builder.ToString();

      // todo: remove when unicode support will be added
      content = content.Replace("00", "");

      return TrivialEncoding.GetBytes(content);
    }

    /// <summary>
    /// Formats given float with a dot as fraction part delimeter and foors it to 4 digits after dot
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string FormatFloat(double number)
    {
      return string.Format(CultureInfo.InvariantCulture, "{0:0.####}", number);
    }

    /// <summary>
		/// Returns the lenght of a word
		/// </summary>
		/// <param name="word">Input word</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="font">PDF font</param>
		/// <returns>Size of the word</returns>
		public static float GetWordWidth(string word, int fontSize, PdfFont font)
    {
      float weight = 0;
      foreach (var letter in word)
      {
        weight += font.CharWeights[letter];
      }

      return weight * fontSize / 1000;
    }
  }
}