using System;
using System.IO;
using System.Linq;
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
  public sealed class PdfWriter : IDisposable
  {
    public PdfWriter(Stream stream)
    {
      m_Stream = stream;
      m_Writer = new BinaryWriter(stream, Encoding.ASCII, true);

#if DEBUG
      PrettyFormatting = true;
#endif
    }

    private readonly Stream m_Stream;

    private readonly BinaryWriter m_Writer;

    /// <summary>
    /// Use flate decode filter for text
    /// </summary>
    //public bool FlateDecode { get; set; }

    /// <summary>
    /// Insert nonnecessary tabs and returns for a pretty output look file
    /// </summary>
    public bool PrettyFormatting { get; set; }

    #region Public

    /// <summary>
    /// Writes PDF document into file stream
    /// </summary>
    /// <param name="document">PDF document</param>
    public void Write(PdfDocument document)
    {
      // document meta data
      Write(document.Meta);

      // header object
      document.Trailer.AddObjectOffset(m_Stream.Position);
      Write(document.Header);

      // info object
      document.Trailer.AddObjectOffset(m_Stream.Position);
      Write(document.Info);

      // outlines object
      document.Trailer.AddObjectOffset(m_Stream.Position);
      Write(document.Outlines);

      // fonts
      foreach (var font in document.Fonts)
      {
        document.Trailer.AddObjectOffset(m_Stream.Position);
        Write(font);
      }

      // page tree
      document.Trailer.AddObjectOffset(m_Stream.Position);
      Write(document.PageTree);

      // pages
      foreach (var page in document.Pages)
      {
        document.Trailer.AddObjectOffset(m_Stream.Position);
        Write(page);

        // elements
        foreach (var element in page.Elements)
        {
          // all
          document.Trailer.AddObjectOffset(m_Stream.Position);
          element.Write(this);

          // images
          var image = element as ImageElement;
          if (image != null)
          {
            document.Trailer.AddObjectOffset(m_Stream.Position);
            WriteXObject(image);
          }
        }
      }

      // trailer 
      document.Trailer.XRefOffset = m_Stream.Position;
      Write(document.Trailer);
    }

    public void Write(PdfMeta meta)
    {
      writeLineRaw("%PDF-{0}", meta.Version);
      writeLineRaw("%\u00b5\u00b5\u00b5\u00b5");
    }

    /// <summary>
    /// Writes PDF font into file stream 
    /// </summary>
    /// <param name="font">PDF font</param>
    public void Write(PdfFont font)
    {
      writeBeginObject(font.ObjectId);
      writeBeginDictionary();
      writeDictionaryEntry("/Type", "/Font");
      writeDictionaryEntry("/Subtype", "/Type1");
      writeDictionaryEntry("/Name", string.Format("/F{0}", font.Number));
      writeDictionaryEntry("/BaseFont", string.Format("/{0}", font.Name));
      writeDictionaryEntry("/Encoding", "/WinAnsiEncoding");
      writeEndDictionary();
      writeEndObject();
    }

    /// <summary>
    /// Writes PDF header into file stream 
    /// </summary>
    /// <param name="root">PDF document root</param>
    internal void Write(PdfRoot root)
    {
      writeBeginObject(root.ObjectId);
      writeBeginDictionary();
      writeDictionaryEntry("/Type", "/Catalog");
      writeDictionaryEntry("/Pages", string.Format("{0} 0 R", root.PageTreeId));
      writeDictionaryEntry("/Outlines", string.Format("{0} 0 R", root.OutlinesId));
      writeEndDictionary();
      writeEndObject();
    }

    /// <summary>
    /// Writes PDF info into file stream
    /// </summary>
    /// <param name="info">PDF document info</param>
    public void Write(PdfInfo info)
    {
      writeBeginObject(info.ObjectId);
      writeBeginDictionary();
      if (!string.IsNullOrWhiteSpace(info.Title))
        writeDictionaryEntry("/Title", info.Title);
      if (!string.IsNullOrWhiteSpace(info.Subject))
        writeDictionaryEntry("/Subject", info.Subject);
      if (!string.IsNullOrWhiteSpace(info.Keywords))
        writeDictionaryEntry("/Keywords", info.Keywords);
      if (!string.IsNullOrWhiteSpace(info.Author))
        writeDictionaryEntry("/Author", info.Author);
      if (!string.IsNullOrWhiteSpace(info.Creator))
        writeDictionaryEntry("/Creator", info.Creator);
      if (!string.IsNullOrWhiteSpace(info.Producer))
        writeDictionaryEntry("/Producer", info.Producer);
      writeDictionaryEntry("/CreationDate", string.Format("(D:{0:yyyyMMddhhmmss})", info.CreationDate == DateTime.MinValue ? DateTime.UtcNow : info.CreationDate));
      writeDictionaryEntry("/ModDate", string.Format("(D:{0:yyyyMMddhhmmss})", info.ModificationDate == DateTime.MinValue ? DateTime.UtcNow : info.ModificationDate));
      writeEndDictionary();
      writeEndObject();
    }

    /// <summary>
    /// Writes PDF document outlines into file stream
    /// </summary>
    /// <param name="outlines">PDF document outlines</param>
    public void Write(PdfOutlines outlines)
    {
      writeBeginObject(outlines.ObjectId);
      writeBeginDictionary();
      writeDictionaryEntry("/Type", "/Outlines");
      writeDictionaryEntry("/Count", "0");
      writeEndDictionary();
      writeEndObject();
    }

    /// <summary>
    /// Writes PDF document page tree into file stream
    /// </summary>
    /// <param name="pageTree">PDF document page tree</param>
    internal void Write(PdfPageTree pageTree)
    {
      if (pageTree.Pages.Count == 0)
        throw new InvalidOperationException("PDF document has no pages");

      var pages = string.Join(Constants.SPACE.ToString(), pageTree.Pages.Select(p => p.GetReference()));

      writeBeginObject(pageTree.ObjectId);
      writeBeginDictionary();
      writeDictionaryEntry("/Type", "/Pages");
      writeDictionaryEntry("/Count", string.Format("{0}", pageTree.Pages.Count));
      writeDictionaryEntry("/Kids", string.Format("[{0}]", pages));
      writeEndDictionary();
      writeEndObject();
    }

    /// <summary>
    /// Writes PDF page into file stream
    /// </summary>
    /// <param name="page">PDF page</param>
    public void Write(PdfPage page)
    {
      var resourcesBuilder = new StringBuilder();
      var elements = string.Join(Constants.SPACE.ToString(), page.Elements.Select(p => p.GetReference()));
      var images = string.Join(Constants.SPACE.ToString(), page.Elements.OfType<ImageElement>().Select(p => string.Format("/I{0} {0} 0 R", p.XObjectId)));
      var fonts = string.Join(Constants.SPACE.ToString(), page.Fonts.Select(p => string.Format("/F{0} {1} 0 R", p.Number, p.ObjectId)));

      if (fonts.Length > 0)
        resourcesBuilder.AppendFormat(" /Font <<{0}>> ", fonts);
      if (images.Length > 0)
        resourcesBuilder.AppendFormat(" /XObject <<{0}>> ", images);

      var w = TextAdapter.FormatFloat(page.Width);
      var h = TextAdapter.FormatFloat(page.Height);

      writeBeginObject(page.ObjectId);
      writeBeginDictionary();
      writeDictionaryEntry("/Type", "/Page");
      writeDictionaryEntry("/UserUnit", string.Format("{0}", TextAdapter.FormatFloat(page.UserUnit)));
      writeDictionaryEntry("/Parent", string.Format("{0} 0 R", page.Parent.ObjectId));
      writeDictionaryEntry("/Resources", string.Format("<<{0}>>", resourcesBuilder));
      writeDictionaryEntry("/MediaBox", string.Format("[0 0 {0} {1}]", w, h));
      writeDictionaryEntry("/CropBox", string.Format("[0 0 {0} {1}]", w, h));
      writeDictionaryEntry("/Rotate", string.Format("0"));
      writeDictionaryEntry("/ProcSet", string.Format("[ /PDF /Text /ImageC ]"));
      if (elements.Length > 0)
      {
        writeDictionaryEntry("/Contents", string.Format("[{0}]", elements));
      }
      writeEndDictionary();
      writeEndObject();
    }

    /// <summary>
    /// Writes PDF document trailer into file stream
    /// </summary>
    /// <param name="trailer">PDF document trailer</param>
    public void Write(PdfTrailer trailer)
    {
      writeLineRaw("xref");
      writeLineRaw("0 {0}", trailer.LastObjectId + 1);
      writeLineRaw("0000000000 65535 f");
      foreach (var offset in trailer.ObjectOffsets)
      {
        writeLineRaw("{0} 00000 n", offset);
      }
      writeLineRaw("trailer");
      writeBeginDictionary();
      writeDictionaryEntry("/Size", trailer.LastObjectId + 1);
      writeDictionaryEntry("/Root", "1 0 R");
      writeDictionaryEntry("/Info", "2 0 R");
      writeEndDictionary();
      writeLineRaw("startxref");
      writeLineRaw("{0}", trailer.XRefOffset);
      writeRaw(Encoding.ASCII.GetBytes("%%EOF"));
    }

    /// <summary>
    /// Writes PDF image element into file stream
    /// </summary>
    /// <param name="image">PDF image element</param>
    public void Write(ImageElement image)
    {
      var imageContent = new StringBuilder();
      imageContent.AppendLine("q");
      imageContent.AppendFormatLine("{0} 0 0 {1} {2} {3} cm", image.Width, image.Height, image.X, image.Y);
      imageContent.AppendFormatLine("/I{0} Do", image.XObjectId);
      imageContent.Append("Q");

      writeStreamedObject(image.ObjectId, imageContent.ToString());
    }

    /// <summary>
    /// Writes PDF image xObject element into file stream
    /// </summary>
    /// <param name="image">PDF image xObject element</param>
    public void WriteXObject(ImageElement image)
    {
      writeBeginObject(image.XObjectId);
      
      writeBeginDictionary();
      writeDictionaryEntry("/Type", "/XObject");
      writeDictionaryEntry("/Subtype", "/Image");
      writeDictionaryEntry("/Name", string.Format("/I{0}", image.XObjectId));
      writeDictionaryEntry("/Filter", "/DCTDecode");
      writeDictionaryEntry("/Width", image.OwnWidth);
      writeDictionaryEntry("/Height", image.OwnHeight);
      writeDictionaryEntry("/BitsPerComponent", 8);
      writeDictionaryEntry("/ColorSpace", "/DeviceRGB");
      writeDictionaryEntry("/Length", image.Content.Length);
      writeEndDictionary();

      writeBeginStream();
      writeLineRaw(image.Content);
      writeEndStream();

      writeEndObject();
    }

    /// <summary>
    /// Writes PDF line element into file stream
    /// </summary>
    /// <param name="line">PDF line element</param>
    public void Write(LineElement line)
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
      lineContent.Append("Q");

      writeStreamedObject(line.ObjectId, lineContent.ToString());
    }

    /// <summary>
    /// Writes PDF rectangle element into file stream
    /// </summary>
    /// <param name="rectangle">PDF rectangle element</param>
    public void Write(RectangleElement rectangle)
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
      rectangleContent.Append(borderStyle);
      rectangleContent.AppendFormatLine("{0} {1} {2} {3} re", x, y, w, h);
      rectangleContent.AppendLine("B");
      rectangleContent.Append("Q");

      writeStreamedObject(rectangle.ObjectId, rectangleContent.ToString());
    }

    /// <summary>
    /// Writes PDF circle element into file stream
    /// </summary>
    /// <param name="circle">PDF circle element</param>
    public void Write(CircleElement circle)
    {
      var borderStyle = getLineStylePdf(circle.BorderStyle);

      var xLeft = TextAdapter.FormatFloat(circle.X);
      var xRight = TextAdapter.FormatFloat(circle.X + 2*circle.R);
      var centerY = TextAdapter.FormatFloat(circle.CenterY);
      var positiveBezier = TextAdapter.FormatFloat(circle.CenterY + circle.R*Constants.SQRT_TWO);
      var negativeBezier = TextAdapter.FormatFloat(circle.CenterY - circle.R*Constants.SQRT_TWO);

      var circleContent = new StringBuilder();
      circleContent.AppendLine("q");
      circleContent.AppendFormatLine("{0} RG", circle.BorderStyle.Color);
      circleContent.AppendFormatLine("{0} rg", circle.Fill);
      circleContent.Append(borderStyle);
      circleContent.AppendFormatLine("{0} {1} m", xLeft, centerY);
      circleContent.AppendFormatLine("{0} {1} {2} {1} {2} {3} c", xLeft, positiveBezier, xRight, centerY);
      circleContent.AppendFormatLine("{0} {1} m", xLeft, centerY);
      circleContent.AppendFormatLine("{0} {1} {2} {1} {2} {3} c", xLeft, negativeBezier, xRight, centerY);
      circleContent.AppendLine("B");
      circleContent.Append("Q");

      writeStreamedObject(circle.ObjectId, circleContent.ToString());
    }

    /// <summary>
    /// Writes PDF text element into file stream
    /// </summary>
    /// <param name="text">PDF text element</param>
    public void Write(TextElement text)
    {
      var escapedText = TextAdapter.FixEscapes(text.Content);

      var pdfStreamBuilder = new StringBuilder();
      pdfStreamBuilder.AppendLine("q");
      pdfStreamBuilder.AppendLine("BT");
      pdfStreamBuilder.AppendFormatLine("/F{0} {1} Tf", text.Font.Number, TextAdapter.FormatFloat(text.FontSize));
      pdfStreamBuilder.AppendFormatLine("{0} rg", text.Color);
      pdfStreamBuilder.AppendFormatLine("{0} {1} Td", TextAdapter.FormatFloat(text.X), TextAdapter.FormatFloat(text.Y));
      pdfStreamBuilder.AppendFormatLine("({0}) Tj", escapedText);
      pdfStreamBuilder.AppendLine("ET");
      pdfStreamBuilder.Append("Q");

      writeStreamedObject(text.ObjectId, pdfStreamBuilder.ToString());
    }

    #endregion Public

    #region .pvt

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

    private void writeStreamedObject(int objectId, string stream)
    {
      writeBeginObject(objectId);
      
      writeBeginDictionary();
      writeDictionaryEntry("/Length", stream.Length);
      //if (FlateDecode)
      //  writeDictionaryEntry("/Filter", "/FlateDecode");
      writeEndDictionary();

      writeBeginStream();
      writeLineRaw(stream);
      writeEndStream();

      writeEndObject();
    }

    private void writeRaw(byte[] bytes)
    {
      m_Writer.Write(bytes);
    }

    private void writeLineRaw(byte[] bytes)
    {
      writeRaw(bytes);
      writeRaw(Constants.RETURN);
    }

    private void writeRaw(string str, params object[] parameters)
    {
      str = string.Format(str, parameters);
      var bytes = Encoding.ASCII.GetBytes(str);
      writeRaw(bytes);
    }

    private void writeLineRaw(string str, params object[] parameters)
    {
      writeRaw(str, parameters);
      writeRaw(Constants.RETURN);
    }

    private void writeBeginObject(int objectId)
    {
      var str = string.Format("{0} 0 obj", objectId);
      writeLineRaw(str);
    }

    private void writeEndObject()
    {
      writeLineRaw("endobj");
    }

    private void writeBeginStream()
    {
      writeLineRaw("stream");
    }

    private void writeEndStream()
    {
      writeLineRaw("endstream");
    }

    private void writeBeginDictionary()
    {
      if (PrettyFormatting)
        writeLineRaw("<<");
      else
        writeRaw("<< ");
    }

    private void writeEndDictionary()
    {
      writeLineRaw(">>");
    }

    private void writeDictionaryEntry(string key, object value)
    {
      if (PrettyFormatting)
        writeLineRaw("{0}{0}{1} {2}", Constants.SPACE, key, value);
      else
        writeRaw("{0} {1} ", key, value);
    }

    #endregion .pvt

    public void Dispose()
    {
      m_Writer.Flush();
      m_Writer.Close();
    }
  }
}