using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;
using Interfaces.Logic;
using API.Services;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly EventApiService _apiService;
        //public List<Event> events { get; set; }
        public List<Event> events { get; set; }
        public string ErrorMessage { get; set; }
        public IndexModel(IEventService eventService, EventApiService apiService)
        {
            _eventService = eventService;
            events = new List<Event>();
            _apiService = apiService;
        }
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                events = await _apiService.GetEvents();

                //events = _eventService.GetAllEvents();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error retrieving events.";
            }
            return Page();
        }
    }
}