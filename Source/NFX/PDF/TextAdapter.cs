using System.Text;

namespace NFX.PDF
{
	/// <summary>
	/// Utility class for operations with text 
	/// </summary>
	public static class TextAdapter
	{
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