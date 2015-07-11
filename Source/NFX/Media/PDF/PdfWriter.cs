using System;
using System.IO;
using System.Text;
using NFX.Media.PDF.DocumentModel;
using NFX.Media.PDF.Elements;
using NFX.Media.PDF.Styling;
using NFX.Media.PDF.Text;

namespace NFX.Media.PDF
{
  /// <summary>
  /// Class that aggregates PDF format-specific writing logic
  /// </summary>
  public class PdfWriter
  {
    public PdfWriter(Stream stream)
    {
      m_Stream = stream;
    }

    private readonly Stream m_Stream;

    #region Public

    /// <summary>
    /// Writes PDF document into file stream
    /// </summary>
    /// <param name="document">PDF document</param>
    public void Write(PdfDocument document)
    {
      long position = 0;

      // definition
      string meta = "%PDF-1.4{0}".Args(Constants.RETURN);
      position += WriteRaw(meta);

      // header object
      document.Trailer.AddObjectOffset(position);
      position += Write(document.Header);

      // info object
      document.Trailer.AddObjectOffset(position);
      position += Write(document.Info);

      // outlines object
      document.Trailer.AddObjectOffset(position);
      position += Write(document.Outlines);

      // fonts
      foreach (var font in document.Fonts)
      {
        document.Trailer.AddObjectOffset(position);
        position += Write(font);
      }

      // page tree
      document.Trailer.AddObjectOffset(position);
      position += Write(document.PageTree);

      // pages
      foreach (var page in document.Pages)
      {
        document.Trailer.AddObjectOffset(position);
        position += Write(page);

        // elements
        foreach (var element in page.Elements)
        {
          document.Trailer.AddObjectOffset(position);
          position += element.Write(this);

          var image = element as ImageElement;
          if (image != null)
          {
            throw new NotImplementedException();
          }
        }
      }

      // trailer 
      document.Trailer.XRefOffset = position;
      Write(document.Trailer);
    }

