using Interfaces.Logic;
using Interfaces.Logic.QRRelated;
using Logic.QRRelated;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VPTExtra.Pages.QRCode
{
    public class TicketDownloadPageModel : PageModel
    {
        private readonly ICreatePdf _createPdfService;

        public TicketDownloadPageModel(ICreatePdf createPdf) 
        {
            _createPdfService = createPdf;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnGetScan(int userId, int eventId)
        {           
            var pdfBytes = _createPdfService.CreateTicketPdf(userId, eventId);

            return File(pdfBytes, "application/pdf", "Ticket.pdf");
        }
    }
}
