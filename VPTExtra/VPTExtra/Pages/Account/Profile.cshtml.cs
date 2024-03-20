using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public List<Event> _visitorEvents { get; set; }       
        public User user { get; set; }       

        private readonly UserProfileDataService _userProfileDataService;
        private readonly UserService _userService;
        public ProfileModel(UserProfileDataService userProfileDataService, UserService userService)
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

            user = _userService.GetVisitorById(visitorId);
            _visitorEvents = _userProfileDataService.RetrieveUserEvents(visitorId);

            return null;
        }
    }
}
