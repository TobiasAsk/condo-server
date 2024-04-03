namespace Condo.Api;

public class Post
{
    public Resident? Author { get; set; }
    
    public string? Text { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public string? Id { get; set; }
}