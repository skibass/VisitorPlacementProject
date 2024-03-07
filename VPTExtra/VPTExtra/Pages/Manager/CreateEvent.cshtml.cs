using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Manager
{
    public class CreateEventModel : PageModel
    {
        private readonly EventGenerationService _eventGenerationService;
        public CreateEventModel(EventGenerationService eventGenerationService) 
        { 
            _eventGenerationService = eventGenerationService;
        }
        public void OnGet()
        {
        }
        public void OnPostGenerateEvent() 
        {
            Event _event = new();
            _eventGenerationService.GenerateEvent(_event, 0, 0);
        }
    }
}
