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

    public async Task<Resident> GetResident(string userId)
    {
        var container = _cosmosClient.GetDatabase("Borettslag").GetContainer("Residents");
        var resident = await container.ReadItemAsync<Resident>(
            id: userId,
            partitionKey: new PartitionKey(userId));
        return resident.Resource;
    }

    public async Task<Post> CreatePost(Post post)
    {
        var container = _cosmosClient.GetDatabase("Borettslag").GetContainer("Posts");
        var itemResponse = await container.CreateItemAsync<Post>(post, requestOptions: new ItemRequestOptions
        {
            
        });
        return itemResponse.Resource;
    }
}