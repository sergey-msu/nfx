using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

      // paragraph
      var paragraph = page.AddParagraph("asdsa sd asd asd sadas d asd asddddddddddddas sda   dasd asd as d sadsa asdsad sad as", 100, 12, 12, PdfFont.Times, PdfColor.DarkPurple, PdfHorizontalAlign.Left);
      paragraph.X = 10;
      paragraph.Y = 680;

      // lines
      page.AddLine(20, 620, 50, 620);
      page.AddLine(50, 620, 70, 600, 2.5F);
      page.AddLine(70, 600, 80, 550, 0.5F, PdfColor.Red);
      page.AddLine(80, 550, 80, 530, 1.0F, PdfColor.Green, PdfLineStyle.Outlined);
      page.AddLine(80, 530, 80, 500, 3.2F, PdfColor.DarkBlue, PdfLineStyle.OutlinedBold);
      page.AddLine(80, 500, 120, 500, 1.0F, PdfColor.Black, PdfLineStyle.Normal);
      page.AddLine(130, 500, 170, 500, 1.0F, PdfColor.Black, PdfLineStyle.Outlined);
      page.AddLine(180, 500, 240, 500, 1.0F, PdfColor.Black, PdfLineStyle.OutlinedThin);
      page.AddLine(250, 500, 290, 500, 1.0F, PdfColor.Black, PdfLineStyle.OutlinedBold);

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }
  }
}
