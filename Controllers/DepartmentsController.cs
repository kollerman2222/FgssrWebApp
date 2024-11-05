using fgssr.Models;
using fgssr.UnitOFWork;
using fgssr.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fgssr.Controllers
{

	[Authorize(Roles = "Admin")]
	public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public DepartmentsController(IUnitOfWork unitofwork) 
        {
            _unitofwork = unitofwork;
        }

        public async Task<IActionResult> Index()
        {

            var departments = await _unitofwork.DiplomasDepartments.GetAllAsync();
            var ddvm = departments.Select(dep => new DiplomasDepartmentsViewModel
            {
                DepId = dep.DepartmentId,
                DepartmentName = dep.DepartmentName,
                Description = dep.Description,
                Sections = dep.Sections
            }).ToList();
                  
            return View(ddvm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(id);
            if(department == null)
            {
                return NotFound();
            }
            var ddvm = new DiplomasDepartmentsViewModel()
            {
                DepId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };

            return View(ddvm);
        }


        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(id);
            if(department == null)
            {
                return NotFound();
            }
            _unitofwork.DiplomasDepartments.Delete(department);
            _unitofwork.SaveChanges();
            string SuccessMessage = "تمت عمليه الحذف بنجاح";

            return Json(new { deleteMessage = SuccessMessage });
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiplomasDepartmentsViewModel DDVM)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "لم تتم عمليه الإضافه";

                return View();
            }

           await _unitofwork.DiplomasDepartments.CreateAsync(DDVM);
            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه الإضافه بنجاح";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var ddvm = new DiplomasDepartmentsViewModel()
            {
                DepId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };

            return View(ddvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DiplomasDepartmentsViewModel ddvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(ddvm.DepId);
            if (department == null)
            {
                return NotFound();
            }

            department.DepartmentName = ddvm.DepartmentName;
            department.Description = ddvm.Description;
            _unitofwork.SaveChanges();

            TempData["SuccessMessage"] = "تمت عمليه التعديل بنجاح";

            return RedirectToAction("Index");
        }

      
    }
}
