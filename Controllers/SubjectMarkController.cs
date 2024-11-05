using fgssr.Models;
using fgssr.UnitOFWork;
using fgssr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fgssr.Controllers
{
    public class SubjectMarkController : Controller
    {

        private readonly IUnitOfWork _unitofwork;
        private readonly UserManager<ApplicationUser> _userManager;


        public SubjectMarkController(IUnitOfWork unitofwork , UserManager<ApplicationUser> userManager)
        {
            _unitofwork = unitofwork;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index()
        {
            
            return View();
        }


        public async Task<IActionResult> Create(string UserId)
        {
            var getUser = await _userManager.FindByIdAsync(UserId);
            if (getUser == null)
            {
                return NotFound();
            }
            var sectionId = getUser.SectionId;
            var sectionName = await _unitofwork.Sections.GetNamebyIdAsync(sectionId);

            SubjectsMarksViewModel smvm = new SubjectsMarksViewModel()
            {
                SectionName = sectionName,
            };

            ViewBag.Subjects = new SelectList(await _unitofwork.Subjects.GetAllBySectionNameAsync(sectionName), "SubjectId", "SubjectNameArabic");

            return View(smvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectsMarksViewModel SMVM , string UserId)
        {
          
            if (!ModelState.IsValid)
            {
                var getUser = await _userManager.FindByIdAsync(UserId);
                if (getUser == null)
                {
                    return NotFound();
                }

                var sectionId = getUser.SectionId;
                var sectionName = await _unitofwork.Sections.GetNamebyIdAsync(sectionId);
                ViewBag.Subjects = new SelectList(await _unitofwork.Subjects.GetAllBySectionNameAsync(sectionName), "SubjectId", "SubjectNameArabic");
                //TempData["ErrorMessage"] = "لم تتم عمليه الإضافه";
                return View();
            }

             await _unitofwork.SubjectMarks.CreateAsync(SMVM);
            _unitofwork.SaveChanges();

            return View();
        }
    }
}
