// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using fgssr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using fgssr.CustomMethodsServices;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Mail;

namespace fgssr.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSystem _emailSystem;


        public EmailModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSystem emailSystem)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSystem = emailSystem; 
        }

       
      

      
        [TempData]
        public string StatusMessage { get; set; }

       
        [BindProperty]
        public InputModel Input { get; set; }

        
        public class InputModel
        {

            [StringLength(250)]
            [Display(Name = "FullNameEnglish")]
            public string FullNameEnglish { get; set; }

            [StringLength(250)]
            [Display(Name = "FullNameArabic")]
            public string FullNameArabic { get; set; }
            public string ProfileImage { get; set; }

            public bool IsEmailConfirmed { get; set; }


            [StringLength(20)]
            public string? EmailVerifyCode { get; set; }


            [StringLength(20)]
            public string? SendVerifyCode { get; set; }

            public DateTime EmailCodeCreated { get; set; }


            [EmailAddress]
            [Display(Name = "Current email")]
            public string CurrentEmail { get; set; }

            
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

            public string StudentNumber { get; set; }

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
                
                FullNameEnglish = user.FullNameEnglish,
                FullNameArabic = user.FullNameArabic,
                CurrentEmail = user.Email,
                ProfileImage = user.ProfileImage,
                IsEmailConfirmed=user.EmailConfirmed,
                EmailCodeCreated = user.EmailCodeCreated,
                EmailVerifyCode = user.EmailVerifyCode,
            };

            return Page();
        }


        public async Task<IActionResult> OnPostGenerateCodeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var code =  VerifyCodeGenerator.GenerateRandomCode(111111, 999999);
            var emailSend = await _emailSystem.SendEmail(user.Email, "Email Verification Code", code, user.FullNameEnglish);
            if (emailSend)
            {
                user.EmailVerifyCode = code;
                user.EmailCodeCreated = DateTime.Now;
                await _userManager.UpdateAsync(user);
            }

            //user.EmailVerifyCode = code;
            //user.EmailCodeCreated = DateTime.Now;
            //await _userManager.UpdateAsync(user);
            StatusMessage = "Email Verification code was sent successfully pls check your Email";


            return RedirectToPage();

        }

        public async Task<IActionResult> OnPostVerifyCodeAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if(Input.SendVerifyCode == user.EmailVerifyCode || Input.SendVerifyCode=="12345") //12345 code incase email failed for presentaion purpose only
            {
                user.EmailConfirmed = true;
                user.EmailCodeCreated = DateTime.MinValue;
                user.EmailVerifyCode = null;
               await _userManager.UpdateAsync(user);
                StatusMessage = "Email Verification is successfull";
            }
            else
            {
                StatusMessage = "Error Something is wrong try again ";
            }


            return RedirectToPage();

        }



        public async Task<IActionResult> OnPostChangeEmailAsync()
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                if (!ModelState.IsValid)
                {
                    return RedirectToAction(nameof(OnGetAsync));
                }

                var searchRandomUserWithEmail = await _userManager.FindByEmailAsync(Input.NewEmail);
                if(searchRandomUserWithEmail != null && searchRandomUserWithEmail.Id != user.Id)
                {
                    StatusMessage = "Error Email is already taken";

                }
                else if (Input.NewEmail != null && Input.NewEmail == user.Email)
                {
                    StatusMessage = "Your email is unchanged";
                }
                else if(Input.NewEmail != null && Input.NewEmail != user.Email)
                {
                    user.Email = Input.NewEmail;
                    user.UserName = new MailAddress(Input.NewEmail).User;
                    user.EmailConfirmed = false;
                    user.EmailCodeCreated= DateTime.MinValue;
                    user.EmailVerifyCode = null;
                    await _userManager.UpdateAsync(user);
                    StatusMessage = "Email changed successfully";
                 }

                return RedirectToPage();
             }
        
    }
}
