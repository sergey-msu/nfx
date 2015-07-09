namespace NFX.PDF.DocumentModel
{
	/// <summary>
	/// PDF document header
	/// </summary>
	public abstract class PdfDocumentObjectBase : IPdfDocumentObject
	{	
		#region Properties

		/// <summary>
		/// Document-wide unique object Id
		/// </summary>
		public int ObjectId { get; internal set; }
					
		#endregion Properties
	}
}
