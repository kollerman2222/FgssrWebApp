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
using fgssr.UnitOFWork;
using static System.Collections.Specialized.BitVector32;

namespace fgssr.Areas.Identity.Pages.Account.Manage
{
    public class DiplomaStatusModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitofwork;



        public DiplomaStatusModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , IUnitOfWork unitofwork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitofwork = unitofwork;
        }





        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {

            [Display(Name = "FullNameEnglish")]
            public string FullNameEnglish { get; set; }

            [StringLength(250)]
            [Display(Name = "FullNameArabic")]
            public string FullNameArabic { get; set; }

            [Display(Name = "Admission Status")]
            public string ApplicationStatus { get; set; }

            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Diploma")]
            public string Diploma { get; set; }

            [Display(Name = "Birth Date")]
            public DateOnly BirthDate { get; set; }

            public string ProfileImage { get; set; }

            [Required]
            [StringLength(20)]
            [Display(Name = "NationalId")]
            public string NationalId { get; set; }

            public string StudentNumber { get; set; }

            public string AdmissionNumber { get; set; }

            public string DeclineReason { get; set; }

        }


        public async Task<IActionResult> OnGetAsync()
        {
            var sectionName = "";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if(user.SectionId != null)
            {
                var dip = await _unitofwork.Sections.GetByIdAsync(user.SectionId);
                if (dip != null)
                {
                    sectionName = dip.SectionName;
                }
            }

         

           
            Input = new InputModel
            {

                FullNameEnglish = user.FullNameEnglish,
                FullNameArabic = user.FullNameArabic,
                ApplicationStatus = user.ApplicationStatus,
                PhoneNumber = user.PhoneNumber,
                Diploma = sectionName,
                BirthDate =user.BirthDate,
                ProfileImage = user.ProfileImage,
                AdmissionNumber=user.AdmissionNumber,
                DeclineReason = user.DeclineReason,
                StudentNumber = user.StudentNumber,
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

            user.ApplicationStatus = null;
            user.SectionId = null;
            user.StudentNumber = null;
            user.AdmissionNumber = null;
            user.DeclineReason = null;

            await _userManager.UpdateAsync(user);

            return Page();

		}



	}
}
