namespace FileStore.Models;

public class ResponseMessage
{
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}
