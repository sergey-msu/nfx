using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NFX.Media.PDF.Elements
{
	/// <summary>
	/// PDF image element
	/// </summary>
	public class ImageElement : PdfElementBase
	{
		public ImageElement(string filePath)
		{
			loadImage(filePath);
		}

		public ImageElement(string filePath, float width, float height)
		{
			loadImage(filePath);

			Width = width;
			Height = height;
		}

		private int m_XObjectId;

		/// <summary>
		/// Image unique object Id
		/// </summary>
		public int XObjectId
		{
			get { return m_XObjectId; }
		}

		/// <summary>
		/// PDF displayed image width
		/// </summary>
		public float Width { get; private set; }

		/// <summary>
		/// PDF displayed image height
		/// </summary>
		public float Height { get; private set; }

		/// <summary>
		/// Image bytes
		/// </summary>
		public byte[] Content { get; private set; }

		/// <summary>
		/// Image's own widht
		/// </summary>
		public float OwnWidth { get; private set; }

		/// <summary>
		/// Image's own height
		/// </summary>
		public float OwnHeight { get; private set; }

		/// <summary>
		/// Writes element into file stream
		/// </summary>
		/// <param name="writer">PDF writer</param>
		/// <returns>Written bytes count</returns>
		public override long Write(PdfWriter writer)
		{
			return writer.Write(this);
		}

		internal override void SetId(ObjectIdGenerator generator)
		{
			base.SetId(generator);

			if (m_XObjectId != 0)
				throw new InvalidOperationException("Image's x-object Id has already been setted.");

			m_XObjectId = generator.GenerateId();
		}

		private void loadImage(string filePath)
		{
			using (var image = Image.FromFile(filePath))
			using (var stream = new MemoryStream())
			{
				image.Save(stream, ImageFormat.Jpeg);
				Content = stream.ToArray();
				Width = image.Width;
				OwnWidth = image.Width;
				Height = image.Height;
				OwnHeight = image.Height;
			}
		}
	}
}
