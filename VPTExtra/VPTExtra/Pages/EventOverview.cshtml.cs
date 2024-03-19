using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models;

namespace VPTExtra.Pages
{
    public class EventOverviewModel : PageModel
    {
        private readonly EventService _eventService;
        private readonly VisitorPlacementService _visitorPlacementService;
        public Event Event { get; set; }
        private Event tempEvent {  get; set; }
        [BindProperty]
        public int currentUserId {  get; set; }

        public EventOverviewModel(EventService eventService, VisitorPlacementService visitorPlacementService)
        {
            _eventService = eventService;
            _visitorPlacementService = visitorPlacementService;
        }
        public IActionResult OnGet(int eventId)
        {
            currentUserId = (int)HttpContext.Session.GetInt32("uId");

            if (currentUserId != null)
            {
                tempEvent = _eventService.GetEventById(eventId);

                if (tempEvent != null)
                {
                    Event = tempEvent;
                    TempData["eventId"] = eventId;
                }
                else
                {
                    return RedirectToPage("/Index");
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }

        public IActionResult OnPostPlaceVisitor(int chairId)
        {
            int visitorId = (int)HttpContext.Session.GetInt32("uId");

            int eventId = Convert.ToInt32(TempData["eventId"]);

            _visitorPlacementService.PlaceVisitor(chairId, visitorId);

            return RedirectToPage(new { eventId = eventId });
        }
    }
}
