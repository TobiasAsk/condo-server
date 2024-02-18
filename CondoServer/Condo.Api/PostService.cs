using Microsoft.Azure.Cosmos;

namespace Condo.Api;

public class PostService : IPostService
{
    private readonly CosmosClient _cosmosClient;

    private static readonly QueryDefinition GetAllCondoPostsQuery =
        new("SELECT * FROM Posts p WHERE p.author.condominium.id = @condominiumId");

    public PostService(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public async Task<IEnumerable<Post>> GetPosts(string condominiumId)
    {
        var query = GetAllCondoPostsQuery.WithParameter("@condominiumId", condominiumId);
        var container = _cosmosClient.GetDatabase("Borettslag").GetContainer("Posts");
        using var feedIterator = container.GetItemQueryIterator<Post>(queryDefinition: query);
        var posts = new List<Post>();

        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync();
            posts.AddRange(response);
        }

        return posts;
    }
}