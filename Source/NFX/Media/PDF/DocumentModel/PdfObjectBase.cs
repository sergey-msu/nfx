using System;

namespace NFX.Media.PDF.DocumentModel
{
	/// <summary>
	/// PDF document header
	/// </summary>
	public abstract class PdfObjectBase : IPdfObject
	{
		private int m_ObjectId;

		/// <summary>
		/// Document-wide unique object Id
		/// </summary>
		public int ObjectId
		{
			get { return m_ObjectId; }
		}

		internal virtual void SetId(ObjectIdGenerator generator)
		{
			if (m_ObjectId != 0)
				throw new InvalidOperationException("Element's object Id has already been setted.");

			m_ObjectId = generator.GenerateId();;
		}
	}
}
