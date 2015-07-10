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
			document.Fonts.Add(PdfFont.Helvetica);

			var page = document.AddPage();
			var text = page.AddText("hello world!!!", 20, PdfFont.Helvetica, PdfColor.DarkGreen);
			text.X = 10;
			text.Y = 700;

			document.Save(@"test.pdf");

			Process.Start(@"test.pdf");
		}
	}
}
