using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condo.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CondominiumsController : ControllerBase
{
    private readonly IPostService _postService;

    public CondominiumsController(IPostService postService)
    {
        _postService = postService;
    }

    [Route("{condominiumId}/posts")]
    public async Task<IEnumerable<Post>> GetPosts([FromRoute] string condominiumId)
    {
        return await _postService.GetPosts(condominiumId);
    }
}