using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;
using Interfaces.Service;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        public List<Event> events { get; set; }
        public string ErrorMessage { get; set; }
        public IndexModel(IEventService eventService)
        {
            _eventService = eventService;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") != null)
            {
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
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}