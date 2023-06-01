using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Application.Dtos;
using DevBlog.Application.Response;
using DevBlog.Domain;
using DevBlog.Domain.Entities;

namespace DevBlog.Application.Handlers.Query
{
    public class GetPost : IRequest<ApiResponse<PostDto>>
	{
		public class Query : IRequest<ApiResponse<PostDto>>
		{
			public string Slug { get; }

			public Query(string slug)
			{
				Slug = slug;
			}
		}

		public class Handler : IRequestHandler<Query, ApiResponse<PostDto>>
		{
			private readonly Context _context;
			private readonly IMapper _mapper;

			public Handler(Context context, IMapper mapper)
			{
				_context = context;
				_mapper = mapper;
			}

			public async Task<ApiResponse<PostDto>> Handle(Query query, CancellationToken cancellationToken)
			{
				var post = await _context.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Slug.ToLower() == query.Slug.ToLower(), cancellationToken);

				if (post == null) return ApiResponse<PostDto>.NotFound($"Unable to find post with slug '{query.Slug}'.");

				var dto = _mapper.Map<Post, PostDto>(post);

				return ApiResponse<PostDto>.Ok(dto);
			}
		}
	}
}


