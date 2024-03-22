using NuGet.ProjectModel;

namespace AuctionAPI_10_Api.Services;

public class FileService
{
    public static async Task<string?> SaveImageAsync(IFormFile image, IWebHostEnvironment webHostEnvironment)
    {
        if (image.Length <= 0) return null;

        if (!IsImageFile(image))
        {
            throw new FileFormatException("File is not an image");
        }

        const string directory = "uploads/images";
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

    private static bool IsImageFile(IFormFile file)
    {
        string mimeType = file.ContentType.ToLower();
        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        string[] allowedMimeTypes = ["image/jpeg", "image/png", "image/gif"];
        string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

        return allowedMimeTypes.Contains(mimeType) && allowedExtensions.Contains(fileExtension);
    }
}