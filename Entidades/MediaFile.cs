public class MediaFile
{
    public int Id { get; set; }
    public required string FileName { get; set; } 
    public required string FilePath { get; set; } 
    public required string ContentType { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; } 
}