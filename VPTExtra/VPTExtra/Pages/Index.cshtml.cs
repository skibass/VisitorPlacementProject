using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly EventService _eventService;
        public List<Event> Events { get; set; }

        public IndexModel(EventService eventService)
        {
            _eventService = eventService;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") != null)
            {
                Events = _eventService.GetAllEvents();
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}