using MediatR;
using DevBlog.Application.Response;

namespace DevBlog.Application.Dtos
{
	public class ClaimDto : IRequest<ApiResponse>
	{
		public string Type { get; }
		public string Value { get; }

		public ClaimDto(string type, string value)
		{
			Type = type;
			Value = value;
		}
	}
}