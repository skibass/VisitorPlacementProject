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
        public void OnGet()
        {

        }
        [HttpPost]
        public IActionResult OnPostGenerateEvent() 
        {
            _eventGenerationService.GenerateEvent(_event, AmountOfParts, AmountOfRows);

            return RedirectToPage();
        }
    }
}
