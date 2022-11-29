using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manufactures.Application.AzureUtility
{
    public class AzureImage : AzureStorage, IAzureImage
    {
        private IServiceProvider _serviceProvider;
        public AzureImage(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private string getBase64File(string encoded)
        {
            return encoded.Substring(encoded.IndexOf(',') + 1);
        }

        private string getBase64Type(string encoded)
        {
            Regex regex = new Regex(@"data:([a-zA-Z0-9]+\/[a-zA-Z0-9-.+]+).*,.*");
            string match = regex.Match(encoded).Groups[1].Value;

            return match == null && match == String.Empty ? "image/jpeg" : match;
        }

        public string GetFileNameFromPath(string imagePath)
        {
            string[] filePath = imagePath.Split('/');
            return filePath[filePath.Length - 1];
        }

        public string GenerateFileName(Guid id, DateTime _createdUtc)
        {
            return String.Format("IMG_{0}_{1}", id, TimeStamp.Generate(_createdUtc));
        }

        public string GenerateFileName(Guid id, DateTime _createdUtc, int index)
        {
            return String.Format("IMG_{0}_{1}_{2}", id, index, TimeStamp.Generate(_createdUtc));
        }

        public async Task<string> DownloadImage(string moduleName, string imagePath)
        {
            if (imagePath != null)
            {
                string imageName = this.GetFileNameFromPath(imagePath);
                return await this.DownloadBase64Image(moduleName, imageName);
            }
            return null;
        }

        public async Task<List<string>> DownloadMultipleImages(string moduleName, List<string> imagesPathList)
        {
            if (imagesPathList.Count > 0)
            {
                List<Task<string>> downloadTasks = new List<Task<string>>();
                if (imagesPathList.Count > 0)
                {
                    foreach (string imagePath in imagesPathList)
                    {
                        string fileName = this.GetFileNameFromPath(imagePath);
                        downloadTasks.Add(this.DownloadImage(moduleName, fileName));
                    }
                }
                string[] files = await Task.WhenAll(downloadTasks);
                return files.ToList<string>();
            }
            return null;
        }

        private async Task<string> DownloadBase64Image(string moduleName, string imageName)
        {
            string imageSrc = string.Empty;

            try
            {
                CloudBlobContainer container = this.StorageContainer;
                CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

                CloudBlockBlob blob = dir.GetBlockBlobReference(imageName);
                await blob.FetchAttributesAsync();

                byte[] imageBytes = new byte[blob.Properties.Length];
                await blob.DownloadToByteArrayAsync(imageBytes, 0);

                string imageBase64 = Convert.ToBase64String(imageBytes);
                imageSrc = "data:" + blob.Properties.ContentType + ";base64," + imageBase64;
            }
            catch (Exception ex)
            {
                if (!(ex is StorageException))
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return imageSrc;
        }

        public async Task<string> UploadImage(string moduleName, Guid id, DateTime _createdUtc, string imageBase64)
        {
            string imageName = this.GenerateFileName(id, _createdUtc);
            return await this.UploadBase64Image(moduleName, imageBase64, imageName);
        }

        public async Task<string> UploadMultipleImage(string moduleName, Guid id, DateTime _createdUtc, List<string> imagesBase64, List<string> beforeImagePaths)
        {
            List<Task<string>> uploadTasks = new List<Task<string>>();

            for (int i = 0; i < imagesBase64.Count; i++)
            {
                string imageBase64 = imagesBase64[i];
                string imageName = this.GenerateFileName(id, _createdUtc, i);
                uploadTasks.Add(this.UploadBase64Image(moduleName, imageBase64, imageName));
            }

            string[] afterPaths = await Task.WhenAll(uploadTasks);

            if (beforeImagePaths != null)
            {
                string imagesPath = JsonConvert.SerializeObject(await this.RemoveLeftoverImage(moduleName, beforeImagePaths, afterPaths.ToList<string>()));
                return imagesPath;
            }

            return JsonConvert.SerializeObject(afterPaths.ToList<string>());
        }

        private async Task<List<string>> RemoveLeftoverImage(string moduleName, List<string> before, List<string> after)
        {
            List<string> final = after;
            if (after.Count < before.Count)
            {
                int index = after.Count;
                int count = before.Count - index;
                for (int i = index; i < before.Count; i++)
                {
                    string fileName = this.GetFileNameFromPath(before[i]);
                    await this.RemoveBase64Image(moduleName, fileName);
                }
            }
            return final;
        }

        private async Task<string> UploadBase64Image(string moduleName, string imageBase64, string imageName)
        {
            string path = null;

            try
            {
                string imageFile = this.getBase64File(imageBase64);
                string imageType = this.getBase64Type(imageBase64);
                byte[] imageBytes = Convert.FromBase64String(imageFile);
                if (imageBytes != null)
                {
                    CloudBlobContainer container = this.StorageContainer;
                    CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

                    CloudBlockBlob blob = dir.GetBlockBlobReference(imageName);
                    blob.Properties.ContentType = imageType;
                    await blob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
                    path = "/" + this.StorageContainer.Name + "/" + moduleName + "/" + imageName;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ArgumentNullException) && !(ex is FormatException))
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return path;
        }

        public async Task RemoveImage(string moduleName, string imagePath)
        {
            if (imagePath != null)
            {
                string fileName = this.GetFileNameFromPath(imagePath);
                await this.RemoveBase64Image(moduleName, fileName);
            }
        }

        public async Task RemoveMultipleImage(string moduleName, string imagesPath)
        {
            if (imagesPath != null)
            {
                List<Task> removeTasks = new List<Task>();
                List<string> imagesPathList = JsonConvert.DeserializeObject<List<string>>(imagesPath);
                foreach (string imagePath in imagesPathList)
                {
                    string fileName = this.GetFileNameFromPath(imagePath);
                    removeTasks.Add(this.RemoveBase64Image(moduleName, fileName));
                }
                await Task.WhenAll(removeTasks);
            }
        }

        private async Task RemoveBase64Image(string moduleName, string fileName)
        {
            CloudBlobContainer container = this.StorageContainer;
            CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

            CloudBlockBlob blob = dir.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }

    }
}