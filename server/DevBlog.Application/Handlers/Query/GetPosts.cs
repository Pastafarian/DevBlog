using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Application.Auth;
using DevBlog.Application.Dtos;
using DevBlog.Application.Extensions;
using DevBlog.Application.Response;
using DevBlog.Domain;

namespace DevBlog.Application.Handlers.Query
{
	public class GetPosts : IRequest<ApiResponse<List<PostDto>>>
	{
		public class Query : IRequest<ApiResponse<List<PostDto>>>
		{
			public SiteUser SiteUser { get; set; }

			public Query(SiteUser siteUser)
			{
				SiteUser = siteUser;
			}
		}

		/// <summary>
		/// https://www.thereformedprogrammer.net/building-efficient-database-queries-using-entity-framework-core-and-automapper/
		/// </summary>
		public class Handler : IRequestHandler<Query, ApiResponse<List<PostDto>>>
		{
			private readonly Context context;

			public Handler(Context context)
			{
				this.context = context;
			}

			public async Task<ApiResponse<List<PostDto>>> Handle(Query query, CancellationToken cancellationToken)
			{
				var posts = context.Posts.AsNoTracking();


				if (!query.SiteUser.IsAdmin) posts = posts.Where(x => x.PublishDate != null && x.PublishDate < DateTime.UtcNow);

				var postSummaries = await posts.OrderByDescending(x => x.PublishDate)
						.Select(x => new PostDto
						{
							Id = x.Id,
							Body = x.Body,
							PublishDate = x.PublishDate.Value,
							HeaderImage = x.HeaderImage,
							ReadMinutes = x.ReadMinutes,
							IsPublished = x.PublishDate < DateTime.UtcNow,
							Title = x.Title,
							Slug = x.Slug
						})
						.ToListAsync(cancellationToken);

				postSummaries.ForEach(x =>
				{
					x.Body = x.Body.HtmlToPlainText().TruncateText(220);
				});
				
				return ApiResponse<List<PostDto>>.Ok(postSummaries);
			}
		}
	}
}
