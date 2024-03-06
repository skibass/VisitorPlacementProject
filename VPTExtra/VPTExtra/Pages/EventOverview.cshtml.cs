using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages
{
    public class EventOverviewModel : PageModel
    {
        private readonly IEventManagement _eventManagement;
        public Event Event { get; set; }

        public EventOverviewModel(IEventManagement eventManagement)
        {
            _eventManagement = eventManagement;
        }
        public void OnGet(int id)
        {
            Event = _eventManagement.GetEventById(id);
        }
    }
}
