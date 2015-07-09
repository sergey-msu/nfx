namespace NFX.PDF
{
	/// <summary>
	/// Object that can be placed in PDF document
	/// </summary>
	internal interface IPdfDocumentObject
	{
		/// <summary>
		/// Document-wide unique object Id
		/// </summary>
		int ObjectId { get; }
	}
}