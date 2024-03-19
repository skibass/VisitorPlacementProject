using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        [BindProperty]       
        public User userToRegister { get; set; }
        public RegistrationModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void OnGet()
        {
        }
        public void OnPostRegister() 
        {
            if (_userRepository.RegisterUser(userToRegister) != null)
            {
                HttpContext.Session.SetString("isLoggedIn", "true");
            }
        }
    }
}
