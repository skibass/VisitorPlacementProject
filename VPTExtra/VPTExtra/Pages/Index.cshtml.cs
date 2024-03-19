using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventManagement _eventManagement;
        public List<Event> Events { get; set; }

        public IndexModel(IEventManagement eventManagement)
        {
            _eventManagement = eventManagement;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") != null)
            {
                Events = _eventManagement.GetEvents();
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}