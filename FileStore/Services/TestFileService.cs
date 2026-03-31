using System.Net;

namespace FileStore.Services
{
    public class TestFileService
    {
        public async Task SaveFiles(Files data)
        {
            try
            {
                string directoryPath = $"\\\\192.168.0.0\\computer_files\\{Path.GetDirectoryName(data.PathFile)}";
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

                   
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
