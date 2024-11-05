using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fgssr.Data;
using fgssr.Models;
using fgssr.UnitOFWork;
using fgssr.ViewModels;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Authorization;

namespace fgssr.Controllers
{
	[Authorize(Roles = "Admin")]

	public class SectionsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public SectionsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IActionResult> Index(string departmentSearch)
        {

            var sections = await _unitofwork.Sections.GetAllAsync();
            var filterSections = sections.Where(s => s.Department?.DepartmentName == departmentSearch);
            var dsvm = filterSections.Select(sec => new DiplomasSectionsViewModel
            {
              SectionName = sec.SectionName,
              SecId=sec.SectionId,
              Description = sec.Description,
              isActive=sec.isActive,
              Department=sec.Department,
              SectionImage=sec.SectionImage,
            }).ToList();

            return View(dsvm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var section = await _unitofwork.Sections.GetByIdAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            var dsvm = new DiplomasSectionsViewModel()
            {
                SectionName = section.SectionName,
                SecId = section.SectionId,
                Description = section.Description,
                isActive = section.isActive,
                Department = section.Department,
                SectionImage = section.SectionImage
            };
            return View(dsvm);
        }


        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var section = await _unitofwork.Sections.GetByIdAsync(id);
            var getDepartmentName = section?.Department?.DepartmentName;
            if (section == null)
            {
                return NotFound();
            }
            _unitofwork.Sections.Delete(section);
            var imageDelete = Path.Combine(_unitofwork.Sections.uploadFolderPublic, section.SectionImage);
            System.IO.File.Delete(imageDelete);
            _unitofwork.SaveChanges();
            string SuccessMessage = "تمت عمليه الحذف بنجاح";

            return Json( new { departmentSearch = getDepartmentName , deleteMessage = SuccessMessage });
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.DepartmentList = new SelectList(await _unitofwork.DiplomasDepartments.GetAllAsync(), "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiplomasSectionsViewModel DSVM)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = new SelectList(await _unitofwork.DiplomasDepartments.GetAllAsync(), "DepartmentId", "DepartmentName");
                TempData["ErrorMessage"] = "لم تتم عمليه الإضافه";
                return View();
            }

            await _unitofwork.Sections.CreateAsync(DSVM);
            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه الإضافه بنجاح";
            var getDepartmentName = _unitofwork.DiplomasDepartments.GetByIdAsync(DSVM.DepartmentId);
            return RedirectToAction("Index" , new { departmentSearch = getDepartmentName?.Result?.DepartmentName });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var section = await _unitofwork.Sections.GetByIdAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentList = new SelectList(await _unitofwork.DiplomasDepartments.GetAllAsync(), "DepartmentId", "DepartmentName");

            var dsvm = new DiplomasSectionsViewModel()
            {
                SectionName = section.SectionName,
                SectionImage = section.SectionImage,
                Department = section.Department,
                DepartmentId = section.DepartmentId,
                Description = section.Description,
                SecId=section.SectionId,
                isActive=section.isActive                
            };

            return View(dsvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DiplomasSectionsViewModel DSVM)
        {
            ModelState.Remove(nameof(DSVM.CreateSectionImage));

            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = new SelectList(await _unitofwork.DiplomasDepartments.GetAllAsync(), "DepartmentId", "DepartmentName");

                return View(DSVM);
            }
            var sections = await _unitofwork.Sections.GetByIdAsync(DSVM.SecId);
            if (sections == null)
            {
                return NotFound();
            }

            var oldImage = sections.SectionImage;

            sections.SectionName=DSVM.SectionName;
            sections.Description=DSVM.Description;
            sections.isActive=DSVM.isActive;
            sections.SectionId = DSVM.SecId;
            sections.SectionImage=DSVM.SectionImage;
            sections.DepartmentId=DSVM.DepartmentId;

            if (DSVM.EditSectionImage != null)
            {
                sections.SectionImage = await _unitofwork.Sections.SaveNewImage(DSVM.EditSectionImage);
                var oldImageDelete = Path.Combine(_unitofwork.Sections.uploadFolderPublic, oldImage);
                System.IO.File.Delete(oldImageDelete);
            }

            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه التعديل بنجاح";
            var getDepartmentName = _unitofwork.DiplomasDepartments.GetByIdAsync(DSVM.DepartmentId);

            return RedirectToAction("Index", new { departmentSearch = getDepartmentName?.Result?.DepartmentName });
        }
    }
}
