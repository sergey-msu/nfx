namespace NFX.PDF.Elements
{
	/// <summary>
	/// Base class for all PDF primitives
	/// </summary>
	public abstract class PdfElementBase : IPdfDocumentObject
	{
		#region Properties

		/// <summary>
		/// Document-wide unique object id
		/// </summary>
		public int ObjectId { get; set; }	

		/// <summary>
		/// X-position
		/// </summary>
		public double X { get; set; }
		
		/// <summary>
		/// Y-position
		/// </summary>
		public double Y { get; set; }

		#endregion Properties

		#region Public

		/// <summary>
		/// Writes element into file stream
		/// </summary>
		/// <param name="writer">PDF writer</param>
		/// <returns>Written bytes count</returns>
		public abstract long Write(PdfWriter writer);

		#endregion Public
	}
}
