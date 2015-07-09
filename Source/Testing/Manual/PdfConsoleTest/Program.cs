using System.Diagnostics;
using NFX.PDF.DocumentModel;
using NFX.PDF.Styling;

namespace PdfConsoleTest
{
	class Program
	{
		static void Main(string[] args)
		{
			SimpleText();
		}

		private static void SimpleText()
		{
			var document = new PdfDocument("my title..", "me..");
			document.Fonts.Add(Font.Helvetica);

			var page = document.AddPage();
			var text = page.AddText("hello world!!!", 20, Font.Helvetica, Color.DarkGreen);
			text.X = 10;
			text.Y = 700;

			document.Save(@"test.pdf");

			Process.Start(@"test.pdf");
		}
	}
}
