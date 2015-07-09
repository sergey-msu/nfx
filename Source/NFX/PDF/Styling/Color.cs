using System;
using System.Globalization;

namespace NFX.PDF.Styling
{
	/// <summary>
	/// PDF Color
	/// </summary>
	public class Color
	{
		#region Fields

		private const string TO_STRING_FORMAT = "{0} {1} {2}";
														 
		private readonly byte m_R;
													
		private readonly byte m_G;

		private readonly byte m_B;

		#endregion Fields

		#region .ctor

		public Color(byte r, byte g, byte b)
		{
			m_R = r;
			m_G = g;
			m_B = b;
		}

		#endregion .ctor

		#region Properties

		/// <summary>
		/// R-component 
		/// </summary>
		public byte R
		{
			get { return m_R; }
		}

		/// <summary>
		/// G-component 
		/// </summary>
		public byte G
		{
			get { return m_G; }
		}

		/// <summary>
		/// B-component 
		/// </summary>
		public byte B
		{
			get { return m_B; }
		}

		#endregion Properties

		#region Predefined

	  private static readonly Color s_Black = new Color(0, 0, 0);
		public static Color Black
		{
			get { return s_Black; }
		}

		private static readonly Color s_White = new Color(255, 255, 255);
		public static Color White
		{
			get { return s_White; }
		}

		private static readonly Color s_Red = new Color(255, 0, 0);
		public static Color Red
		{
			get { return s_Red; }
		}

		private static readonly Color s_LightRed = new Color(255, 192, 192);
		public static Color LightRed
		{
			get { return s_LightRed; }
		}

		private static readonly Color s_DarkRed = new Color(128, 0, 0);
		public static Color DarkRed
		{
			get { return s_DarkRed; }
		}

		private static readonly Color s_Orange = new Color(255, 128, 0);	
		public static Color Orange
		{
			get { return s_Orange; }
		}

		private static readonly Color s_LightOrange = new Color(255, 192, 0); 
		public static Color LightOrange
		{
			get { return s_LightOrange; }
		}

		private static readonly Color s_DarkOrange = new Color(255, 64, 0);
		public static Color DarkOrange
		{
			get { return s_DarkOrange; }
		}

		private static readonly Color s_Yellow = new Color(255, 255, 64); 
		public static Color Yellow
		{
			get { return s_Yellow; }
		}

		private static readonly Color s_LightYellow = new Color(255, 255, 192);
		public static Color LightYellow 
		{
			get { return s_LightYellow; }
		}

		private static readonly Color s_DarkYellow = new Color(255, 255, 0);
		public static Color DarkYellow
		{
			get { return s_DarkYellow; }
		}

		private static readonly Color s_Blue = new Color(0, 0, 255);
		public static Color Blue
		{
			get { return s_Blue; }
		}

		private static readonly Color s_LightBlue = new Color(26, 77, 192);
		public static Color LightBlue
		{
			get { return s_LightBlue; }
		}

		private static readonly Color s_DarkBlue = new Color(0, 0, 128);
		public static Color DarkBlue
		{
			get { return s_DarkBlue; }
		}

		private static readonly Color s_Green = new Color(0, 255, 0); 
		public static Color Green
		{
			get { return s_Green; }
		}

		private static readonly Color s_LightGreen = new Color(192, 255, 192);	
		public static Color LightGreen
		{
			get { return s_LightGreen; }
		}

		private static readonly Color s_DarkGreen = new Color(0, 128, 0); 
		public static Color DarkGreen
		{
			get { return s_DarkGreen; }
		}

		private static readonly Color s_Cyan = new Color(0, 128, 255);
		public static Color Cyan
		{
			get { return s_Cyan; }
		}

		private static readonly Color s_LightCyan = new Color(51, 205, 255);
		public static Color LghtCyan
		{
			get { return s_LightCyan; }
		}

		private static readonly Color s_DarkCyan = new Color(0, 102, 205);
		public static Color DarkCyan
		{
			get { return s_DarkCyan; }
		}

		private static readonly Color s_Purple = new Color(128, 0, 255);	
		public static Color Purple
		{
			get { return s_Purple; }
		}

		private static readonly Color s_LightPurple = new Color(192, 115, 243);
		public static Color LightPurple 
		{
			get { return s_LightPurple; }
		}

		private static readonly Color s_DarkPurple = new Color(102, 26, 128);
		public static Color DarkPurple
		{
			get { return s_DarkPurple; }
		}

		private static readonly Color s_Gray = new Color(128, 128, 128);	
		public static Color Gray
		{
			get { return s_Gray; }
		}

		private static readonly Color s_LightGray = new Color(191, 191, 191);
		public static Color LghtGray
		{
			get { return s_LightGray; }
		}

		private static readonly Color s_DarkGray = new Color(64, 64, 64);
		public static Color DarkGray
		{
			get { return s_DarkGray; }
		}

		#endregion Predefined

		public override string ToString()
		{
			var r = Math.Round((float)R / byte.MaxValue, 2).ToString(CultureInfo.InvariantCulture);
			var g = Math.Round((float)G / byte.MaxValue, 2).ToString(CultureInfo.InvariantCulture);
			var b = Math.Round((float)B / byte.MaxValue, 2).ToString(CultureInfo.InvariantCulture);

			return TO_STRING_FORMAT.Args(r, g, b);
		}
	}
}
