using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages
{
    public class EventOverviewModel : PageModel
    {
        private readonly IEventManagement _eventManagement;
        private readonly IVisitorPlacement _visitorPlacement;
        public Event Event { get; set; }

        public EventOverviewModel(IEventManagement eventManagement, IVisitorPlacement visitorPlacement)
        {
            _eventManagement = eventManagement;
            _visitorPlacement = visitorPlacement;
        }
        public void OnGet(int eventId)
        {
            Event = _eventManagement.GetEventById(eventId);
            TempData["eventId"] = eventId;
        }

        public IActionResult OnPostPlaceVisitor(int chairId)
        {
            int eventId = Convert.ToInt32(TempData["eventId"]);

            _visitorPlacement.PlaceVisitor(chairId);

            return RedirectToPage(new { eventId = eventId });
        }
    }
}
