using Interfaces.Service;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public List<Event> visitorEvents { get; set; }       
        public User user { get; set; }       
        public string ErrorMessage { get; set; }       

        private readonly IUserProfileService _userProfileDataService;
        private readonly IUserService _userService;
        public ProfileModel(IUserProfileService userProfileDataService, IUserService userService)
        {
            _userProfileDataService = userProfileDataService;
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
 
            int visitorId = (int)HttpContext.Session.GetInt32("uId");

            try
            {
                user = _userService.GetVisitorById(visitorId);
                visitorEvents = _userProfileDataService.RetrieveUserEvents(visitorId);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}
