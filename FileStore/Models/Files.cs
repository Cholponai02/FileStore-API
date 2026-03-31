namespace FileStore.Models;

public class Files
{
    public string PathFile { get; set; }
    public byte[] FileData { get; set; } = null!;
}
