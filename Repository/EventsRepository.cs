﻿using fgssr.Data;
using fgssr.IRepository;
using fgssr.Models;
using fgssr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace fgssr.Repository
{
    public class EventsRepository:IEventsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;

        public EventsRepository(ApplicationDbContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/events";
        }

        public string uploadFolderPublic { get { return _uploadFolderPath; } }

        public async Task CreateAsync(EventsViewModel EVM)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(EVM.CreateEventImage.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await EVM.CreateEventImage.CopyToAsync(stream);

            Events eve = new()
            {
                EventTitle = EVM.EventTitle,
                EventDescription = EVM.EventDescription,
                EventLocation = EVM.EventLocation,
                DateDay = EVM.DateDay,
                DateMonth = EVM.DateMonth,
                Time = EVM.Time,
                EventImage=imageName

            };
            await _context.Events.AddAsync(eve);

        }

        public void Delete(Events eve)
        {

            _context.Events.Remove(eve);
        }

        public async Task<IEnumerable<Events>> GetAllAsync()
        {
            var events = await _context.Events.ToListAsync();
            return events;
        }

        public async Task<Events?> GetByIdAsync(int? id)
        {
            var events = await _context.Events.FindAsync(id);
            return events;
        }

        public async Task<string> SaveNewImage(IFormFile image)
        {

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await image.CopyToAsync(stream);

            return imageName;
        }

    }
}
