using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace VPTExtra.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        [BindProperty]
        public User userToLogin { get; set; }
        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostLogin()
        {
            User userToBeLogged = _userRepository.LoginUser(userToLogin);
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
