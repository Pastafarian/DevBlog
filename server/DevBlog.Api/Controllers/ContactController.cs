using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Requests;
using DevBlog.Application.Response;
using DevBlog.Application.Settings;

namespace DevBlog.Api.Controllers
{
	[Route("[controller]")]
	public class ContactController : BaseController
	{
		private readonly IMediator mediator;
        private readonly AppSettings appSettings;

        public ContactController(IMediator mediator, AppSettings appSettings)
        {
            this.mediator = mediator;
            this.appSettings = appSettings;
        }

        [HttpPost]
        public async Task<ActionResult<EntityCreatedResponseDto>> Post([FromBody] SubmitContactRequestDto contact)
        {
            var response = await mediator.Send(new SubmitContact.Command(contact));
            return ToActionResult(response);
        }
    }
}
