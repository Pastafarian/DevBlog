using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Application.Dtos;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Handlers.Query;
using Microsoft.AspNetCore.Authorization;
using DevBlog.Application.Response;
using DevBlog.Application.Requests;

namespace DevBlog.Api.Controllers
{
	[Route("[controller]")]
	public class PostController : BaseController
	{
		private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

		[HttpGet]
		public async Task<ActionResult<List<PostDto>>> Index()
		{
			var response = await _mediator.Send(new GetPosts.Query(SiteUser), CancellationToken.None);
			return ToActionResult(response);
		}

        [Route("{slug}")]
		[HttpGet]
		public async Task<ActionResult<PostDto>> GetPost(string slug)
        {
            var response = await _mediator.Send(new GetPost.Query(slug), CancellationToken.None);
			return ToActionResult(response);
		}

		[HttpPut]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<PostDto>> UpdatePost([FromBody] UpdatePostRequestDto post)
		{
			var response = await _mediator.Send(new UpdatePost.Command(post), CancellationToken.None);
			return ToActionResult(response);
		}

		[HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto postDto)
		{
			var response = await _mediator.Send(new CreatePost.Command(postDto), CancellationToken.None);
			return ToActionResult(response);
		}

		[Route("{postId}")]
		[HttpDelete]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ApiResponse>> DeletePost(int postId)
		{
			var response = await _mediator.Send(new DeletePost.Command(postId), CancellationToken.None);
            return ToActionResult(response);
        }
    }
}