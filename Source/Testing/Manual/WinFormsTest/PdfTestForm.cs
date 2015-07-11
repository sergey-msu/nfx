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
      var content = "abc"; // "иς界z"
      var text = page.AddText(content, 20, PdfFont.CourierBold, PdfColor.DarkGreen);
      text.X = 10;
      text.Y = 700;

      var paragraph = page.AddParagraph("asdsa sd asd asd sadas d asd asddddddddddddas sda   dasd asd as d sadsa asdsad sad as", 100, 12, 12, PdfFont.Times, PdfColor.DarkPurple, PdfHorizontalAlign.Left);
      paragraph.X = 10;
      paragraph.Y = 600;

      document.Save(@"test.pdf");

      Process.Start(@"test.pdf");
    }
  }
}
