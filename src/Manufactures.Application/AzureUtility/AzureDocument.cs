using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manufactures.Application.AzureUtility
{
    public class AzureDocument : AzureStorage, IAzureDocument
    {
        public AzureDocument(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        private string getBase64File(string encoded)
        {
            return encoded.Substring(encoded.IndexOf(',') + 1);
        }

        private string getBase64Type(string encoded)
        {
            Regex regex = new Regex(@"data:([a-zA-Z0-9]+\/[a-zA-Z0-9-.+]+).*,.*");
            string match = regex.Match(encoded).Groups[1].Value;

            return match == null && match == string.Empty ? "file/octet-stream" : match;
        }

        public string GetFileNameFromPath(string filePath)
        {
            string[] filesPath = filePath.Split('/');
            return filesPath[filesPath.Length - 1];
        }

        public string GenerateFileName(Guid id, DateTime _createdUtc, int index)
        {
            return string.Format("FILE_{0}_{1}_{2}", id, index, TimeStamp.Generate(_createdUtc));
        }

        public async Task<string> DownloadFile(string moduleName, string filePath)
        {
            if (filePath != null)
            {
                string fileName = GetFileNameFromPath(filePath);
                return await DownloadBase64File(moduleName, fileName);
            }
            return null;
        }

        public async Task<List<string>> DownloadMultipleFiles(string moduleName, List<string> filesPathList)
        {
            if (filesPathList.Count > 0)
            {
                List<Task<string>> downloadTasks = new List<Task<string>>();
                if (filesPathList.Count > 0)
                {
                    foreach (string filePath in filesPathList)
                    {
                        string fileName = GetFileNameFromPath(filePath);
                        downloadTasks.Add(DownloadFile(moduleName, fileName));
                    }
                }
                string[] files = await Task.WhenAll(downloadTasks);
                return files.ToList<string>();
            }
            return null;
        }

        private async Task<string> DownloadBase64File(string moduleName, string fileName)
        {
            string fileSrc = string.Empty;

            try
            {
                CloudBlobContainer container = StorageContainer;
                CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

                CloudBlockBlob blob = dir.GetBlockBlobReference(fileName);
                await blob.FetchAttributesAsync();

                byte[] fileBytes = new byte[blob.Properties.Length];
                await blob.DownloadToByteArrayAsync(fileBytes, 0);

                string fileBase64 = Convert.ToBase64String(fileBytes);
                fileSrc = "data:" + blob.Properties.ContentType + ";base64," + fileBase64;
            }
            catch (Exception ex)
            {
                if (!(ex is StorageException))
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return fileSrc;
        }

        public async Task<string> UploadMultipleFile(string moduleName, Guid id, DateTime _createdUtc, List<string> filesBase64, List<string> filesNameString, List<string> beforeFilePaths)
        {
            List<Task<string>> uploadTasks = new List<Task<string>>();

            for (int i = 0; i < filesBase64.Count; i++)
            {
                string fileBase64 = filesBase64[i];
                string fileName = GenerateFileName(id, _createdUtc, i);
                uploadTasks.Add(UploadBase64File(moduleName, fileBase64, fileName));
            }

            string[] afterPaths = await Task.WhenAll(uploadTasks);

            if (beforeFilePaths != null)
            {
                string filesPath = JsonConvert.SerializeObject(await RemoveLeftoverFile(moduleName, beforeFilePaths, afterPaths.ToList()));
                return filesPath;
            }

            return JsonConvert.SerializeObject(afterPaths.ToList());
        }

        private async Task<List<string>> RemoveLeftoverFile(string moduleName, List<string> before, List<string> after)
        {
            List<string> final = after;
            if (after.Count < before.Count)
            {
                int index = after.Count;
                int count = before.Count - index;
                for (int i = index; i < before.Count; i++)
                {
                    string fileName = GetFileNameFromPath(before[i]);
                    await RemoveFile(moduleName, fileName);
                }
            }
            return final;
        }

        private async Task<string> UploadBase64File(string moduleName, string fileBase64, string fileName)
        {
            string path = null;

            try
            {
                string fileFile = getBase64File(fileBase64);
                string fileType = getBase64Type(fileBase64);
                byte[] fileBytes = Convert.FromBase64String(fileFile);
                if (fileBytes != null)
                {
                    CloudBlobContainer container = StorageContainer;
                    CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

                    CloudBlockBlob blob = dir.GetBlockBlobReference(fileName);
                    blob.Properties.ContentType = fileType;
                    await blob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
                    path = "/" + StorageContainer.Name + "/" + moduleName + "/" + fileName;
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

        public async Task<DocumentFileResult> DownloadDocument(string documentPath)
        {
            try
            {
                CloudBlobContainer container = StorageContainer;
                CloudBlockBlob blob = container.GetBlockBlobReference(documentPath);

                if (await blob.ExistsAsync())
                {
                    //await blob.FetchAttributesAsync();
                    //MemoryStream stream = new MemoryStream();
                    //await blob.DownloadToStreamAsync(stream);
                    Stream stream = await blob.OpenReadAsync();

                    DocumentFileResult result = new DocumentFileResult(stream, GetFileNameFromPath(documentPath), blob.Properties.ContentType);

                    return result;
                }
                else
                {
                    throw new Exception("File not found.");
                }
            }
            catch (StorageException ex)
            {
                if (!(ex is StorageException))
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task RemoveMultipleFile(string moduleName, string filesPath)
        {
            if (filesPath != null)
            {
                List<Task> removeTasks = new List<Task>();
                List<string> filesPathList = JsonConvert.DeserializeObject<List<string>>(filesPath);
                foreach (string filePath in filesPathList)
                {
                    string fileName = GetFileNameFromPath(filePath);
                    removeTasks.Add(RemoveFile(moduleName, fileName));
                }
                await Task.WhenAll(removeTasks);
            }
        }

        private async Task RemoveFile(string moduleName, string fileName)
        {
            CloudBlobContainer container = StorageContainer;
            CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

            CloudBlockBlob blob = dir.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public class DocumentFileResult
        {
            public Stream File { get; }
            public string FileName { get; }
            public string FileType { get; }

            public DocumentFileResult(Stream file, string fileName, string fileType)
            {
                File = file;
                FileName = fileName;
                FileType = fileType;
            }
        }

    }
}
