using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Application.Dtos;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Handlers.Query;

namespace DevBlog.Api.Controllers
{
	[Route("[controller]")]
	public class PostController : BaseController
	{
		private readonly IMediator mediator;

		public PostController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<PostDto>>> Index()
		{
			var response = await mediator.Send(new GetPosts.Query(SiteUser), CancellationToken.None);
			return ToActionResult(response);
		}

        [Route("{slug}")]
		[HttpGet]
		public async Task<ActionResult<PostDto>> GetPost(string slug)
		{
			var response = await mediator.Send(new GetPost.Query(slug), CancellationToken.None);
			return ToActionResult(response);
		}

		[HttpPut]
		public async Task<ActionResult<PostDto>> UpdatePost([FromBody] PostDto post)
		{
			var response = await mediator.Send(new UpdatePost.Command(post), CancellationToken.None);
			return ToActionResult(response);
		}

		[HttpPost]
		public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto postDto)
		{
			var response = await mediator.Send(new CreatePost.Command(postDto), CancellationToken.None);
			return ToActionResult(response);
		}

		[Route("{postId}")]
		[HttpDelete]
		public async Task<ActionResult> DeletePost(int postId)
		{
			await mediator.Send(new DeletePost.Command(postId), CancellationToken.None);
			return Ok();
		}
	}
}