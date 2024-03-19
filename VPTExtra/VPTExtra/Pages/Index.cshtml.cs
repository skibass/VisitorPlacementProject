using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Services;
using Models;
using Interfaces;

namespace VPTExtra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventManagement _eventManagement;
        public List<Event> Events { get; set; }

        public IndexModel(IEventManagement eventManagement)        
        { 
            _eventManagement = eventManagement;
        }
        public void OnGet()
        {
            if (HttpContext.Session.GetString("isLoggedIn") == "false")
            {
                RedirectToPage("Acount/Registration");
            }
            else
            {
                Events = _eventManagement.GetEvents();
            }
        }
    }
}