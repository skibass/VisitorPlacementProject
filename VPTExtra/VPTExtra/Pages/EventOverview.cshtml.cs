using Interfaces;
using Interfaces.Service;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models;

namespace VPTExtra.Pages
{
    public class EventOverviewModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly IVisitorPlacementService _visitorPlacementService;
        public Event currentEvent { get; set; }
        private Event tempEvent {  get; set; }
        [BindProperty]
        public int currentUserId {  get; set; }
        public string ErrorMessage {  get; set; }

        public EventOverviewModel(IEventService eventService, IVisitorPlacementService visitorPlacementService)
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
                    currentEvent = tempEvent;
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

            try
            {
                _visitorPlacementService.PlaceVisitor(chairId, visitorId, eventId);

                return RedirectToPage(new { eventId = eventId });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        public IActionResult OnPostRevertPlacement(int chairId)
        {
            int visitorId = (int)HttpContext.Session.GetInt32("uId");

            int eventId = Convert.ToInt32(TempData["eventId"]);

            try
            {
                _visitorPlacementService.RevertVisitorPlacement(chairId, visitorId, eventId);

                return RedirectToPage(new { eventId = eventId });

            }
            catch (Exception ex)
            {
                ErrorMessage=ex.Message;

                return Page();
            }
        }
    }
}
