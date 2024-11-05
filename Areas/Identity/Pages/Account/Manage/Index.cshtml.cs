// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using fgssr.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Collections.Specialized.BitVector32;

namespace fgssr.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;

        public IndexModel(
            UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager , IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/users";
        }

        
        //public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "username")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            
            [StringLength(250)]
            [Display(Name = "FullNameEnglish")]
            public string FullNameEnglish { get; set; }

            
            [StringLength(250)]
            [Display(Name = "FullNameArabic")]
            public string FullNameArabic { get; set; }

            
            [StringLength(250)]
            [Display(Name = "Address")]
            public string Address { get; set; }

			[Required]
			[StringLength(20)]
			[Display(Name = "NationalId")]
			public string NationalId { get; set; }
			[StringLength(50)]
            public string Gender { get; set; } = string.Empty;

            
            [DataType(DataType.DateTime)]
            [Display(Name = "Birth date")]
            public DateOnly BirthDate { get; set; }


            public string? ProfileImage { get; set; } = string.Empty;


            public IFormFile? EditProfileImage { get; set; }



            [Display(Name = "Current email")]
            public string CurrentEmail { get; set; }

            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

            
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string CurrentPassword { get; set; }


            
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmNewPassword { get; set; }

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
                Username= user.UserName,
                PhoneNumber = user.PhoneNumber,
                FullNameArabic=user.FullNameArabic,
                Address = user.Address,
                FullNameEnglish = user.FullNameEnglish,
                BirthDate= user.BirthDate,
                CurrentEmail=user.Email,
                ProfileImage=user.ProfileImage,
                StudentNumber=user.StudentNumber,
                NationalId=user.NationalId
               
          };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldImage = user.ProfileImage;

            user.NationalId = Input.NationalId;
            user.BirthDate=Input.BirthDate;
            //user.PhoneNumber=Input.PhoneNumber;
            user.Address=Input.Address;
            user.FullNameEnglish=Input.FullNameEnglish;
            user.FullNameArabic=Input.FullNameArabic;
            user.ProfileImage=Input.ProfileImage;
            if (Input.EditProfileImage!= null)
            {
                user.ProfileImage = await SaveNewImage(Input.EditProfileImage);
                if(oldImage!=null)
                {
                    var oldImageDelete = Path.Combine(_uploadFolderPath, oldImage);
                    System.IO.File.Delete(oldImageDelete);
                }
              
            }
            
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<string> SaveNewImage(IFormFile image)
        {

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = System.IO.File.Create(imageSaveLocation);
            await image.CopyToAsync(stream);

            return imageName;
        }


    }
}
