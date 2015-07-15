﻿using System;
using System.Diagnostics;
using System.IO;
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
	}
}
