namespace NFX.Media.PDF.Styling
{
	/// <summary>
	/// Page size definitions (ISO and others)
	/// </summary>
	public class PdfPageSize
	{
		public PdfPageSize(float width, float height)
		{									
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Page's height measured in values denined in UserUnit tag of the Page
		/// (by default: 1 pt = 1/72 inch)
		/// </summary>
		public float Height { get; private set; }
			 
		/// <summary>
		/// Page's width measured in values denined in UserUnit tag of the Page
		/// (by default: 1 pt = 1/72 inch)
		/// </summary>
		public float Width { get; private set; }

		#region Predefined

		private static readonly PdfPageSize s_Letter = new PdfPageSize(612, 792);
		public static PdfPageSize Letter
		{
			get { return s_Letter; }
		}

		private static readonly PdfPageSize s_A0 = new PdfPageSize(2380, 3368);
		public static PdfPageSize A0
		{
			get { return s_A0; }
		}

		private static readonly PdfPageSize s_A1 = new PdfPageSize(1684, 2380);
		public static PdfPageSize A1
		{
			get { return s_A1; }
		}

		private static readonly PdfPageSize s_A2 = new PdfPageSize(1190, 1684);
		public static PdfPageSize A2
		{
			get { return s_A2; }
		}

		private static readonly PdfPageSize s_A3 = new PdfPageSize(842, 1190);
		public static PdfPageSize A3
		{
			get { return s_A3; }
		}

		private static readonly PdfPageSize s_A4 = new PdfPageSize(595, 842);
		public static PdfPageSize A4
		{
			get { return s_A4; }
		}

		private static readonly PdfPageSize s_A5 = new PdfPageSize(420, 595);
		public static PdfPageSize A5
		{
			get { return s_A5; }
		}

		private static readonly PdfPageSize s_B4 = new PdfPageSize(729, 1032);
		public static PdfPageSize B4
		{
			get { return s_B4; }
		}

		private static readonly PdfPageSize s_B5 = new PdfPageSize(516, 729);
		public static PdfPageSize B5
		{
			get { return s_B5; }
		}

		#endregion Predefined
	}
}