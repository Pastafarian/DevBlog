using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Application.Dtos;

namespace DevBlog.Api.Controllers
{
	
	[Route("[controller]")]
	public class AuthController : BaseController
	{
		[HttpGet]
		[Route("claims")]
		public IActionResult Claims()
		{
			return Ok(new ClaimsDto(User.Claims.Select(x => new ClaimDto(x.Type, x.Value)).ToList()));
		}
	}
}