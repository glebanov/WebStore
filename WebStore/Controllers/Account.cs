using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _UserManager;
        private RoleManager<Role> _RoleManager;

        public AccountController(UserManager<User> UserManeger, RoleManager<Role> RoleManeger)
        {
            _UserManager = UserManeger;
            _RoleManager = RoleManeger;
        }
  
    }
}
