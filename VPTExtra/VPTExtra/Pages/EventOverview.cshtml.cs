using Interfaces;
using Interfaces.Logic;
using Interfaces.DataAcces.API;
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
        private readonly IEventEditService _eventEditService;

        private readonly IEventApiService _apiService;

        public Event currentEvent { get; set; }
        private Event tempEvent { get; set; }
        [BindProperty]
        public int currentUserId {  get; set; }
        public string ErrorMessage {  get; set; }

        public EventOverviewModel(IEventService eventService, IVisitorPlacementService visitorPlacementService, IEventEditService eventEditService, IEventApiService apiService)
        {
            _eventService = eventService;
            _visitorPlacementService = visitorPlacementService;
            _eventEditService = eventEditService;
            _apiService = apiService;
        }
        public async Task<IActionResult> OnGet(int eventId)
        {
            currentUserId = (int)HttpContext.Session.GetInt32("uId");

            try
            {
                if (currentUserId != null)
                {
                    //tempEvent = _eventService.GetEventById(eventId);
                    tempEvent = await _apiService.GetEventById(eventId);

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
            catch (Exception ex)
            {
                ErrorMessage = "Error retrieving event.";
                return Page();
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
                ErrorMessage = "Error placing visitor.";
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
                ErrorMessage = "Error undoing placement.";

                return Page();
            }
        }

        
        public IActionResult OnPostAddPart(int eventId)
        {
            _eventEditService.AddPart(eventId);

            return RedirectToPage(new { eventId = eventId });
        }
        public IActionResult OnPostAddRow(int partId)
        {
            int eventId = Convert.ToInt32(TempData["eventId"]);

            _eventEditService.AddRow(partId);

            return RedirectToPage(new { eventId = eventId });
        }
        public IActionResult OnPostAddChair(int rowId)
        {
            int eventId = Convert.ToInt32(TempData["eventId"]);

            _eventEditService.AddChair(rowId, eventId);

            return RedirectToPage(new { eventId = eventId });
        }
    }
}
