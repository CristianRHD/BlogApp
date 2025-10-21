using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using BlogApp.Data;
using BlogApp.Entidades;

namespace BlogApp.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
       
        private const int MaxFileSize = 5242880;

        public FileService(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        public async Task<MediaFile> SaveFileAsync(IBrowserFile file)
        {
           
            if (file.Size > MaxFileSize)
                throw new InvalidOperationException("El archivo excede el límite de 5MB.");

            var uploadsPath = Path.Combine(_env.WebRootPath, "images", "uploads");
            Directory.CreateDirectory(uploadsPath);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.Name);
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream(MaxFileSize).CopyToAsync(stream);
            }

            var media = new MediaFile
            {
                FileName = file.Name,
                FilePath = $"/images/uploads/{uniqueFileName}", 
                ContentType = file.ContentType,
                UploadDate = DateTime.UtcNow
            };

            _context.MediaFiles.Add(media);
            await _context.SaveChangesAsync();

            return media;
        }
    }
}