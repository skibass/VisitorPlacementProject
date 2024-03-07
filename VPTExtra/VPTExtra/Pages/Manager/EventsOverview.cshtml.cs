using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class EventsOverviewModel : PageModel
    {
        private readonly IEventManagement _eventManagement;
        public List<Event> Events { get; set; }

        public EventsOverviewModel(IEventManagement eventManagement)
        {
            _eventManagement = eventManagement;
        }
        public void OnGet()
        {
            Events = _eventManagement.GetEvents();
        }

        [HttpPost]
        public IActionResult OnPostDelete(int id)
        {
            _eventManagement.DeleteEvent(id);
           return RedirectToPage();
        }
    }
}
