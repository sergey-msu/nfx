using System;

namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF document header
  /// </summary>
  public abstract class PdfObject : IPdfObject
  {
    private int m_ObjectId;

    /// <summary>
    /// Document-wide unique object Id
    /// </summary>
    public int ObjectId
    {
      get { return m_ObjectId; }
    }

    internal virtual void Prepare(ObjectIdGenerator generator)
    {
      m_ObjectId = generator.GenerateId();
    }

    /// <summary>
    /// Returns PDF object indirect reference
    /// </summary>
    public string GetReference()
    {
      return string.Format("{0} 0 R", ObjectId);
    }
  }
}
