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
			private readonly Context context;
			private readonly IMapper mapper;

			public Handler(Context context, IMapper mapper)
			{
				this.context = context;
				this.mapper = mapper;
			}

			public async Task<ApiResponse<PostDto>> Handle(Query query, CancellationToken cancellationToken)
			{
				var post = await context.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Slug.ToLower() == query.Slug.ToLower(), cancellationToken);

				if (post == null) return ApiResponse<PostDto>.NotFound();

				var dto = mapper.Map<Post, PostDto>(post);

				return ApiResponse<PostDto>.Ok(dto);
			}
		}
	}

}


