using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class EventsOverviewModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        public List<Event> Events { get; set; }

        public EventsOverviewModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
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

            Events = _eventRepository.GetAllEvents();
            return null;
        }

        public IActionResult OnPostDelete(int id)
        {
            _eventRepository.DeleteEvent(id);
           return RedirectToPage();
        }
    }
}
