using System;
using System.Collections.Generic;
using NFX.Media.PDF.Elements;
using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF Page
  /// </summary>
  public class PdfPage : PdfObject
  {
    internal PdfPage(PdfPageTree parent, PdfSize size)
    {
      m_Size = size;
      m_Unit = m_Size.Unit;
      m_Parent = parent;
      m_Fonts = new List<PdfFont>();
      m_Elements = new List<PdfElement>();
    }

    private readonly PdfSize m_Size;

    private readonly PdfUnit m_Unit;

    private readonly List<PdfElement> m_Elements;

    private readonly PdfPageTree m_Parent;

    private readonly List<PdfFont> m_Fonts;

    #region Properties

    /// <summary>
    /// Page's height
    /// </summary>
    public float Height
    {
      get { return m_Size.Height; }
    }

    /// <summary>
    /// Page's width
    /// </summary>
    public float Width
    {
      get { return m_Size.Width; }
    }

    /// <summary>
    /// Page elements
    /// </summary>
    public List<PdfElement> Elements
    {
      get { return m_Elements; }
    }

    /// <summary>
    /// User space units
    /// (the default user space unit is 1/72 inch)
    /// </summary>
    public float UserUnit
    {
      get { return m_Unit.Points; }
    }

    /// <summary>
    /// Page tree
    /// </summary>
    internal PdfPageTree Parent
    {
      get { return m_Parent; }
    }

    /// <summary>
    /// Fonts used on the page
    /// </summary>
    internal List<PdfFont> Fonts
    {
      get { return m_Fonts; }
    }

    #endregion Properties

    /// <summary>
    /// Adds PDF element to page's elements collection
    /// </summary>
    public void Add(PdfElement element)
    {
      if (m_Elements.Contains(element))
        throw new InvalidOperationException("The element has already been added");

      m_Elements.Add(element);
    }

    #region Add text

    /// <summary>
    /// Add raw text to the page
    /// </summary>
    public TextElement AddText(string text)
    {
      return this.AddText(text, Constants.DEFAULT_FONT_SIZE, PdfFont.Courier);
    }

    /// <summary>
    /// Add raw text to the page
    /// </summary>
    public TextElement AddText(string text, float fontSize, PdfFont font)
    {
      return this.AddText(text, fontSize, font, PdfColor.Black);
    }

    /// <summary>
    /// Add raw text to the page
    /// </summary>
    public TextElement AddText(string text, float fontSize, PdfFont font, PdfColor foreground)
    {
      var element = new TextElement(text, fontSize, font, foreground);
      Add(element);

      return element;
    }

    #endregion Add text

    #region Add path

    /// <summary>
    /// Add path to the page
    /// </summary>
    public PathElement AddPath(float x, float y)
    {
      return AddPath(x, y, new PdfLineStyle());
    }

    /// <summary>
    /// Add path to the page
    /// </summary>
    public PathElement AddPath(float x, float y, float thickness)
    {
      return AddPath(x, y, new PdfLineStyle(thickness));
    }

    /// <summary>
    /// Add path to the page
    /// </summary>
    public PathElement AddPath(float x, float y, float thickness, PdfColor color)
    {
      return AddPath(x, y, new PdfLineStyle(thickness, color));
    }

    /// <summary>
    /// Add path to the page
    /// </summary>
    public PathElement AddPath(float x, float y, float thickness, PdfColor color, PdfLineType type)
    {
      return AddPath(x, y, new PdfLineStyle(thickness, color, type));
    }

    /// <summary>
    /// Add path to the page
    /// </summary>
    public PathElement AddPath(float x, float y, PdfLineStyle style)
    {
      var path = new PathElement(x, y, style);
      Add(path);

      return path;
    }

    #endregion Add path

    #region Add line

    /// <summary>
    /// Add line primitive to the page
    /// </summary>
    public LineElement AddLine(float x1, float y1, float x2, float y2)
    {
      return AddLine(x1, y1, x2, y2, new PdfLineStyle());
    }

    /// <summary>
    /// Add line primitive to the page
    /// </summary>
    public LineElement AddLine(float x1, float y1, float x2, float y2, float thickness)
    {
      return AddLine(x1, y1, x2, y2, new PdfLineStyle(thickness));
    }

    /// <summary>
    /// Add line primitive to the page
    /// </summary>
    public LineElement AddLine(float x1, float y1, float x2, float y2, float thickness, PdfColor color)
    {
      return AddLine(x1, y1, x2, y2, new PdfLineStyle(thickness, color));
    }

    /// <summary>
    /// Add line primitive to the page
    /// </summary>
    public LineElement AddLine(float x1, float y1, float x2, float y2, float thickness, PdfColor color, PdfLineType type)
    {
      return AddLine(x1, y1, x2, y2, new PdfLineStyle(thickness, color, type));
    }

    /// <summary>
    /// Add line primitive to the page
    /// </summary>
    public LineElement AddLine(float x1, float y1, float x2, float y2, PdfLineStyle style)
    {
      var line = new LineElement(x1, y1, x2, y2, style);
      Add(line);

      return line;
    }

    #endregion Add line

    #region Add rectangle

    /// <summary>
    /// Add rectangle primitive to the page
    /// </summary>
    public RectangleElement AddRectangle(float x1, float y1, float x2, float y2, PdfColor fill)
    {
      return AddRectangle(x1, y1, x2, y2, fill, new PdfLineStyle());
    }

    /// <summary>
    /// Add rectangle primitive to the page
    /// </summary>
    public RectangleElement AddRectangle(float x1, float y1, float x2, float y2, PdfColor fill, float borderThickness)
    {
      return AddRectangle(x1, y1, x2, y2, fill, new PdfLineStyle(borderThickness));
    }

    /// <summary>
    /// Add rectangle primitive to the page
    /// </summary>
    public RectangleElement AddRectangle(float x1, float y1, float x2, float y2, PdfColor fill, float borderThickness, PdfColor borderColor)
    {
      return AddRectangle(x1, y1, x2, y2, fill, new PdfLineStyle(borderThickness, borderColor));
    }

    /// <summary>
    /// Add rectangle primitive to the page
    /// </summary>
    public RectangleElement AddRectangle(float x1, float y1, float x2, float y2, PdfColor fill, float borderThickness, PdfColor borderColor, PdfLineType borderType)
    {
      return AddRectangle(x1, y1, x2, y2, fill, new PdfLineStyle(borderThickness, borderColor, borderType));
    }

    /// <summary>
    /// Add rectangle primitive to the page
    /// </summary>
    public RectangleElement AddRectangle(float x1, float y1, float x2, float y2, PdfColor fill, PdfLineStyle style)
    {
      var rectangle = new RectangleElement(x1, y1, x2, y2, fill, style);
      Add(rectangle);

      return rectangle;
    }

    #endregion Add rectangle

    #region Add circle

    /// <summary>
    /// Add circle primitive to the page
    /// </summary>
    public CircleElement AddCircle(float x, float y, float r, PdfColor fill)
    {
      return AddCircle(x, y, r, fill, new PdfLineStyle());
    }

    /// <summary>
    /// Add circle primitive to the page
    /// </summary>
    public CircleElement AddCircle(float x, float y, float r, PdfColor fill, float borderThickness)
    {
      return AddCircle(x, y, r, fill, new PdfLineStyle(borderThickness));
    }

    /// <summary>
    /// Add circle primitive to the page
    /// </summary>
    public CircleElement AddCircle(float x, float y, float r, PdfColor fill, float borderThickness, PdfColor borderColor)
    {
      return AddCircle(x, y, r, fill, new PdfLineStyle(borderThickness, borderColor));
    }

    /// <summary>
    /// Add circle primitive to the page
    /// </summary>
    public CircleElement AddCircle(float x, float y, float r, PdfColor fill, float borderThickness, PdfColor borderColor, PdfLineType borderType)
    {
      return AddCircle(x, y, r, fill, new PdfLineStyle(borderThickness, borderColor, borderType));
    }

    /// <summary>
    /// Add circle primitive to the page
    /// </summary>
    public CircleElement AddCircle(float x, float y, float r, PdfColor fill, PdfLineStyle borderStyle)
    {
      var circle = new CircleElement(x, y, r, fill, borderStyle);
      Add(circle);

      return circle;
    }

    #endregion Add circle

    #region Add image

    /// <summary>
    /// Add image to the page
    /// </summary>
    public ImageElement AddImage(string filePath)
    {
      var image = new ImageElement(filePath);
      Add(image);

      return image;
    }

    /// <summary>
    /// Add image to the page
    /// </summary>
    public ImageElement AddImage(string filePath, float width, float height)
    {
      var image = new ImageElement(filePath, width, height);
      Add(image);

      return image;
    }

    #endregion Add image
  }
}