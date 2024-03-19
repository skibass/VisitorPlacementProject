using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models;

namespace VPTExtra.Pages
{
    public class EventOverviewModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly IVisitorPlacement _visitorPlacement;
        public Event Event { get; set; }
        private Event tempEvent {  get; set; }
        [BindProperty]
        public int currentUserId {  get; set; }

        public EventOverviewModel(IEventRepository eventManagement, IVisitorPlacement visitorPlacement)
        {
            _eventRepository = eventManagement;
            _visitorPlacement = visitorPlacement;
        }
        public IActionResult OnGet(int eventId)
        {
            currentUserId = (int)HttpContext.Session.GetInt32("uId");

            if (currentUserId != null)
            {
                tempEvent = _eventRepository.GetEventById(eventId);

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

            _visitorPlacement.PlaceVisitor(chairId, visitorId);

            return RedirectToPage(new { eventId = eventId });
        }
    }
}
