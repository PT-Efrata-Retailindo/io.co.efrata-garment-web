using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.AzureUtility
{
    public interface IAzureImage
    {
        Task<string> DownloadImage(string moduleName, string imagePath);
        Task<string> UploadImage(string moduleName, Guid id, DateTime _createdUtc, string imageBase64);
        Task<List<string>> DownloadMultipleImages(string moduleName, List<string> imagesPath);
        Task<string> UploadMultipleImage(string moduleName, Guid id, DateTime _createdUtc, List<string> imagesBase64, List<string> beforeImagePaths);
        Task RemoveMultipleImage(string moduleName, string imagesPath);
    }
}
