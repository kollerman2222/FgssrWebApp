using fgssr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fgssr.Controllers
{
    public class RolesController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;


        public RolesController(RoleManager<IdentityRole> roleManager) 
        {
            
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {

            var getRoles =_roleManager.Roles.ToList();
            return View(getRoles);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel RVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            await _roleManager.CreateAsync(new IdentityRole(RVM.RoleName));
            
            return RedirectToAction("Homepage", "Home");
        }

    }
}
