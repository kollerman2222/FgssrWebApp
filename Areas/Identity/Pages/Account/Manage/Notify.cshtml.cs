using fgssr.CustomMethodsServices;
using fgssr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace fgssr.Areas.Identity.Pages.Account.Manage
{
    public class NotifyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public NotifyModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }



        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {

            [StringLength(250)]
            [Display(Name = "FullNameArabic")]
            public string? FullNameArabic { get; set; }
            public string? ProfileImage { get; set; }

            public string? StudentNumber { get; set; }

        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {

                FullNameArabic = user.FullNameArabic,
                ProfileImage = user.ProfileImage,

            };

            return Page();
        }
       
    }
}
