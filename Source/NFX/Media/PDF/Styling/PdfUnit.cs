namespace NFX.Media.PDF.Styling
{
	public class PdfUnit
	{
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

		private static readonly PdfUnit s_Point = new PdfUnit(1);
		public static PdfUnit Point
		{
			get { return s_Point; }
		}

		private static readonly PdfUnit s_Inch = new PdfUnit(Constants.INCHES_IN_POINT);
		public static PdfUnit Inch
		{
			get { return s_Inch; }
		}

		private static readonly PdfUnit s_Millimeter = new PdfUnit(Constants.MILLIMETER_IN_POINT);
		public static PdfUnit Millimeter
		{
			get { return s_Millimeter; }
		}

		private static readonly PdfUnit s_Centimeter = new PdfUnit(Constants.CENTIMETER_IN_POINT);
		public static PdfUnit Centimeter
		{
			get { return s_Centimeter; }
		}

		private static readonly PdfUnit s_Presentation = new PdfUnit(Constants.PRESENTATION_IN_POINT);
		public static PdfUnit Presentation
		{
			get { return s_Presentation; }
		}

		#endregion Predefined
	}
}