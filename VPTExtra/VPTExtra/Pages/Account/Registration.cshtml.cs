using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        private readonly UserService _userService;
        [BindProperty]       
        public User userToRegister { get; set; }
        public RegistrationModel(UserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPostRegister() 
        {
            if (_userService.RegisterUser(userToRegister) != null)
            {
                return RedirectToPage("/Account/Login");
            }
            return null;
        }
    }
}
