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
        public void OnGet()
        {
            Events = _eventRepository.GetAllEvents();
        }

        public IActionResult OnPostDelete(int id)
        {
            _eventRepository.DeleteEvent(id);
           return RedirectToPage();
        }
    }
}
