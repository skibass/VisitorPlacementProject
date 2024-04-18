using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;
using Interfaces.Logic;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        public List<Event> events { get; set; }
        public string ErrorMessage { get; set; }
        public IndexModel(IEventService eventService)
        {
            _eventService = eventService;
            events = new List<Event>();
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                events = _eventService.GetAllEvents();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error retrieving events.";
            }
            return Page();
        }
    }
}