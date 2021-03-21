using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Api.Extensions;
using System.Threading;
using System.Threading.Tasks;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Response;

namespace DevBlog.Api.Controllers
{
	[Route("[controller]")]
	public class ImageController : BaseController
	{
		private readonly IMediator mediator;

		public ImageController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[Route("upload")]
		[HttpPost]
		public async Task<ActionResult<ImageUploadedResponseDto>> Upload(IFormFile formFile)
		{
			var result = await mediator.Send(new UploadImage.Command(formFile.ReadBytes(), formFile.FileName), CancellationToken.None);

			return ToActionResult(result);
		}
	}
}