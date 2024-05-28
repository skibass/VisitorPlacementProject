using Interfaces.Logic.QRRelated;
using Interfaces.Repositories;
using Logic.Services;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace Logic.QRRelated
{
    public class CreatePdf : ICreatePdf
    {
        IUserProfileDataRepository userDataRepo;
        public CreatePdf(IUserProfileDataRepository userDataRepo)
        {
            this.userDataRepo = userDataRepo;
        }

        static CreatePdf()
        {
            GlobalFontSettings.FontResolver = new FontResolver();
        }

        public byte[] CreateTicketPdf(int userId, int eventId)
        {
            string seats = userDataRepo.RetrieveUserEventChairNames(userId, eventId).ChairNames;
            string location = userDataRepo.RetrieveUserEventChairNames(userId, eventId).Location;

            var pdfDoc = new PdfDocument();

            PdfPage page = pdfDoc.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Verdana", 20, XFontStyleEx.Bold);

            gfx.DrawString($"Event location: {location} \n Seats: {seats}", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            using (var stream = new MemoryStream())
            {
                pdfDoc.Save(stream);
                return stream.ToArray();
            }
        }
    }

    public class FontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            if (faceName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
            {
                string fontPath = "Verdana.ttf";
                return File.ReadAllBytes(fontPath);
            }
            return null;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Verdana");
            }
            return null;
        }
    }
}
