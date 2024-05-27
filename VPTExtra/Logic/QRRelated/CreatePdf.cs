using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace Logic.QRRelated
{
    public class CreatePdf
    {
        public byte[] CreateTicketPdf(int userId)
        {
            // Set the font resolver
            GlobalFontSettings.FontResolver = new FontResolver();

            // Create a new PDF document
            var pdfDoc = new PdfDocument();

            // Add a page
            PdfPage page = pdfDoc.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyleEx.Bold);

            // Draw the text
            gfx.DrawString("Hello World! " + userId, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            // Save the document into a MemoryStream
            using (var stream = new MemoryStream())
            {
                pdfDoc.Save(stream);
                return stream.ToArray();
            }
        }
    }

    // Custom font resolver
    public class FontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            if (faceName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
            {
                // Load and return the Verdana font file
                string fontPath = "Verdana.ttf"; // Path to your Verdana font file
                return File.ReadAllBytes(fontPath);
            }
            return null;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
            {
                // Return font information for Verdana
                return new FontResolverInfo("Verdana");
            }
            return null;
        }
    }
}
