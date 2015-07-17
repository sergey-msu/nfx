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

      // document meta data
      position += Write(document.Meta);

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
          // all
          document.Trailer.AddObjectOffset(position);
          position += element.Write(this);

          // images
          var image = element as ImageElement;
          if (image != null)
          {
            document.Trailer.AddObjectOffset(position);
            position += WriteX(image);
          }
        }
      }

      // trailer 
      document.Trailer.XRefOffset = position;
      Write(document.Trailer);
    }

    public long Write(PdfMeta meta)
    {
      var metaBuilder = new StringBuilder();
      metaBuilder.AppendFormatLine("%PDF-{0}", meta.Version);

      return WriteRaw(metaBuilder.ToString());
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
          imageBuilder.AppendFormat("/I{0} {0} 0 R ", image.XObjectId);
        }
      }

      var fontsBuilder = new StringBuilder();
      foreach (var font in page.Fonts)
      {
        fontsBuilder.AppendFormat("/F{0} {1} 0 R ", font.Number, font.ObjectId);
      }

      var w = TextAdapter.FormatFloat(page.Width);
      var h = TextAdapter.FormatFloat(page.Height);

      var pageBuilder = new StringBuilder();
      pageBuilder.AppendFormatLine("{0} 0 obj", page.ObjectId);
      pageBuilder.AppendLine("<<");
      pageBuilder.AppendLine("/Type /Page");
      pageBuilder.AppendFormatLine("/UserUnit {0}", TextAdapter.FormatFloat(page.UserUnit));
      pageBuilder.AppendFormatLine("/Parent {0} 0 R", page.Parent.ObjectId);
      pageBuilder.AppendFormatLine("/Resources <</Font <<{0}>>", fontsBuilder);
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
      var imageContent = new StringBuilder();
      imageContent.AppendLine("q");
      imageContent.AppendFormatLine("{0} 0 0 {1} {2} {3} cm", image.Width, image.Height, image.X, image.Y);
      imageContent.AppendFormatLine("/I{0} Do", image.XObjectId);
      imageContent.AppendLine("Q");

      return writePdfObject(image.ObjectId, imageContent.ToString());
    }

    public long WriteX(ImageElement image)
    {
      long bytesWritten = 0;

      var xObject = new StringBuilder();
      xObject.AppendFormatLine("{0} 0 obj", image.XObjectId);
      xObject.AppendLine("<<");
      xObject.AppendLine("/Type /XObject");
      xObject.AppendLine("/Subtype /Image");
      xObject.AppendFormatLine("/Name /I{0}", image.XObjectId);
      xObject.AppendLine("/Filter /DCTDecode");
      xObject.AppendFormatLine("/Width {0}", image.OwnWidth);
      xObject.AppendFormatLine("/Height {0}", image.OwnHeight);
      xObject.AppendLine("/BitsPerComponent 8");
      xObject.AppendLine("/ColorSpace /DeviceRGB");
      xObject.AppendFormatLine("/Length {0}", image.Content.Length);
      xObject.AppendLine(">>");

      var resultImage = new StringBuilder();
      resultImage.Append(xObject);
      resultImage.AppendLine("stream");

      bytesWritten += WriteRaw(resultImage.ToString());
      bytesWritten += WriteRaw(image.Content);

      var footer = new StringBuilder();
      footer.AppendLine();
      footer.AppendLine("endstream");
      footer.AppendLine("endobj");

      bytesWritten += WriteRaw(footer.ToString());

      return bytesWritten;
    }

    /// <summary>
    /// Writes PDF line element into file stream
    /// </summary>
    /// <param name="line">PDF line element</param>
    /// <returns>Written bytes count</returns>
    public long Write(LineElement line)
    {
      var borderStyle = getLineStylePdf(line.Style);

      var x = TextAdapter.FormatFloat(line.X);
      var y = TextAdapter.FormatFloat(line.Y);
      var x1 = TextAdapter.FormatFloat(line.X1);
      var y1 = TextAdapter.FormatFloat(line.Y1);

      var lineContent = new StringBuilder();
      lineContent.AppendFormatLine("{0} RG", line.Style.Color);
      lineContent.AppendLine("q");
      lineContent.Append(borderStyle);
      lineContent.AppendFormatLine("{0} {1} m {2} {3} l", x, y, x1, y1);
      lineContent.AppendLine("S");
      lineContent.AppendLine("Q");

      return writePdfObject(line.ObjectId, lineContent.ToString());
    }

    /// <summary>
    /// Writes PDF rectangle element into file stream
    /// </summary>
    /// <param name="rectangle">PDF rectangle element</param>
    /// <returns>Written bytes count</returns>
    public long Write(RectangleElement rectangle)
    {
      var borderStyle = getLineStylePdf(rectangle.BorderStyle);

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

      return writePdfObject(rectangle.ObjectId, rectangleContent.ToString());
    }

    /// <summary>
    /// Writes PDF circle element into file stream
    /// </summary>
    /// <param name="circle">PDF circle element</param>
    /// <returns>Written bytes count</returns>
    public long Write(CircleElement circle)
    {
      var borderStyle = getLineStylePdf(circle.BorderStyle);

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

      return writePdfObject(circle.ObjectId, circleContent.ToString());
    }

    /// <summary>
    /// Writes PDF text element into file stream
    /// </summary>
    /// <param name="text">PDF text element</param>
    /// <returns>Written bytes count</returns>
    public long Write(TextElement text)
    {
      var bytes = TextAdapter.UnicodeEncoding.GetBytes(text.Content);
      bytes = TextAdapter.FormatHexStringLiteral(bytes);
      var unicodeContent = TextAdapter.TrivialEncoding.GetString(bytes, 0, bytes.Length);

      var pdfStreamBuilder = new StringBuilder();
      pdfStreamBuilder.AppendLine("q");
      pdfStreamBuilder.AppendLine("BT");
      pdfStreamBuilder.AppendFormatLine("/F{0} {1} Tf", text.Font.Number, TextAdapter.FormatFloat(text.FontSize));
      pdfStreamBuilder.AppendFormatLine("{0} rg", text.Color);
      pdfStreamBuilder.AppendFormatLine("{0} {1} Td", TextAdapter.FormatFloat(text.X), TextAdapter.FormatFloat(text.Y));
      pdfStreamBuilder.AppendFormatLine("{0} Tj", unicodeContent);
      pdfStreamBuilder.AppendLine("ET");
      pdfStreamBuilder.AppendLine("Q");

      return writePdfObject(text.ObjectId, pdfStreamBuilder.ToString());
    }

    #endregion Public

    #region .pvt        

    /// <summary>
    /// Writes raw string into file stream
    /// </summary>
    /// <param name="text">Raw PDF string</param>
    /// <returns>Written bytes count</returns>
    private long WriteRaw(string text)
    {
      byte[] bytes = TextAdapter.TrivialEncoding.GetBytes(text);
      m_Stream.Write(bytes, 0, bytes.Length);

      return bytes.Length;
    }

    /// <summary>
    /// Writes raw data bytes into file stream
    /// </summary>
    /// <param name="bytes">Raw data bytes</param>
    /// <returns>Written bytes count</returns>
    private long WriteRaw(byte[] bytes)
    {
      m_Stream.Write(bytes, 0, bytes.Length);

      return bytes.Length;
    }

    private string getLineStylePdf(PdfLineStyle style)
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

    private long writePdfObject(int objectId, string content)
    {
      var resultLine = new StringBuilder();
      resultLine.AppendFormatLine("{0} 0 obj", objectId);
      resultLine.AppendFormatLine("<< /Length {0} >>", content.Length);
      resultLine.AppendLine("stream");
      resultLine.Append(content);
      resultLine.AppendLine("endstream");
      resultLine.AppendLine("endobj");

      return WriteRaw(resultLine.ToString());
    }

    #endregion .pvt
  }
}