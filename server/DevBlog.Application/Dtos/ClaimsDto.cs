using System.Collections.Generic;

namespace DevBlog.Application.Dtos
{
	public class ClaimsDto
	{
		public List<ClaimDto> Claims { get; }

		public ClaimsDto(List<ClaimDto> claims)
		{
			Claims = claims;
		}
	}
}