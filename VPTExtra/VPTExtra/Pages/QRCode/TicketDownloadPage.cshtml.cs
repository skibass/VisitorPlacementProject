using Logic.QRRelated;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.QRRelated;
namespace VPTExtra.Pages.QRCode
{
    public class TicketDownloadPageModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnGetScan(int userId)
        {
            CreatePdf pdf = new CreatePdf();
            var pdfBytes = pdf.CreateTicketPdf(userId);

            // Return the PDF document as a file result
            return File(pdfBytes, "application/pdf", "Ticket.pdf");
        }
    }
}
