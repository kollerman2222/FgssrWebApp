// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using fgssr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace fgssr.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
            
        public RegisterModel(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;           
            _signInManager = signInManager;               
        }

        
        [BindProperty]
        public InputModel Input { get; set; }

        
        public string ReturnUrl { get; set; }

        
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

           
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(250)]
            [Display(Name = "FullNameEnglish")]
            public string FullNameEnglish { get; set; }

            [Phone]
            [Required]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }


            [Required]
            [StringLength(250)]
            [Display(Name = "FullNameArabic")]
            public string FullNameArabic { get; set; }

            [Required]
            [StringLength(20)]
            [Display(Name = "NationalId")]
            public string NationalId { get; set; } 

            [Required]
            [StringLength(250)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [StringLength(50)]
            [AllowedValues("أنثي","ذكر",ErrorMessage ="أختر ذكر أو أنثي" )]
            public string Gender { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.DateTime)]
            [Display(Name = "Birth date")]
            public DateOnly BirthDate { get; set; }



        }


        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = new MailAddress(Input.Email).User,
                    Email = Input.Email,
                    FullNameArabic = Input.FullNameArabic,
                    FullNameEnglish= Input.FullNameEnglish,
                    Address= Input.Address,
                    Gender= Input.Gender,
                    BirthDate= Input.BirthDate,
                    PhoneNumber= Input.PhoneNumber,
                    PhoneConfirmed = false,
                    NationalId= Input.NationalId
                   
                };


                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        
    }
}
