using fgssr.CustomMethodsServices;
using fgssr.Models;
using fgssr.UnitOFWork;
using fgssr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace fgssr.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _uploadFolderPath;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {

     

            var users = await _userManager.Users.ToListAsync();

            var getUsers = new List<UsersViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                

                var userModel = new UsersViewModel
                {
                    UserId = user.Id,
                    FullNameEnglish = user.FullNameEnglish,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ApplicationStatus = user.ApplicationStatus,
                    ProfileImage = user.ProfileImage,
                    SectionName= await _unitofwork.Sections.GetNamebyIdAsync(user.SectionId),
                    Roles = userRoles.Select(roleName => new RoleViewModel
                    {
                        RoleName = roleName
                    }).ToList()
                };

                getUsers.Add(userModel);
            }

            return View(getUsers);




        }

        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var getUser = new UsersViewModel
            {

                UserId = user.Id,
                FullNameEnglish = user.FullNameEnglish,
                BirthDate = user.BirthDate,
                Email = user.Email,  
                PhoneNumber=user.PhoneNumber,

            };
                    
            return View(getUser);

        }


        public async Task<IActionResult> Edit(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var getUser = new UsersViewModel
            {

                UserId = user.Id,
                FullNameArabic = user.FullNameArabic,
                PhoneNumber = user.PhoneNumber,
                //ApplicationStatus = user.ApplicationStatus,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user ,role.Name).Result
                    
                }).ToList()
            };

            return View(getUser);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersViewModel UVM)
        {
            var user = await _userManager.FindByIdAsync(UVM.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var roles= await _userManager.GetRolesAsync(user);

            foreach (var role in UVM.Roles)
            {
                if (roles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!roles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            //user.PhoneNumber = UVM.PhoneNumber;
            //user.FullNameEnglish = UVM.FullNameEnglish;
            //user.ApplicationStatus = UVM.ApplicationStatus;

           await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Admission(int diplomaId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var getEmptyUser = new AdmissionViewModel
                {
                    BirthDate= null,
                    isEmailConfirmed = false,
                    isPhoneConfirmed = false
                };
                return View(getEmptyUser);
            }
            var section = await _unitofwork.Sections.GetByIdAsync(diplomaId);

            var getUser = new AdmissionViewModel
            {

                userId = user.Id,
                FullNameArabic = user.FullNameArabic,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                sectionId = section.SectionId,
                Email = user.Email,
                sectionName=section.SectionName,
                ApplicationStatus=user.ApplicationStatus,
                isEmailConfirmed=user.EmailConfirmed,
                isPhoneConfirmed=user.PhoneConfirmed,
                NationalId=user.NationalId
            };
            return View(getUser);

        }


        // add validation for admission button not to be added unless login and verified
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Admission(AdmissionViewModel AVM)
        {
            var user = await _userManager.FindByIdAsync(AVM.userId);
            if (user == null)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(AVM.sections));
            ModelState.Remove(nameof(AVM.DeclineReason));


            if (!ModelState.IsValid)
            {
                return View();
            }
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/gradcerts/{user.NationalId}";
            Directory.CreateDirectory(_uploadFolderPath);
            user.ApplicationStatus = "Pending";
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(AVM.GraduationCert.FileName)}";
            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);
            using var stream = System.IO.File.Create(imageSaveLocation);
            await AVM.GraduationCert.CopyToAsync(stream);
            user.GraduationCertUpload = imageName;
            user.SectionId = AVM.sectionId;
            AVM.AdmissionNumber = AdmissionNumberGenerator.GenerateAdmissionNumber(11111, 99999);
            user.AdmissionNumber=AVM.AdmissionNumber;
            await _userManager.UpdateAsync(user);

            return Redirect("/account/diplomastatus");

        }


        public async Task<IActionResult> GetAllEnquiry()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            //var section = await _unitofwork.Sections.GetByIdAsync(user.SectionId);
            

            var getUser = new AdmissionViewModel
            {
                UsersList = users,
                sections = await _unitofwork.Sections.GetAllAsync()
            };
            return View(getUser);

        }



        public async Task<IActionResult> GetPendingEnquiry()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            //var section = await _unitofwork.Sections.GetByIdAsync(user.SectionId);

            var getUser = new AdmissionViewModel
            {
               UsersList=users,
               sections = await _unitofwork.Sections.GetAllAsync()

            };
            return View(getUser);

        }
        public async Task<IActionResult> Enquiry(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var section = await _unitofwork.Sections.GetByIdAsync(user.SectionId);

            var getUser = new AdmissionViewModel
            {

                userId = user.Id,
                FullNameEnglish = user.FullNameEnglish,
                FullNameArabic =user.FullNameArabic,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Address = user.Address,
                sectionName = section.SectionName,
                GraduationCertUpload = user.GraduationCertUpload,
                ApplicationStatus = user.ApplicationStatus,
                sectionId=user.SectionId,
                AdmissionNumber=user.AdmissionNumber,
                NationalId=user.NationalId
            };
            return View(getUser);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enquiry(AdmissionViewModel AVM)
        {
            var user = await _userManager.FindByIdAsync(AVM.userId);
            if (user == null)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(AVM.GraduationCert));

            if(AVM.DeclineReason == null)
            {
                 ModelState.Remove(nameof(AVM.DeclineReason));

            }


            if (!ModelState.IsValid)
            {
                return View();
            }

            user.ApplicationStatus = AVM.ApplicationStatus;
            if (AVM.ApplicationStatus == "Approved")
            {
                await _userManager.AddToRoleAsync(user, "Student");
                AVM.StudentNumber = StudentNumberGenerator.GenerateStudentNumber(11111111111111, 99999999999999);

                user.StudentNumber = AVM.StudentNumber;
            }
            else if(AVM.ApplicationStatus == "Declined")
            {
                user.DeclineReason= AVM.DeclineReason;
            }
            


            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(GetPendingEnquiry));

        }


        public async Task<IActionResult> Delete(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index)); 
        }



       
       


    }
}
