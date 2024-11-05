using fgssr.UnitOFWork;
using fgssr.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace fgssr.Controllers
{
	[Authorize(Roles = "Admin")]

	public class EventsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public EventsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IActionResult> Index()
        {

            var events = await _unitofwork.Events.GetAllAsync();
            var evm = events.Select(eve => new EventsViewModel
            {
                EveId = eve.EventId,
                EventTitle = eve.EventTitle,
                EventDescription = eve.EventDescription,
                DateDay = eve.DateDay,
                EventLocation = eve.EventLocation,
                DateMonth = eve.DateMonth,
                EventImageName=eve.EventImage,
                Time=eve.Time,

            }).ToList();

            return View(evm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var events = await _unitofwork.Events.GetByIdAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            var evm = new EventsViewModel()
            {
                EveId = events.EventId,
                EventTitle = events.EventTitle,
                EventDescription = events.EventDescription,
                EventLocation = events.EventLocation,
                DateDay = events.DateDay,
                DateMonth = events.DateMonth,
                EventImageName = events.EventImage,
                Time = events.Time,
            };
            return View(evm);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var events = await _unitofwork.Events.GetByIdAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            _unitofwork.Events.Delete(events);
            var imageDelete = Path.Combine(_unitofwork.Events.uploadFolderPublic, events.EventImage);
            System.IO.File.Delete(imageDelete);
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
        public async Task<IActionResult> Create(EventsViewModel EVM)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "لم تتم عمليه الإضافه";

                return View();
            }

            await _unitofwork.Events.CreateAsync(EVM);
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
            var events = await _unitofwork.Events.GetByIdAsync(id);
            if (events == null)
            {
                return NotFound();
            }

            var evm = new EventsViewModel()
            {

                EveId = events.EventId,
                EventTitle = events.EventTitle,
                EventDescription = events.EventDescription,
                DateDay = events.DateDay,
                EventLocation = events.EventLocation,
                DateMonth = events.DateMonth,
                EventImageName = events.EventImage,
                Time = events.Time,
            };

            return View(evm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventsViewModel EVM)
        {
            ModelState.Remove(nameof(EVM.CreateEventImage));

            if (!ModelState.IsValid)
            {
                return View(EVM);
            }
            var events = await _unitofwork.Events.GetByIdAsync(EVM.EveId);
            if (events == null)
            {
                return NotFound();
            }

            var oldImage = events.EventImage;

            events.EventId = EVM.EveId;
            events.EventTitle = EVM.EventTitle;
            events.EventDescription = EVM.EventDescription;
            events.DateDay = EVM.DateDay;
            events.EventLocation = EVM.EventLocation;
            events.DateMonth = EVM.DateMonth;
            events.Time = EVM.Time;
            events.EventImage = EVM.EventImageName; 
            if(EVM.EditEventImage != null)
            {
                events.EventImage = await _unitofwork.Events.SaveNewImage(EVM.EditEventImage);
                var oldImageDelete = Path.Combine(_unitofwork.Events.uploadFolderPublic, oldImage);
                System.IO.File.Delete(oldImageDelete);
            }
                       
            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه التعديل بنجاح";

            return RedirectToAction("Index");
        }

    }
}
