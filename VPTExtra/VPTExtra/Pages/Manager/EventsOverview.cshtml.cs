using Interfaces;
using Interfaces.Service;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class EventsOverviewModel : PageModel
    {
        private readonly IEventService _eventService;
        public List<Event> events { get; set; }
        public string ErrorMessage { get; set; }

        public EventsOverviewModel(IEventService eventService)
        {
            _eventService = eventService;
            events = new List<Event>();
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

            try
            {
                events = _eventService.GetAllEvents();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                _eventService.DeleteEvent(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
