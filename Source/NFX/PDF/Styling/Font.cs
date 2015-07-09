namespace NFX.PDF.Styling
{
	/// <summary>
	/// PDF Font
	/// </summary>
	public class Font : IPdfDocumentObject
	{
		#region Fields

		private readonly string m_Name;

		private readonly int m_Number;

		#endregion Fileds

		#region .ctor

		private Font(string name, int number)
		{
			m_Name = name;
			m_Number = number;
		}

		#endregion .ctor

		#region Properties

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

		#endregion Properties

		#region Predefined

		private static readonly Font s_Helvetica = new Font(Constants.HELVETICA, 1);
		public static Font Helvetica
		{
			get { return s_Helvetica; }
		}

		private static readonly Font s_HelveticaBold = new Font(Constants.HELVETICA_BOLD, 2);
		public static Font HelveticaBold
		{
			get { return s_HelveticaBold; }
		}

		private static readonly Font s_HelveticaOblique = new Font(Constants.HELVETICA_OBLIQUE, 3);
		public static Font HelveticaOblique
		{
			get { return s_HelveticaOblique; }
		}

		private static readonly Font s_HelvetivaBoldOblique = new Font(Constants.HELVETICA_BOLDOBLIQUE, 4);
		public static Font HelvetivaBoldOblique
		{
			get { return s_HelvetivaBoldOblique; }
		}

		private static readonly Font s_Courier = new Font(Constants.COURIER, 5);
		public static Font Courier
		{
			get { return s_Courier; }
		}

		private static readonly Font s_CourierBold = new Font(Constants.COURIER_BOLD, 6);
		public static Font CourierBold
		{
			get { return s_CourierBold; }
		}

		private static readonly Font s_CourierOblique = new Font(Constants.COURIER_OBLIQUE, 7);
		public static Font CourierOblique
		{
			get { return s_CourierOblique; }
		}

		private static readonly Font s_CourierBoldOblique = new Font(Constants.COURIER_BOLDOBLIQUE, 8);
		public static Font CourierBoldOblique
		{
			get { return s_CourierBoldOblique; }
		}

		private static readonly Font s_Times = new Font(Constants.TIMES, 9);
		public static Font Times
		{
			get { return s_Times; }
		}

		private static readonly Font s_TimesBold = new Font(Constants.TIMES_BOLD, 10);
		public static Font TimesBold
		{
			get { return s_TimesBold; }
		}

		private static readonly Font s_TimesItalic = new Font(Constants.TIMES_ITALIC, 11);
		public static Font TimesItalic
		{
			get { return s_TimesItalic; }
		}

		private static readonly Font s_TimesBoldItalic = new Font(Constants.TIMES_BOLDITALIC, 12);
		public static Font TimesBoldItalic
		{
			get { return s_TimesBoldItalic; }
		}

		#endregion Predefined
	}
}
