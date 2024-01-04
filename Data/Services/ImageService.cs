using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Books.Data.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                return null;

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderPath, uniqueFileName);
        }
    }
}