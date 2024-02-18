namespace Condo.Api;

public class PostService : IPostService
{
    public async Task<IEnumerable<Post>> GetPosts(string condominiumId)
    {
        return new Post[]
        {
            new Post
            {
                Author = new Resident
                {
                    Condominium = new Condominium
                    {
                        Id = "1"
                    }
                }
            }
        };
    }
}