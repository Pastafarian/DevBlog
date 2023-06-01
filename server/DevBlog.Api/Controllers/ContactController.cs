using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Requests;
using DevBlog.Application.Response;

namespace DevBlog.Api.Controllers
{
	[Route("[controller]")]
	public class ContactController : BaseController
	{
		private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<EntityCreatedResponseDto>> Post([FromBody] SubmitContactRequestDto contact)
        {
            var response = await _mediator.Send(new SubmitContact.Command(contact));
            return ToActionResult(response);
        }
    }
}
