﻿using fgssr.Data;
using fgssr.IRepository;
using fgssr.Models;
using fgssr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace fgssr.Repository
{
    public class StaffRepository:IStaffRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;

        public StaffRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/staff";
        }

        public string uploadFolderPublic { get { return _uploadFolderPath; } }

        public async Task CreateAsync(StaffViewModel SVM)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(SVM.CreateStaffImage.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await SVM.CreateStaffImage.CopyToAsync(stream);

            Staff staff = new()
            {
                StaffName = SVM.StaffName,
                StaffPosition = SVM.StaffPosition,
                Biograpghy = SVM.Biograpghy,
                Email = SVM.Email,
                StaffImage = imageName,                         
            };
            await _context.Staff.AddAsync(staff);

        }

        public void Delete(Staff staff)
        {

            _context.Staff.Remove(staff);
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            var staff = await _context.Staff.ToListAsync();
            return staff;
        }

        public async Task<Staff?> GetByIdAsync(int? id)
        {
            var staff = await _context.Staff.FindAsync(id);
            return staff;
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
