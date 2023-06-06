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
using Microsoft.Extensions.Logging;

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
			private readonly Context _context;
            private readonly ILogger<Handler> _logger;

            public Handler(Context context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

			public async Task<ApiResponse<List<PostDto>>> Handle(Query query, CancellationToken cancellationToken)
			{
				var posts = _context.Posts.AsNoTracking();
                _logger.LogInformation("GetPosts called: SiteUser - IsAdmin: {IsAdmin}, IsAuthenticated: {IsAuthenticated}", query.SiteUser.IsAdmin, query.SiteUser.IsAuthenticated);


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
