using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class CreateEventModel : PageModel
    {
        [BindProperty]
        public Event _event {  get; set; }
        [BindProperty]
        public int AmountOfParts { get; set; }
        [BindProperty]
        public int AmountOfRows { get; set; }

        private readonly EventGenerationService _eventGenerationService;
        public CreateEventModel(EventGenerationService eventGenerationService) 
        { 
            _eventGenerationService = eventGenerationService;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uRoleId") != 2)
            {
                return RedirectToPage("/Account/Login");
            }
            return null;
        }
        public IActionResult OnPostGenerateEvent() 
        {
            _eventGenerationService.GenerateEvent(_event, AmountOfParts, AmountOfRows);

            return RedirectToPage();
        }
    }
}
