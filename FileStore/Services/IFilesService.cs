namespace FileStore.Services
{
    public interface IFilesService<TResponse>
    {
        Task<ResponseMessage> SaveFiles(TResponse response);

        Task<byte[]> GetFiles(GetFiles files);
        Task<byte[]> GetDigitalDocument(GetFiles file);
        Task<List<byte[]>> GetFilesOnlineDoc(ImageUrlRequest imageUrl);
        Task<List<string>> GetFolderContent(string folderName);
        Task<List<string>> GetDigitalDocumentsList(string folderName);
        Task<ResponseMessage> SaveDigitalDocument(Files file);
        string DeleteDigitalDocumentAsync(string filePath);
        string DeleteAbsFile(string filePath);
        IActionResult DownloadFolderContent(string folderName);

    }
}
