﻿namespace NFX.Media.PDF.Processing
{
  /// <summary>
  /// Class that generates document-wide unique Id-s
  /// (the class is not thread-safe)
  /// </summary>
  internal class DocumentObjectIdGenerator
  {
    public DocumentObjectIdGenerator()
    {
      m_CurrentId = 0;
    }

    private int m_CurrentId;

    /// <summary>
    /// Lfst used Id
    /// </summary>
    public int CurrentId
    {
      get { return m_CurrentId; }
    }

    /// <summary>
    /// Generates new unique Id
    /// </summary>
    /// <returns></returns>
    public int GenerateId()
    {
      return ++m_CurrentId;
    }
  }
}