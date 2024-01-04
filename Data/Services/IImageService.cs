namespace Books.Data.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderPath);
    }
}
