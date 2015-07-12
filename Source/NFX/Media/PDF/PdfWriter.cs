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
      string meta = "%PDF-1.4".Args(Constants.RETURN);
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
      builder.AppendFormatLine("{0} 0 obj", font.ObjectId);
      builder.AppendLine("<<");
      builder.AppendLine("/Type /Font");
      builder.AppendLine("/Subtype /Type1");
      builder.AppendFormatLine("/Name /F{0}", font.Number);
      builder.AppendFormatLine("/BaseFont /{0}", font.Name);
      builder.AppendLine("/Encoding /WinAnsiEncoding");
      builder.AppendLine(">>");
      builder.AppendLine("endobj");

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
      builder.AppendFormatLine("{0} 0 obj", header.ObjectId);
      builder.AppendLine("<<");
      builder.AppendLine("/Type /Catalog");
      builder.AppendLine("/Version /1.4");
      builder.AppendFormatLine("/Pages {0} 0 R", header.PageTreeId);
      builder.AppendFormatLine("/Outlines {0} 0 R", header.OutlinesId);
      builder.AppendLine(">>");
      builder.AppendLine("endobj");

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
      builder.AppendFormatLine("{0} 0 obj", info.ObjectId);
      builder.AppendLine("<<");
      builder.AppendFormatLine("/Title ({0})", info.Title);
      builder.AppendFormatLine("/Author ({0})", info.Author);
      builder.AppendFormatLine("/Creator ({0})", info.Creator);
      builder.AppendFormatLine("/CreationDate ({0:yyyyMMdd})", DateTime.Now);
      builder.AppendLine(">>");
      builder.AppendLine("endobj");

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
      builder.AppendFormatLine("{0} 0 obj", outlines.ObjectId);
      builder.AppendLine("<<");
      builder.AppendLine("/Type /Outlines");
      builder.AppendLine("/Count 0");
      builder.AppendLine(">>");
      builder.AppendLine("endobj");

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
      builder.AppendFormatLine("{0} 0 obj", pageTree.ObjectId);
      builder.AppendLine("<<");
      builder.AppendLine("/Type /Pages");
      builder.AppendFormatLine("/Count {0}", pageTree.Pages.Count);
      builder.AppendFormatLine("/Kids [{0}]", pageListBuilder);
      builder.AppendLine(">>");
      builder.AppendLine("endobj");

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

      var w = TextAdapter.FormatFloat(page.Width);
      var h = TextAdapter.FormatFloat(page.Height);

      var pageBuilder = new StringBuilder();
      pageBuilder.AppendFormatLine("{0} 0 obj", page.ObjectId);
      pageBuilder.AppendLine("<<");
      pageBuilder.AppendLine("/Type /Page");
      pageBuilder.AppendFormatLine("/Parent {0} 0 R", page.PageTree.ObjectId);
      pageBuilder.AppendFormatLine("/Resources <</Font <<{0}>>", resultString);
      if (imageBuilder.Length > 0)
      {
        pageBuilder.AppendFormatLine("/XObject <<{0}>>", imageBuilder);
      }
      pageBuilder.AppendLine(">>");
      pageBuilder.AppendFormatLine("/MediaBox [0 0 {0} {1}]", w, h);
      pageBuilder.AppendFormatLine("/CropBox [0 0 {0} {1}]", w, h);
      pageBuilder.AppendLine("/Rotate 0");
      pageBuilder.AppendLine("/ProcSet [/PDF /Text /ImageC]");
      if (elementBuilder.Length > 0)
      {
        pageBuilder.AppendFormatLine("/Contents [{0}]", elementBuilder);
      }
      pageBuilder.AppendLine(">>");
      pageBuilder.AppendLine("endobj");

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
      builder.AppendLine("xref");
      builder.AppendFormatLine("0 {0}", trailer.LastObjectId + 1);
      builder.AppendLine("0000000000 65535 f");
      foreach (var offset in trailer.ObjectOffsets)
      {
        builder.AppendFormatLine("{0} 00000 n", offset);
      }
      builder.AppendLine("trailer");
      builder.AppendLine("<<");
      builder.AppendFormatLine("/Size {0}", trailer.LastObjectId + 1);
      builder.AppendLine("/Root 1 0 R");
      builder.AppendLine("/Info 2 0 R");
      builder.AppendLine(">>");
      builder.AppendLine("startxref");
      builder.AppendFormatLine("{0}", trailer.XRefOffset);
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
    /// Writes PDF line element into file stream
    /// </summary>
    /// <param name="line">PDF line element</param>
    /// <returns>Written bytes count</returns>
    public long Write(LineElement line)
    {
      var borderStyle = GetLineStylePdf(line.Style);

      var x = TextAdapter.FormatFloat(line.X);
      var y = TextAdapter.FormatFloat(line.Y);
      var x1 = TextAdapter.FormatFloat(line.X1);
      var y1 = TextAdapter.FormatFloat(line.Y1);

      var lineContent = new StringBuilder();
      lineContent.AppendFormatLine("{0} RG", line.Style.Color);
      lineContent.AppendLine("q");
      lineContent.AppendLine(borderStyle);
      lineContent.AppendFormatLine("{0} {1} m {2} {3} l", x, y, x1, y1);
      lineContent.AppendLine("S");
      lineContent.AppendLine("Q");

      var resultLine = new StringBuilder();
      resultLine.AppendFormatLine("{0} 0 obj", line.ObjectId);
      resultLine.AppendLine("<<");
      resultLine.AppendFormatLine("/Length {0}", lineContent.Length);
      resultLine.AppendLine(">>");
      resultLine.AppendLine("stream");
      resultLine.AppendLine(lineContent.ToString());
      resultLine.AppendLine("endstream");
      resultLine.AppendLine("endobj");

      return WriteRaw(resultLine.ToString());
    }

    /// <summary>
    /// Writes PDF rectangle element into file stream
    /// </summary>
    /// <param name="rectangle">PDF rectangle element</param>
    /// <returns>Written bytes count</returns>
    public long Write(RectangleElement rectangle)
    {
      var borderStyle = GetLineStylePdf(rectangle.BorderStyle);

      var x = TextAdapter.FormatFloat(rectangle.X);
      var y = TextAdapter.FormatFloat(rectangle.Y);
      var w = TextAdapter.FormatFloat(rectangle.X1 - rectangle.X);
      var h = TextAdapter.FormatFloat(rectangle.Y1 - rectangle.Y);

      var rectangleContent = new StringBuilder();
      rectangleContent.AppendLine("q");
      rectangleContent.AppendFormatLine("{0} RG", rectangle.BorderStyle.Color);
      rectangleContent.AppendFormatLine("{0} rg", rectangle.Fill);
      rectangleContent.AppendLine(borderStyle);
      rectangleContent.AppendFormatLine("{0} {1} {2} {3} re", x, y, w, h);
      rectangleContent.AppendLine("B");
      rectangleContent.AppendLine("Q");

      var resultRectangle = new StringBuilder();
      resultRectangle.AppendFormatLine("{0} 0 obj", rectangle.ObjectId);
      resultRectangle.AppendLine("<<");
      resultRectangle.AppendFormatLine("/Length {0}", rectangleContent.Length);
      resultRectangle.AppendLine(">>");
      resultRectangle.AppendLine("stream");
      resultRectangle.AppendLine(rectangleContent.ToString());
      resultRectangle.AppendLine("endstream");
      resultRectangle.AppendLine("endobj");

      return WriteRaw(resultRectangle.ToString());
    }

    /// <summary>
    /// Writes PDF circle element into file stream
    /// </summary>
    /// <param name="circle">PDF circle element</param>
    /// <returns>Written bytes count</returns>
    public long Write(CircleElement circle)
    {
      var borderStyle = GetLineStylePdf(circle.BorderStyle);

      var xLeft = TextAdapter.FormatFloat(circle.X);
      var xRight = TextAdapter.FormatFloat(circle.X + 2 * circle.R);
      var centerY = TextAdapter.FormatFloat(circle.CenterY);
      var positiveBezier = TextAdapter.FormatFloat(circle.CenterY + circle.R * Constants.SQRT_TWO);
      var negativeBezier = TextAdapter.FormatFloat(circle.CenterY - circle.R * Constants.SQRT_TWO);

      var circleContent = new StringBuilder();
      circleContent.AppendLine("q");
      circleContent.AppendFormatLine("{0} RG", circle.BorderStyle.Color);
      circleContent.AppendFormatLine("{0} rg", circle.Fill);
      circleContent.AppendLine(borderStyle);
      circleContent.AppendFormatLine("{0} {1} m", xLeft, centerY);
      circleContent.AppendFormatLine("{0} {1} {2} {1} {2} {3} c", xLeft, positiveBezier, xRight, centerY);
      circleContent.AppendFormatLine("{0} {1} m", xLeft, centerY);
      circleContent.AppendFormatLine("{0} {1} {2} {1} {2} {3} c", xLeft, negativeBezier, xRight, centerY);
      circleContent.AppendLine("B");
      circleContent.AppendLine("Q");

      var resultCircle = new StringBuilder();
      resultCircle.AppendFormatLine("{0} 0 obj", circle.ObjectId);
      resultCircle.AppendLine("<<");
      resultCircle.AppendFormatLine("/Length {0}", circleContent.Length);
      resultCircle.AppendLine(">>");
      resultCircle.AppendLine("stream");
      resultCircle.AppendLine(circleContent.ToString());
      resultCircle.AppendLine("endstream");
      resultCircle.AppendLine("endobj");

      return WriteRaw(resultCircle.ToString());
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
      pdfStreamBuilder.AppendLine("q");
      pdfStreamBuilder.AppendLine("BT");
      pdfStreamBuilder.AppendFormatLine("/F{0} {1} Tf", text.Font.Number, text.FontSize);
      pdfStreamBuilder.AppendFormatLine("{0} rg", text.Color);
      pdfStreamBuilder.AppendFormatLine("{0} {1} Td {2} Tj", text.X, text.Y, unicodeContent);
      pdfStreamBuilder.AppendLine("ET");
      pdfStreamBuilder.AppendLine("Q");

      var builder = new StringBuilder();
      builder.AppendFormatLine("{0} 0 obj", text.ObjectId);
      builder.AppendLine("<<");
      builder.AppendFormatLine("/Length {0}", pdfStreamBuilder.Length);
      builder.AppendLine(">>");
      builder.AppendLine("stream");
      builder.AppendFormatLine("{0}", pdfStreamBuilder);
      builder.AppendLine("endstream");
      builder.AppendLine("endobj");

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
      pdfStreamBuilder.AppendLine("q");
      pdfStreamBuilder.AppendLine("BT");
      pdfStreamBuilder.AppendFormatLine("/F{0} {1} Tf", paragraph.Font.Number, paragraph.FontSize);
      pdfStreamBuilder.AppendFormatLine("{0} rg", paragraph.Color);
      pdfStreamBuilder.AppendFormatLine("{0} {1} Td ", paragraph.X, paragraph.Y);
      pdfStreamBuilder.AppendLine("14 TL");
      foreach (var line in paragraph.Lines)
      {
        var checkedLine = TextAdapter.CheckText(line.Content);
        var bytes = TextAdapter.UnicodeEncoding.GetBytes(checkedLine);
        bytes = TextAdapter.FormatHexStringLiteral(bytes);
        var unicodeContent = TextAdapter.TrivialEncoding.GetString(bytes, 0, bytes.Length);

        pdfStreamBuilder.AppendFormatLine("{0} -{1} Td {2} Tj", TextAdapter.FormatFloat(line.LeftMargin),
          TextAdapter.FormatFloat(line.TopMargin), unicodeContent);
        pdfStreamBuilder.AppendFormatLine("-{0} 0 Td", TextAdapter.FormatFloat(line.LeftMargin));
      }
      pdfStreamBuilder.AppendLine("ET");
      pdfStreamBuilder.AppendLine("Q");

      var builder = new StringBuilder();
      builder.AppendFormatLine("{0} 0 obj", paragraph.ObjectId);
      builder.AppendLine("<<");
      builder.AppendFormatLine("/Length {0}", pdfStreamBuilder.Length);
      builder.AppendLine(">>");
      builder.AppendLine("stream");
      builder.AppendFormatLine("{0}", pdfStreamBuilder);
      builder.AppendLine("endstream");
      builder.AppendLine("endobj");

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

    private string GetLineStylePdf(PdfLineStyle style)
    {
      var styleBuilder = new StringBuilder();
      styleBuilder.AppendFormatLine("{0} w", TextAdapter.FormatFloat(style.Thickness));
      switch (style.Type)
      {
        case PdfLineType.OutlinedThin:
          styleBuilder.AppendLine("[2 2] 0 d");
          break;
        case PdfLineType.Outlined:
          styleBuilder.AppendLine("[4 4] 0 d");
          break;
        case PdfLineType.OutlinedBold:
          styleBuilder.AppendLine("[6 6] 0 d");
          break;
      }

      return styleBuilder.ToString();
    }
  }
}