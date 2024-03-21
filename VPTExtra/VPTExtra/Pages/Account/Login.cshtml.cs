using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;
        [BindProperty]
        public User userToLogin { get; set; }
        public LoginModel(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") != null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public IActionResult OnPostLogin()
        {
        
            User userToBeLogged = _userService.LoginUser(userToLogin);
            if (userToBeLogged != null)
            {
                HttpContext.Session.SetInt32("uId", userToBeLogged.Id);
                HttpContext.Session.SetString("uName", userToBeLogged.Name);
                HttpContext.Session.SetInt32("uRoleId", userToBeLogged.RoleId);

                return RedirectToPage("/Index");
            }
            return null;
        }
    }
}
