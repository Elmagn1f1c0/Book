using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Books.Data.Services
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                return null;
            var cloudinaryAccount = new Account(
                    _configuration["Cloudinary:CloudName"],
                    _configuration["Cloudinary:ApiKey"],
                    _configuration["Cloudinary:ApiSecret"]
                );
            var cloudinary = new Cloudinary(cloudinaryAccount);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folderPath 
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

  
            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
