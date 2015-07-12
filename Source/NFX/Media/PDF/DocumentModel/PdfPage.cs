﻿using System;
using System.Collections.Generic;
using NFX.Media.PDF.Elements;
using NFX.Media.PDF.Styling;

namespace NFX.Media.PDF.DocumentModel
{
  /// <summary>
  /// PDF Page
  /// </summary>
  public class PdfPage : PdfObjectBase
  {
    internal PdfPage(PdfPageTree pageTree, float height = Constants.DEFAULT_PAGE_HEIGHT, float width = Constants.DEFAULT_PAGE_WIDTH)
    {
      Height = height;
      Width = width;

      m_PageTree = pageTree;
      m_Fonts = new List<PdfFont>();
      m_Elements = new List<PdfElementBase>();
    }

    private readonly List<PdfElementBase> m_Elements;

    private readonly PdfPageTree m_PageTree;

    private readonly List<PdfFont> m_Fonts;

    #region Properties

    /// <summary>
    /// Page's height
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    /// Page's width
    /// </summary>
    public float Width { get; set; }

    /// <summary>
    /// Page elements
    /// </summary>
    public List<PdfElementBase> Elements
    {
      get { return m_Elements; }
    }

    /// <summary>
    /// Page tree
    /// </summary>
    internal PdfPageTree PageTree
    {
      get { return m_PageTree; }
    }

    internal List<PdfFont> Fonts
    {
      get { return m_Fonts; }
    }

    #endregion Properties

    /// <summary>
    /// Adds PDF element to page's elements collection
    /// </summary>
    public void Add(PdfElementBase element)
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
    public TextElement AddText(string text, int fontSize, PdfFont font)
    {
      return this.AddText(text, fontSize, font, PdfColor.Black);
    }

    /// <summary>
    /// Add raw text to the page
    /// </summary>
    public TextElement AddText(string text, int fontSize, PdfFont font, PdfColor foreground)
    {
      var element = new TextElement(text, fontSize, font, foreground);
      Add(element);

      return element;
    }

    #endregion Add text

    #region Add paragraph

    /// <summary>
    /// Add text paragraph to the page
    /// </summary>
    public ParagraphElement AddParagraph(string text, float width)
    {
      return AddParagraph(text, width, Constants.DEFAULT_PARAGRAPH_LINE_HEIGHT, Constants.DEFAULT_FONT_SIZE, PdfFont.Courier, PdfColor.Black, PdfHorizontalAlign.Left);
    }

    /// <summary>
    /// Add text paragraph to the page
    /// </summary>
    public ParagraphElement AddParagraph(string text, float width, float lineHeight)
    {
      return AddParagraph(text, width, lineHeight, Constants.DEFAULT_FONT_SIZE, PdfFont.Courier, PdfColor.Black, PdfHorizontalAlign.Left);
    }

    /// <summary>
    /// Add text paragraph to the page
    /// </summary>
    /// <param name="text">Text</param>
    public ParagraphElement AddParagraph(string text, float width, float lineHeight, int fontSize)
    {
      return AddParagraph(text, width, lineHeight, fontSize, PdfFont.Courier, PdfColor.Black, PdfHorizontalAlign.Left);
    }

    /// <summary>
    /// Add text paragraph to the page
    /// </summary>
    public ParagraphElement AddParagraph(string text, float width, float lineHeight, int fontSize, PdfFont font)
    {
      return AddParagraph(text, width, lineHeight, fontSize, font, PdfColor.Black, PdfHorizontalAlign.Left);
    }

    /// <summary>
    /// Add text paragraph to the page
    /// </summary>
    public ParagraphElement AddParagraph(string text, float width, float lineHeight, int fontSize, PdfFont font, PdfColor color)
    {
      return AddParagraph(text, width, lineHeight, fontSize, font, color, PdfHorizontalAlign.Left);
    }

    /// <summary>
    /// Add text paragraph to the page
    /// </summary>
    public ParagraphElement AddParagraph(string text, float width, float lineHeight, int fontSize, PdfFont font, PdfColor foreground, PdfHorizontalAlign align)
    {
      var paragraph = new ParagraphElement(text, width, lineHeight, fontSize, font, foreground, align);
      Add(paragraph);

      return paragraph;
    }

    #endregion Add paragraph

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
  }
}