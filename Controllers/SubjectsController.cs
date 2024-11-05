using fgssr.UnitOFWork;
using fgssr.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fgssr.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public SubjectsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IActionResult> Index(string subjectSearch)
        {
           var subjects = await _unitofwork.Subjects.GetAllAsync();
           var filterSubjects = subjects.Where(s => s.Section?.Department?.DepartmentName == subjectSearch);
            var svm = filterSubjects.Select(ss => new SubjectsViewModel
            {
                SubjectNameArabic=ss.SubjectNameArabic,
                SubjectNameEnglish=ss.SubjectNameEnglish,
                ScientificDegree=ss.ScientificDegree,
                SubjectCode=ss.SubjectCode,
                Sections=ss.Section,
                SubjectId=ss.SubjectId
                
            }).ToList();

            return View(svm);
        }


        public async Task<IActionResult> Create()
        {
            var viewModel = new SubjectsViewModel(); // to initialize empty list for section in create view instead of writing it here and using viewModel.casSection

            ViewBag.DepartmentList = new SelectList(await _unitofwork.DiplomasDepartments.GetAllAsync(), "DepartmentId", "DepartmentName");

            //ViewBag.SectionsList = new SelectList(viewModel.CasSections, "SectionId", "SectionName");
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectsViewModel SVM)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.SectionsList = new SelectList(await _unitofwork.Sections.GetAllAsync(), "SectionId", "SectionName");
                TempData["ErrorMessage"] = "لم تتم عمليه الإضافه";

                return View();
            }

            await _unitofwork.Subjects.CreateAsync(SVM);

            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه الإضافه بنجاح";

            return RedirectToAction("Dashboard","Home");
        }



        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var subject = await _unitofwork.Subjects.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            _unitofwork.Subjects.Delete(subject);
            _unitofwork.SaveChanges();
            string SuccessMessage = "تمت عمليه الحذف بنجاح";

            return Json(new { subjectSearch = subject.Section?.Department?.DepartmentName, deleteMessage = SuccessMessage });
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var subject = await _unitofwork.Subjects.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            var section = await _unitofwork.Sections.GetAllAsync();
            var filterSection = section.Where(s=>s.Department?.DepartmentName==subject.Section?.Department?.DepartmentName).ToList();

            ViewBag.SectionsList = new SelectList(filterSection, "SectionId", "SectionName");

            var svm = new SubjectsViewModel()
            {
                SubjectCode=subject.SubjectCode,
                ScientificDegree=subject.ScientificDegree,
                SubjectNameArabic=subject.SubjectNameArabic,
                SubjectNameEnglish=subject.SubjectNameEnglish,
                SectionId=subject.SectionId,
                Sections=subject.Section,
                SubjectId=subject.SubjectId,
            };

            return View(svm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubjectsViewModel SVM)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.SectionsList = new SelectList(await _unitofwork.Sections.GetAllAsync(), "SectionId", "SectionName");

                return View(SVM);
            }
            var subjects = await _unitofwork.Subjects.GetByIdAsync(SVM.SubjectId);
            if (subjects == null)
            {
                return NotFound();
            }


            subjects.SubjectNameEnglish = SVM.SubjectNameEnglish;
            subjects.SubjectNameArabic = SVM.SubjectNameArabic;
            subjects.SectionId = SVM.SectionId;
            subjects.ScientificDegree = SVM.ScientificDegree;
            subjects.SubjectCode = SVM.SubjectCode;
          

            _unitofwork.SaveChanges();

            var param = _unitofwork.Subjects.GetByIdAsync(SVM.SubjectId).Result?.Section?.Department?.DepartmentName;

            TempData["SuccessMessage"] = "تمت عمليه التعديل بنجاح";


            return RedirectToAction("Index",new{ subjectSearch = param});
        }



        public async Task<IActionResult> GetSections(int depId) // select to prevent circulation of navigation between models bec i make models refer back and forth
        {
            var section = await _unitofwork.Sections.GetAllAsync();
            var filterSection = section.Where(s=>s.DepartmentId==depId).Select(s=> new { s.SectionId,s.SectionName}).ToList();
            return Ok(filterSection);

        }



    }
}
