using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Condo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CondominiumsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IAuthorizationService _authorizationService;
    
    public CondominiumsController(IPostService postService, IAuthorizationService authorizationService)
    {
        _postService = postService;
        _authorizationService = authorizationService;
    }

    [Route("{condominiumId}/posts")]
    [HttpGet]
    public async Task<IActionResult> GetPosts([FromRoute] string condominiumId)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            user: HttpContext.User,
            resource: condominiumId,
            policyName: "ReadCondominiumPolicy");

        if (authorizationResult.Succeeded)
        {
            return Ok(await _postService.GetPosts(condominiumId));
        }

        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            return new ForbidResult();
        }

        return new ChallengeResult();
    }

    [Route("{condominiumId}/posts")]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromRoute] string condominiumId, [FromBody] Post post)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            user: HttpContext.User,
            resource: condominiumId,
            policyName: "ReadCondominiumPolicy");

        if (authorizationResult.Succeeded)
        {
            await _postService.CreatePost(post);
            var url = $"{HttpContext.Request.GetEncodedUrl()}/{post.Id}";
            return Created(url, null);
        }
        
        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            return new ForbidResult();
        }

        return new ChallengeResult();
    }
}