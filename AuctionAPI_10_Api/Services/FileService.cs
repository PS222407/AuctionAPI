namespace AuctionAPI_10_Api.Services;

public class FileService
{
    public async Task<string?> SaveImageAsync(IFormFile image, IWebHostEnvironment webHostEnvironment)
    {
        if (image.Length > 0)
        {
            string directory = "uploads/images";
            string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            string webRootPath = webHostEnvironment.WebRootPath;
            string imageDirectoryPath = Path.Combine(webRootPath, directory);
            string imagePath = Path.Combine(webRootPath, directory, fileName);

            if (!Directory.Exists(imageDirectoryPath))
            {
                Directory.CreateDirectory(imageDirectoryPath);
            }

            await using FileStream fileStream = new(imagePath, FileMode.Create);
            await image.CopyToAsync(fileStream);

            return "/" + directory + "/" + fileName;
        }

        return null;
    }
}