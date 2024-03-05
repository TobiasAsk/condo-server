namespace Condo.Api;

public interface IPostService
{
    public Task<IEnumerable<Post>> GetPosts(string condominiumId);
    public Task<Resident> GetResident(string userId);
}