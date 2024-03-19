using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        public List<Event> Events { get; set; }

        public IndexModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") != null)
            {
                Events = _eventRepository.GetAllEvents();
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}