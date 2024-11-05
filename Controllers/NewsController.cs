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

	public class NewsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public NewsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IActionResult> Index()
        {

            var news = await _unitofwork.News.GetAllAsync();
            var nvm = news.Select(nn => new NewsViewModel
            {
                NewsDescription=nn.NewsDescription,
                NewsTitle= nn.NewsTitle,
                NId=nn.NewsId,
                NewsDate=nn.NewsDate,
                
            }).ToList();

            return View(nvm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var news = await _unitofwork.News.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            var nvm = new NewsViewModel()
            {
                NewsDate =news.NewsDate,
                NewsDescription=news.NewsDescription,
                NewsTitle=news.NewsTitle,
                NId=news.NewsId
            };

            return View(nvm);
        }


        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var news = await _unitofwork.News.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            _unitofwork.News.Delete(news);
            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه الحذف بنجاح";

            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {
            ViewBag.currentDate = DateOnly.FromDateTime(DateTime.Today);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsViewModel NVM)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "لم تتم عمليه الإضافه";
                return View();
            }

            await _unitofwork.News.CreateAsync(NVM);
            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه الإضافه بنجاح";


            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var news = await _unitofwork.News.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            var nvm = new NewsViewModel()
            {
                NewsDate= news.NewsDate,
                NewsTitle= news.NewsTitle,
                NewsDescription= news.NewsDescription,
                NId=news.NewsId
            };

            return View(nvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsViewModel nvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            var news = await _unitofwork.News.GetByIdAsync(nvm.NId);
            if (news == null)
            {
                return NotFound();
            }
            news.NewsDescription=nvm.NewsDescription;
            news.NewsTitle= nvm.NewsTitle;
            news.NewsDate = nvm.NewsDate;

            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه التعديل بنجاح";


            return RedirectToAction("Index");
        }
    }
}
