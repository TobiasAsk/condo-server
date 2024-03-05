using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condo.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IPostService _postService;

    public PostsController(ILogger<PostsController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    [HttpGet(Name = "GetPosts")]
    public async Task<IEnumerable<Post>> Get()
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "sub").Value;
        return await _postService.GetPosts(condominiumId: "1");
    }
}