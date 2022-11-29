using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Manufactures.Application.AzureUtility.AzureDocument;

namespace Manufactures.Application.AzureUtility
{
    public interface IAzureDocument
    {
        Task<string> UploadMultipleFile(string moduleName, Guid id, DateTime _createdUtc, List<string> filesBase64, List<string> filesNameString, List<string> beforeFilePaths);
        Task<DocumentFileResult> DownloadDocument(string documentPath);
        Task RemoveMultipleFile(string moduleName, string filesPath);
        Task<string> DownloadFile(string moduleName, string filePath);
        Task<List<string>> DownloadMultipleFiles(string moduleName, List<string> filesPath);
    }
}
