using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _UserManager;
        private SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManeger, SignInManager<User> SignInManager)
        {
            _UserManager = UserManeger;
            _SignInManager = SignInManager;
        }

         public IActionResult Register() => View(new RegisterUserViewModel());
      

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> Register (RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var user = new User
            {
                UserName = Model.UserName
            };

            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(Model);
        }
  
    }
}
