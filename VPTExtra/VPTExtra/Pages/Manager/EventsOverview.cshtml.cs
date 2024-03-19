using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class EventsOverviewModel : PageModel
    {
        private readonly EventService _eventService;
        public List<Event> Events { get; set; }

        public EventsOverviewModel(EventService eventService)
        {
            _eventService = eventService;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else if (HttpContext.Session.GetInt32("uRoleId") != 2)
            {
                return RedirectToPage("/Index");
            }

            Events = _eventService.GetAllEvents();
            return null;
        }

        public IActionResult OnPostDelete(int id)
        {
            _eventService.DeleteEvent(id);
           return RedirectToPage();
        }
    }
}
