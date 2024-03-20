using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public List<Event> _visitorEvents { get; set; }       

        private readonly UserProfileDataService _userProfileDataService;
        public ProfileModel(UserProfileDataService userProfileDataService)
        {
            _userProfileDataService = userProfileDataService;
        }

        public void OnGet()
        {
            int visitorId = (int)HttpContext.Session.GetInt32("uId");

            _visitorEvents = _userProfileDataService.RetrieveUserEvents(visitorId);
        }
    }
}
