using Interfaces.Logic.QRRelated;
using Interfaces.Repositories;
using Logic.Services;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
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

            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            var tf = new XTextFormatter(graph);
            var rect = new XRect(50, 150, 500, 500);

            XPen xpen = new XPen(XColors.Navy, 0.4);

            graph.DrawRectangle(xpen, rect);
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;

            XBrush brush = XBrushes.Purple;
            tf.DrawString($"Event: {location} \n\nSeats: {seats}",
                          new XFont("Verdana", 25),
                          brush,
                          new XRect(rect.X + 5, rect.Y, rect.Width - 5, 500), format);

            using (var stream = new MemoryStream())
            {
                pdf.Save(stream);
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
