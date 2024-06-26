using Interfaces.Logic;
using Interfaces.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class CreateEventModel : PageModel
    {
        [BindProperty]
        public Event currentEvent {  get; set; }
        [BindProperty]
        public int AmountOfParts { get; set; }
        [BindProperty]
        public int AmountOfRows { get; set; }
        public string ErrorMessage { get; set; }
        private readonly IEventGenerationService _eventGenerationService;
        public CreateEventModel(IEventGenerationService eventGenerationService) 
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
            try
            {
                _eventGenerationService.GenerateEvent(currentEvent, AmountOfParts, AmountOfRows);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error creating event.";
                return Page();
            }

        }
    }
}