    /// <summary>
    /// Writes PDF font into file stream 
    /// </summary>
    /// <param name="font">PDF font</param>
    /// <returns>Written bytes count</returns>
    public long Write(PdfFont font)
    {
      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, font.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Type /Font{0}", Constants.RETURN);
      builder.AppendFormat("/Subtype /Type1{0}", Constants.RETURN);
      builder.AppendFormat("/Name /F{1}{0}", Constants.RETURN, font.Number);
      builder.AppendFormat("/BaseFont /{1}{0}", Constants.RETURN, font.Name);
      builder.AppendFormat("/Encoding /WinAnsiEncoding{0}", Constants.RETURN);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF header into file stream 
    /// </summary>
    /// <param name="header">PDF document header</param>
    /// <returns>Written bytes count</returns>
    internal long Write(PdfHeader header)
    {
      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, header.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Type /Catalog{0}", Constants.RETURN);
      builder.AppendFormat("/Version /1.4{0}", Constants.RETURN);
      builder.AppendFormat("/Pages {1} 0 R{0}", Constants.RETURN, header.PageTreeId);
      builder.AppendFormat("/Outlines {1} 0 R{0}", Constants.RETURN, header.OutlinesId);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF header into file stream
    /// </summary>
    /// <param name="info">PDF document info</param>
    /// <returns>Written bytes count</returns>
    public long Write(PdfInfo info)
    {
      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, info.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Title ({1}){0}", Constants.RETURN, info.Title);
      builder.AppendFormat("/Author ({1}){0}", Constants.RETURN, info.Author);
      builder.AppendFormat("/Creator ({1}){0}", Constants.RETURN, info.Creator);
      builder.AppendFormat("/CreationDate ({1:yyyyMMdd}){0}", Constants.RETURN, DateTime.Now);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF document outlines into file stream
    /// </summary>
    /// <param name="outlines">PDF document outlines</param>
    /// <returns>Written bytes count</returns>
    public long Write(PdfOutlines outlines)
    {
      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, outlines.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Type /Outlines{0}", Constants.RETURN);
      builder.AppendFormat("/Count 0{0}", Constants.RETURN);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF document page tree into file stream
    /// </summary>
    /// <param name="pageTree">PDF document page tree</param>
    /// <returns>Written bytes count</returns>
    internal long Write(PdfPageTree pageTree)
    {
      if (pageTree.Pages.Count == 0)
        return 0;

      var pageListBuilder = new StringBuilder();
      foreach (var page in pageTree.Pages)
      {
        pageListBuilder.AppendFormat("{0} 0 R ", page.ObjectId);
      }

      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, pageTree.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Type /Pages{0}", Constants.RETURN);
      builder.AppendFormat("/Count {1}{0}", Constants.RETURN, pageTree.Pages.Count);
      builder.AppendFormat("/Kids [{1}]{0}", Constants.RETURN, pageListBuilder);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF page into file stream
    /// </summary>
    /// <param name="page">PDF page</param>
    /// <returns>Written bytes count</returns>
    public long Write(PdfPage page)
    {
      var elementBuilder = new StringBuilder();
      var imageBuilder = new StringBuilder();

      foreach (var element in page.Elements)
      {
        elementBuilder.AppendFormat("{0} 0 R ", element.ObjectId);
        var image = element as ImageElement;
        if (image != null)
        {
          throw new NotImplementedException();
        }
      }

      var resultString = new StringBuilder();
      foreach (var font in page.Fonts)
      {
        resultString.AppendFormat("/F{0} {1} 0 R ", font.Number, font.ObjectId);
      }

      var pageBuilder = new StringBuilder();
      pageBuilder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, page.ObjectId);
      pageBuilder.AppendFormat("<<{0}", Constants.RETURN);
      pageBuilder.AppendFormat("/Type /Page{0}", Constants.RETURN);
      pageBuilder.AppendFormat("/Parent {1} 0 R{0}", Constants.RETURN, page.PageTree.ObjectId);
      pageBuilder.AppendFormat("/Resources <</Font <<{1}>>{0}", Constants.RETURN, resultString);
      if (imageBuilder.Length > 0)
      {
        pageBuilder.AppendFormat("/XObject <<{1}>>{0}", Constants.RETURN, imageBuilder);
      }
      pageBuilder.AppendFormat(">>{0}", Constants.RETURN);
      pageBuilder.AppendFormat("/MediaBox [0 0 {1} {2}]{0}", Constants.RETURN, page.Width, page.Height);
      pageBuilder.AppendFormat("/CropBox [0 0 {1} {2}]{0}", Constants.RETURN, page.Width, page.Height);
      pageBuilder.AppendFormat("/Rotate 0{0}", Constants.RETURN);
      pageBuilder.AppendFormat("/ProcSet [/PDF /Text /ImageC]{0}", Constants.RETURN);
      if (elementBuilder.Length > 0)
      {
        pageBuilder.AppendFormat("/Contents [{1}]{0}", Constants.RETURN, elementBuilder);
      }
      pageBuilder.AppendFormat(">>{0}", Constants.RETURN);
      pageBuilder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(pageBuilder.ToString());
    }

    /// <summary>
    /// Writes PDF document trailer into file stream
    /// </summary>
    /// <param name="trailer">PDF document trailer</param>
    /// <returns>Written bytes count</returns>
    public long Write(PdfTrailer trailer)
    {
      StringBuilder builder = new StringBuilder();
      builder.AppendFormat("xref{0}", Constants.RETURN);
      builder.AppendFormat("0 {1}{0}", Constants.RETURN, trailer.LastObjectId + 1);
      builder.AppendFormat("0000000000 65535 f{0}", Constants.RETURN);
      foreach (var offset in trailer.ObjectOffsets)
      {
        builder.AppendFormat("{1} 00000 n{0}", Constants.RETURN, offset);
      }
      builder.AppendFormat("trailer{0}", Constants.RETURN);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Size {1}{0}", Constants.RETURN, trailer.LastObjectId + 1);
      builder.AppendFormat("/Root 1 0 R{0}", Constants.RETURN);
      builder.AppendFormat("/Info 2 0 R{0}", Constants.RETURN);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("startxref{0}", Constants.RETURN);
      builder.AppendFormat("{1}{0}", Constants.RETURN, trailer.XRefOffset);
      builder.Append("%%EOF");

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF image element into file stream
    /// </summary>
    /// <param name="image">PDF image element</param>
    /// <returns>Written bytes count</returns>
    public long Write(ImageElement image)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Writes PDF text element into file stream
    /// </summary>
    /// <param name="text">PDF text element</param>
    /// <returns>Written bytes count</returns>
    public long Write(TextElement text)
    {
      var checkedText = TextAdapter.CheckText(text.Content);
      var bytes = TextAdapter.UnicodeEncoding.GetBytes(checkedText);
      bytes = TextAdapter.FormatHexStringLiteral(bytes);
      var unicodeContent = TextAdapter.TrivialEncoding.GetString(bytes, 0, bytes.Length);

      var pdfStreamBuilder = new StringBuilder();
      pdfStreamBuilder.AppendFormat("q{0}", Constants.RETURN);
      pdfStreamBuilder.AppendFormat("BT{0}", Constants.RETURN);
      pdfStreamBuilder.AppendFormat("/F{1} {2} Tf{0}", Constants.RETURN, text.Font.Number, text.FontSize);
      pdfStreamBuilder.AppendFormat("{1} rg{0}", Constants.RETURN, text.Color);
      pdfStreamBuilder.AppendFormat("{1} {2} Td {3} Tj{0}", Constants.RETURN, text.X, text.Y, unicodeContent);
      pdfStreamBuilder.AppendFormat("ET{0}", Constants.RETURN);
      pdfStreamBuilder.Append("Q");

      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, text.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Length {1}{0}", Constants.RETURN, pdfStreamBuilder.Length);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("stream{0}", Constants.RETURN);
      builder.AppendFormat("{1}{0}", Constants.RETURN, pdfStreamBuilder);
      builder.AppendFormat("endstream{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes PDF paragraph element into file stream
    /// </summary>
    /// <param name="paragraph">PDF paragraph element</param>
    /// <returns>Written bytes count</returns>
    public long Write(ParagraphElement paragraph)
    {
      var pdfStreamBuilder = new StringBuilder();
      pdfStreamBuilder.AppendFormat("q{0}", Constants.RETURN);
      pdfStreamBuilder.AppendFormat("BT{0}", Constants.RETURN);
      pdfStreamBuilder.AppendFormat("/F{1} {2} Tf{0}", Constants.RETURN, paragraph.Font.Number, paragraph.FontSize);
      pdfStreamBuilder.AppendFormat("{1} rg{0}", Constants.RETURN, paragraph.Color);
      pdfStreamBuilder.AppendFormat("{1} {2} Td {0}", Constants.RETURN, paragraph.X, paragraph.Y);
      pdfStreamBuilder.AppendFormat("14 TL{0}", Constants.RETURN);
      foreach (var line in paragraph.Lines)
      {
        var checkedLine = TextAdapter.CheckText(line.Content);
        var bytes = TextAdapter.UnicodeEncoding.GetBytes(checkedLine);
        bytes = TextAdapter.FormatHexStringLiteral(bytes);
        var unicodeContent = TextAdapter.TrivialEncoding.GetString(bytes, 0, bytes.Length);

        pdfStreamBuilder.AppendFormat("{1} -{2} Td {3} Tj{0}-{1} 0 Td{0}", Constants.RETURN, TextAdapter.FormatFloat(line.LeftMargin), TextAdapter.FormatFloat(line.TopMargin), unicodeContent);
      }
      pdfStreamBuilder.AppendFormat("ET{0}", Constants.RETURN);
      pdfStreamBuilder.Append("Q");

      var builder = new StringBuilder();
      builder.AppendFormat("{1} 0 obj{0}", Constants.RETURN, paragraph.ObjectId);
      builder.AppendFormat("<<{0}", Constants.RETURN);
      builder.AppendFormat("/Length {1}{0}", Constants.RETURN, pdfStreamBuilder.Length);
      builder.AppendFormat(">>{0}", Constants.RETURN);
      builder.AppendFormat("stream{0}", Constants.RETURN);
      builder.AppendFormat("{1}{0}", Constants.RETURN, pdfStreamBuilder);
      builder.AppendFormat("endstream{0}", Constants.RETURN);
      builder.AppendFormat("endobj{0}", Constants.RETURN);

      return WriteRaw(builder.ToString());
    }

    /// <summary>
    /// Writes raw string into file stream
    /// </summary>
    /// <param name="text">Raw PDF string</param>
    /// <returns>Written bytes count</returns>
    public long WriteRaw(string text)
    {
      byte[] bytes = TextAdapter.TrivialEncoding.GetBytes(text);
      m_Stream.Write(bytes, 0, bytes.Length);

      return bytes.Length;
    }

    #endregion Public
  }
}