using Interfaces.Logic;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        private readonly IUserService _userService;
        [BindProperty]       
        public User userToRegister { get; set; }
        public RegistrationModel(IUserService userService)
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
