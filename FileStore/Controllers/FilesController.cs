using DocumentFormat.OpenXml.Packaging;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;
using FileStore.Helpers;

namespace FileStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFilesService<Files> _fileService;
    private readonly ILogger _logger;
    public FilesController(IFilesService<Files> fileService, ILogger<FilesController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    ReplacingWordsHelper replacingWordsHelper = new ReplacingWordsHelper();

    [HttpPost("SaveFile")]
    public async Task<IActionResult> SavesFiles(Files document)
    {
        if (document == null)
            return BadRequest("The document was not uploaded!");

        try
        {
            var response = await _fileService.SaveFiles(document);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = ex.Message
            });
        }
    }

    [HttpPost("SaveDigitalDocument")]
    public async Task<IActionResult> SaveDigitalDocument(Files document)
    {
        if (document == null)
            return BadRequest("The document cannot be empty");

        try
        {
            var response = await _fileService.SaveDigitalDocument(document);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest(response);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = ex.Message
            });
        }
    }

    [HttpPost("GetFiles")]
    public async Task<IActionResult> GetFiles(GetFiles data)
    {
        SQLService sql = new SQLService();
        var listOfFiles =  await _fileService.GetFiles(data);
        if (listOfFiles == null)
            return NotFound($"File {data.FilePath}not found in the specified path");

        return Ok(listOfFiles);
    }
    [HttpPost("GetDigitalDocument")]
    public async Task<IActionResult> GetDigitalDocument(GetFiles data)
    {
        var listOfFiles = await _fileService.GetDigitalDocument(data);
        if (listOfFiles == null)
            return NotFound($"File {data.FilePath}not found in the specified path");

        return Ok(listOfFiles);
    }
    [HttpPost("GetFilesOnlineDoc")]
    public async Task<IActionResult> GetFilesOnlineDoc([FromBody] ImageUrlRequest imageUrl)
    {
        var listOfFiles = await _fileService.GetFilesOnlineDoc(imageUrl);

        if (listOfFiles == null)
            return NotFound("No files were found at this path to display.");

        return Ok(listOfFiles);
    }

    [HttpGet("GetFolderContent")]
    public async Task<List<string>> GetFolderContent(string folderPath)
    {
        return await _fileService.GetFolderContent(folderPath);
    }

    [HttpGet("CopyFolderContent")]
    public IActionResult DownloadFolderContent(string folderPath)
    {
        return _fileService.DownloadFolderContent(folderPath);
    }

    [HttpGet("GetDigitalDocumentsList")]
    public async Task<List<string>> GetDigitalDocumentsList(string folderPath)
    {
        return await _fileService.GetDigitalDocumentsList(folderPath);
    }

    [HttpGet("DeleteDigitalDocument")]
    public string DeleteDigitalDocument(string filePath)
        => _fileService.DeleteDigitalDocumentAsync(filePath);

    [HttpGet("DeleteAbsFile")]
    public string DeleteAbsFile(string filePath)
        => _fileService.DeleteAbsFile(filePath);

    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok("Check work");
    }

    [HttpGet("GetSignature")]
    public string GetSignature(string employeeNumber, string EDS, string pin, string documentId)
    {
        if (!string.IsNullOrWhiteSpace(employeeNumber) && !string.IsNullOrWhiteSpace(EDS)
            && !string.IsNullOrWhiteSpace(pin) && !string.IsNullOrWhiteSpace(documentId))
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(employeeNumber.Trim() + EDS.Trim() + pin.Trim() + documentId.Trim());
                var hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder hashSB = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashSB.Append(hashBytes[i].ToString("x").PadLeft(2, '0').ToUpper());
                }

                return hashSB.ToString();
            }
        }
        return "";
    }

    [HttpPost("SetWordsToDocument")]
    public int SetWordsToDocument([FromBody] ReplaceWords replaceWords)
    {
        var rootDirectory = "D:\\computer_files";
        string wordText = "";
        string fileFullPath = Path.Combine(rootDirectory, replaceWords.FilePath);

        try
        {
            using (WordprocessingDocument wordprocess = WordprocessingDocument.Open(fileFullPath, true))
            {
                using (StreamReader streamReader = new StreamReader(wordprocess.MainDocumentPart.GetStream()))
                {
                    wordText = streamReader.ReadToEnd();
                }

                foreach (var oldAndNewValue in replaceWords.Words)
                {
                    replacingWordsHelper.ReplaceWords(oldAndNewValue.Key, oldAndNewValue.Value, wordprocess);
                }

                using (StreamWriter streamWriter = new StreamWriter(wordprocess.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(wordText);
                }
            }

            return 1;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    
}
