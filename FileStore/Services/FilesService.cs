using System.Net;
using Microsoft.AspNetCore.Mvc;
namespace FileStore.Services;
public class FilesService : IFilesService<Files>
{
    public async Task<ResponseMessage> SaveFiles(Files data)
    {
        try
        {
            string directoryPath = $"D:\\computer_files\\{Path.GetDirectoryName(data.PathFile)}";


            string userName = "computer_files@myTest.local";
            string password = "myFuturePass";

            NetworkCredential credential = new NetworkCredential(userName, password);

            HttpClientHandler hander = new HttpClientHandler()
            {
                Credentials = credential
            };
            using (HttpClient httpClient = new HttpClient(hander)) 
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Path.GetFileName(data.PathFile);
                var filePath = Path.Combine(directoryPath, fileName);
                await File.WriteAllBytesAsync(filePath, data.FileData);

                return new ResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Документ успешно загружен",
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = ex.Message,
            };
        }
    }

    public async Task<ResponseMessage> SaveDigitalDocument(Files data)
    {
        try
        {
            string directoryPath = $"E:\\DigitalDocuments\\{Path.GetDirectoryName(data.PathFile)}";

            string userName = "computer_files@myTest.local";
            string password = "myFuturePass";

            NetworkCredential credential = new NetworkCredential(userName, password);

            HttpClientHandler hander = new HttpClientHandler()
            {
                Credentials = credential
            };
            using (HttpClient httpClient = new HttpClient(hander))
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Path.GetFileName(data.PathFile);
                var filePath = Path.Combine(directoryPath, fileName);
                await File.WriteAllBytesAsync(filePath, data.FileData);

                return new ResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Документ успешно загружен",
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = ex.Message,
            };
        }
    }

    public async Task<byte[]> GetDigitalDocument(GetFiles data)
    {
        string filePath = $"E:\\DigitalDocuments\\{data.FilePath}";

        string directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
            return null;
        if (!File.Exists(filePath))
            return null;

        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        return fileBytes;
    }
    public async Task<byte[]> GetFiles(GetFiles data)
    {
        string filePath = $"D:\\computer_files\\{data.FilePath}";

        string directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
            return null;
        if (!File.Exists(filePath))
            return null;
        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        return fileBytes;
    }
    public async Task<List<byte[]>> GetFilesOnlineDoc(ImageUrlRequest imageUrl)
    {
        string filePath = $"D:\\computer_files\\{imageUrl.ClientITIN}\\{imageUrl.MiddlePath}\\{imageUrl.OnlineLoanId}\\PhotoDoc";

        if (!Directory.Exists(filePath))
            return null;

        List<byte[]> listOfFiles = new List<byte[]>();

        await Task.Run(() =>
        {
            string[] fileArray = Directory.GetFiles(filePath, "*.jpg");
            for (int i = 0; i < fileArray.Length; i++)
            {
                listOfFiles.Add(ImageToByteArrayFromFilePath(fileArray[i]));
            }

            fileArray = Directory.GetFiles(filePath, "*.png");
            for (int i = 0; i < fileArray.Length; i++)
            {
                listOfFiles.Add(ImageToByteArrayFromFilePath(fileArray[i]));
            }
            fileArray = Directory.GetFiles(filePath, "*.jpeg");
            for (int i = 0; i < fileArray.Length; i++)
            {
                listOfFiles.Add(ImageToByteArrayFromFilePath(fileArray[i]));
            }
        });


        return listOfFiles;
    }

    public async Task<List<string>> GetFolderContent(string filePath)
    {
        string rootPath = $"D:\\computer_files\\{filePath}";

        if (!Directory.Exists(rootPath))
            return null;

        List<string> listOfFiles = new List<string>();

        await Task.Run(() =>
        {
            string[] fileArray = Directory.GetFiles(rootPath);

            for (int i = 0; i < fileArray.Length; i++)
                listOfFiles.Add(fileArray[i]);
            
        });


        return listOfFiles;
    }

    public IActionResult DownloadFolderContent(string fileName)
    {
        string _basePath = $"D:\\computer_files";
        var filePath = Path.Combine(_basePath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", fileName);
    }

    public async Task<List<string>> GetDigitalDocumentsList(string filePath)
    {
        string rootPath = $"E:\\DigitalDocuments\\{filePath}";

        if (!Directory.Exists(rootPath))
            return null;

        List<string> listOfFiles = new List<string>();

        await Task.Run(() =>
        {
            string[] fileArray = Directory.GetFiles(rootPath);

            for (int i = 0; i < fileArray.Length; i++)
                listOfFiles.Add(fileArray[i]);
        });

        return listOfFiles;
    }

    public string DeleteDigitalDocumentAsync(string filePath)
    {
        string rootPath = Path.Combine("E:\\DigitalDocuments", filePath);

        try
        {
            File.Delete(rootPath);
            return "File deleted";
        }
        catch (Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}";
        }
    }

    public string DeleteAbsFile(string filePath)
    {
        string rootPath = Path.Combine("D:\\computer_files", filePath);
        try
        {
            File.Delete(rootPath);
            return "File deleted";
        }
        catch (Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}";
        }

    }
    static byte[] ImageToByteArrayFromFilePath(string imagefilePath)
    {
        byte[] imageArray = File.ReadAllBytes(imagefilePath);
        return imageArray;
    }

}
