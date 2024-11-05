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
using Microsoft.AspNetCore.Authorization;

namespace fgssr.Controllers
{
	[Authorize(Roles = "Admin")]

	public class StaffsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public StaffsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IActionResult> Index()
        {

            var staffs = await _unitofwork.Staffs.GetAllAsync();
            var svm = staffs.Select(staff => new StaffViewModel
            {
                StID=staff.StaffId,
                Biograpghy = staff.Biograpghy,
                StaffName = staff.StaffName,
                StaffPosition = staff.StaffPosition,
                StaffImageName = staff.StaffImage,
                Email=staff.Email,
            }).ToList();

            return View(svm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            var svm = new StaffViewModel()
            {
                StID =staff.StaffId,
                Biograpghy = staff.Biograpghy,
                StaffName = staff.StaffName,
                StaffPosition = staff.StaffPosition,
                StaffImageName = staff.StaffImage,
                Email = staff.Email,
            };
            return View(svm);
        }


        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            _unitofwork.Staffs.Delete(staff);
            var imageDelete = Path.Combine(_unitofwork.Staffs.uploadFolderPublic, staff.StaffImage);
            System.IO.File.Delete(imageDelete);
            _unitofwork.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffViewModel SVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            await _unitofwork.Staffs.CreateAsync(SVM);
            _unitofwork.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var svm = new StaffViewModel()
            {

                StID = staff.StaffId,
                Biograpghy = staff.Biograpghy,
                StaffName = staff.StaffName,
                StaffPosition = staff.StaffPosition,
                StaffImageName = staff.StaffImage,
                Email = staff.Email,
            };

            return View(svm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffViewModel SVM)
        {
            ModelState.Remove(nameof(SVM.CreateStaffImage));

            if (!ModelState.IsValid)
            {
                return View(SVM);
            }
            var staff = await _unitofwork.Staffs.GetByIdAsync(SVM.StID);
            if (staff == null)
            {
                return NotFound();
            }

            var oldImage = staff.StaffImage;

            staff.StaffId = SVM.StID;
            staff.StaffName=SVM.StaffName;
            staff.StaffPosition=SVM.StaffPosition;
            staff.Email=SVM.Email;
            staff.Biograpghy=SVM.Biograpghy;
            staff.StaffImage = SVM.StaffImageName;
         
            if (SVM.EditStaffImage != null)
            {
                staff.StaffImage = await _unitofwork.Staffs.SaveNewImage(SVM.EditStaffImage);
                var oldImageDelete = Path.Combine(_unitofwork.Staffs.uploadFolderPublic, oldImage);
                System.IO.File.Delete(oldImageDelete);
            }

            _unitofwork.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
