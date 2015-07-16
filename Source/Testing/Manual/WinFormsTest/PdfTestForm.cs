﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NFX.Media.PDF.DocumentModel;
using NFX.Media.PDF.Styling;

namespace WinFormsTest
{
  public partial class PdfTestForm : Form
  {
    public PdfTestForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var document = new PdfDocument("my title..", "me..");
      document.Fonts.Add(PdfFont.Courier);
      document.Fonts.Add(PdfFont.CourierBold);
      document.Fonts.Add(PdfFont.Times);
      document.Fonts.Add(PdfFont.TimesBoldItalic);

      var page = document.AddPage();

      // simple text
      var text = page.AddText("abc", 20, PdfFont.CourierBold, PdfColor.DarkGreen);
      text.X = 10;
      text.Y = 730;

      // lines
      page.AddLine(20, 620, 50, 620);
      page.AddLine(50, 620, 70, 600, 2.5F);
      page.AddLine(70, 600, 80, 550, 0.5F, PdfColor.Red);
      page.AddLine(80, 550, 80, 530, 1.0F, PdfColor.Green, PdfLineType.Outlined);
      page.AddLine(80, 530, 80, 500, 3.2F, PdfColor.DarkBlue, PdfLineType.OutlinedBold);
      page.AddLine(80, 500, 120, 500, 1.0F, PdfColor.Black, PdfLineType.Normal);
      page.AddLine(130, 500, 170, 500, 1.0F, PdfColor.Black, PdfLineType.Outlined);
      page.AddLine(180, 500, 240, 500, 1.0F, PdfColor.Black, PdfLineType.OutlinedThin);
      page.AddLine(250, 500, 290, 500, 1.0F, PdfColor.Black, PdfLineType.OutlinedBold);

      // rectangles
      page.AddRectangle(100, 750, 200, 700, PdfColor.Blue);
      page.AddRectangle(210, 750, 250, 680, PdfColor.Red, 0.1F);
      page.AddRectangle(260, 750, 300, 700, PdfColor.FromRgb(10, 200, 100), 2.3F, PdfColor.DarkRed);

      // circles
      page.AddCircle(-100, 100, 100, PdfColor.Blue);
      page.AddCircle(-50, 100, 50, PdfColor.Red, 0.0F);
      page.AddCircle(300, 700, 50, PdfColor.FromRgb(150, 150, 50), 2.0F, PdfColor.LightBlue);

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }

    private void button2_Click(object sender, EventArgs e)
    {
      var fileName = textBox1.Text;
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
        return;

      var document = new PdfDocument("my title..", "me..");

      var page = document.AddPage();
      var image = page.AddImage(fileName, 100, 20);
      image.X = 50;
      image.Y = 700;

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }

