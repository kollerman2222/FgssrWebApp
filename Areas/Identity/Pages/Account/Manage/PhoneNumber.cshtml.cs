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
    public class PhoneNumberModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISMSSystem _smssystem;


        public PhoneNumberModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISMSSystem smssystem)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _smssystem = smssystem;
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

            public bool IsPhoneConfirmed { get; set; }


            [StringLength(20)]
            public string? PhoneVerifyCode { get; set; }


            [StringLength(20)]
            public string? SendVerifyCode { get; set; }

            public DateTime PhoneCodeCreated { get; set; }


            [Phone]
            [Display(Name = "Current PhoneNumber")]
            public string CurrentPhoneNumber { get; set; }


            [Phone]
            [Display(Name = "New PhoneNumber")]
            public string NewPhoneNumber { get; set; }

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
                CurrentPhoneNumber = user.PhoneNumber,
                ProfileImage = user.ProfileImage,
                IsPhoneConfirmed=user.PhoneConfirmed,
                PhoneVerifyCode = user.PhoneVerifyCode,
                PhoneCodeCreated = user.PhoneCodeCreated,
                
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
            var code = VerifyCodeGenerator.GenerateRandomCode(111111, 999999);
            var phoneSend = await _smssystem.SendSms(user.PhoneNumber, code);
            if (phoneSend)
            {
                user.PhoneVerifyCode = code;
                user.PhoneCodeCreated = DateTime.Now;
                await _userManager.UpdateAsync(user);
            }

            StatusMessage = "Phone Number Verification code was sent successfully pls check your Phone SMS";


            return RedirectToPage();

        }

        public async Task<IActionResult> OnPostVerifyCodeAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (Input.SendVerifyCode == user.PhoneVerifyCode)
            {
                user.PhoneConfirmed = true;
                user.PhoneCodeCreated = DateTime.MinValue;
                user.PhoneVerifyCode = null;
                await _userManager.UpdateAsync(user);
                StatusMessage = "Phone Number Verification is successfull";
            }
            else
            {
                StatusMessage = "Something is wrong try again ";
            }


            return RedirectToPage();

        }



        public async Task<IActionResult> OnPostChangePhoneNumberAsync()
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

            if (Input.NewPhoneNumber != null && Input.NewPhoneNumber == user.PhoneNumber)
            {
                StatusMessage = "Your Phone Number is unchanged.";
            }

            if (Input.NewPhoneNumber != null && Input.NewPhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = Input.NewPhoneNumber;
                user.PhoneConfirmed = false;
                user.PhoneCodeCreated = DateTime.MinValue;
                user.PhoneVerifyCode = null;
                await _userManager.UpdateAsync(user);
                StatusMessage = "Phone Number changed successfully";

            }

            return RedirectToPage();
        }

    }
}
