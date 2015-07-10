using System.Text;

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
		public static byte[] FormatStringLiteral(byte[] bytes)
		{
			var buider = new StringBuilder();
			buider.Append(Constants.HEX_OPEN);
			for (int i = 0; i < bytes.Length; i += 2)
			{
				buider.AppendFormat(Constants.HEX_STRING_PAIR, bytes[i], bytes[i + 1]);
				if (i != 0 && (i % 48) == 0)
					buider.Append(Constants.CARRIAGE_RETURN);
			}
			buider.Append(Constants.HEX_CLOSE);

			return TrivialEncoding.GetBytes(buider.ToString());
		}

		/// <summary>
		/// Encodes input text in hex format
		/// </summary>
		/// <param name="text">Input text</param>
		/// <returns>Output hex text</returns>
		public static string EncodeHex(string text)
		{
			var builder = new StringBuilder();

			var chars = text.ToCharArray();
			string hex;
			for (int i = 0; i < chars.Length; i++)
			{
				hex = ((byte)chars[i]).ToString("X2");
				builder.Append(hex);
			}

			return builder.ToString();
		}
	}
}