    private void button3_Click(object sender, EventArgs e)
    {
      var dialog = new OpenFileDialog();
      var result = dialog.ShowDialog();
      if (result == DialogResult.Yes || result == DialogResult.OK)
      {
        textBox1.Text = dialog.FileName;
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      var document = new PdfDocument("my title..", "me..");
      document.Fonts.Add(PdfFont.Courier);

      var sizes = new[]
      {
        Tuple.Create("Letter", PdfPageSize.Letter()),
        Tuple.Create("A0", PdfPageSize.A0()),
        Tuple.Create("A1", PdfPageSize.A1()),
        Tuple.Create("A2", PdfPageSize.A2()),
        Tuple.Create("A3", PdfPageSize.A3()),
        Tuple.Create("A4", PdfPageSize.A4()),
        Tuple.Create("A5", PdfPageSize.A5()),
        Tuple.Create("B4", PdfPageSize.B4()),
        Tuple.Create("B5", PdfPageSize.B5()),
        Tuple.Create("Custom", new PdfSize(PdfUnit.Default, 100, 50))
      };

      foreach (var size in sizes)
      {
        var page = document.AddPage(size.Item2);
        page.AddRectangle(0, 0, size.Item2.Width, size.Item2.Height, PdfColor.White, 2.0F, PdfColor.Red);
        var text = page.AddText(string.Format("{0}: w={1} h={2}", size.Item1, size.Item2.Width, size.Item2.Height), 10, PdfFont.Courier);
        text.X = 0;
        text.Y = size.Item2.Height - 20;
      }

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }

    private void button5_Click(object sender, EventArgs e)
    {
      var document = new PdfDocument("my title..", "me..");
      document.Fonts.Add(PdfFont.Courier);

      var units = new[]
      {
        Tuple.Create("Point: 1 pt", PdfUnit.Point),
        Tuple.Create("Millimeter 2.83 pt", PdfUnit.Millimeter),
        Tuple.Create("Centimeter 28.3 pt", PdfUnit.Centimeter),
        Tuple.Create("Inch: 72 pt", PdfUnit.Inch),
        Tuple.Create("Custom: 100 pt", new PdfUnit(100))
      };

      foreach (var unit in units)
      {
        var page = document.AddPage(new PdfSize(unit.Item2, 200, 300)); // create 200x300 units page
        page.AddRectangle(0, 280, 10, 290, PdfColor.Blue, 0.0F, PdfColor.White);
        var text = page.AddText(unit.Item1 + "(upper rectangle's size is 10x10 units)", 5, PdfFont.Courier);
        text.X = 0;
        text.Y = 270;                                             
      }

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }

    private void button6_Click(object sender, EventArgs e)
    {
      var document = new PdfDocument("my title..", "me..");
      document.Fonts.Add(PdfFont.Courier);

      // page 1 - milimeters

      var page = document.AddPage(PdfPageSize.A4(PdfUnit.Millimeter));

      var text = page.AddText("User units of this page are millimeters", 5, PdfFont.Courier);
      text.X = 10;
      text.Y = 280;

      page.AddRectangle(10, 265, 22, 275, PdfColor.Blue, 0.0F, PdfColor.White);

      text = page.AddText("This is 10x12 mm rectangle. This message font is of 5 pt", 5, PdfFont.Courier);
      text.X = 10;
      text.Y = 260;

      // page 2 - centimeters

      page = document.AddPage(PdfPageSize.A4(PdfUnit.Centimeter));

      text = page.AddText("User units of this page are centimeters", 0.5F, PdfFont.Courier);
      text.X = 1;
      text.Y = 28;

      page.AddRectangle(1, 26.5F, 2.2F, 27.5F, PdfColor.Blue, 0.0F, PdfColor.White);

      text = page.AddText("This is 1x1.2 cm rectangle. This message font is of 0.5 pt", 0.5F, PdfFont.Courier);
      text.X = 1;
      text.Y = 26;

      // page 3 - line with 1/150 inch thickness (print with >= 150 dpi)

      var thickness = 72.0F / 150.0F;

      page = document.AddPage(PdfPageSize.A4());

      text = page.AddText("User units of this page are 1 pt = 1/72 inch", 10, PdfFont.Courier);
      text.X = 10;
      text.Y = 780;

      page.AddLine(10, 760, 510, 760, thickness, PdfColor.Blue);
      text = page.AddText("This is a line of 1/150 inch = 0.163 mm thickness", 10, PdfFont.Courier);
      text.X = 10;
      text.Y = 750;

      page.AddLine(10, 730, 510, 730, 5 * thickness, PdfColor.Blue);
      text = page.AddText("This is a line of 5/150 inch = 0.816 thickness", 10, PdfFont.Courier);
      text.X = 10;
      text.Y = 720;

      page.AddLine(10, 700, 510, 700, 10 * thickness, PdfColor.Blue);
      text = page.AddText("This is a line of 10/150 inch = 1.63 mm thickness", 10, PdfFont.Courier);
      text.X = 10;
      text.Y = 690;

      page.AddLine(10, 670, 510, 670, 30 * thickness, PdfColor.Blue);
      text = page.AddText("This is a line of 30/150 inch = 4.9 mm thickness", 10, PdfFont.Courier);
      text.X = 10;
      text.Y = 655;

      text = page.AddText("Lenght of all lines is 500 unit =176.4 mm", 10, PdfFont.Courier);
      text.X = 10;
      text.Y = 630;

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }
  }
}
