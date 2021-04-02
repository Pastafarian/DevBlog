using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Application.Dtos;
using DevBlog.Application.Requests;
using DevBlog.Application.Response;
using DevBlog.Application.Services;
using DevBlog.Domain;
using DevBlog.Domain.Entities;

namespace DevBlog.Application.Handlers.Command
{
	public class CreatePost
	{
		public class Command : IRequest<ApiResponse<PostDto>>
		{
			public CreatePostRequest Request { get; }

			public Command(CreatePostRequest request)
			{
				Request = request;
			}
		}

		public class Handler : IRequestHandler<Command, ApiResponse<PostDto>>
		{
			private readonly Context context;
			private readonly IMapper mapper;
			private readonly IImageStorageService imageStorageService;
			
			public Handler(Context context, IMapper mapper, IImageStorageService imageStorageService)
			{
				this.mapper = mapper;
				this.imageStorageService = imageStorageService;
				this.context = context;
			}

			public async Task<ApiResponse<PostDto>> Handle(Command command, CancellationToken cancellationToken)
			{
                if (await context.Posts.AnyAsync(x => x.Slug == command.Request.Slug, cancellationToken)) return ApiResponse<PostDto>.BadRequest("A post with that slug already exists");

                var post = mapper.Map<CreatePostRequest, Post>(command.Request);

				if (!string.IsNullOrWhiteSpace(command.Request.HeaderImage))
					post.HeaderImage = await imageStorageService.StoreImage(command.Request.HeaderImage, command.Request.Title, cancellationToken);

				await context.Posts.AddAsync(post, cancellationToken);

				await context.SaveChangesAsync(cancellationToken);

				return ApiResponse<PostDto>.Ok(mapper.Map<PostDto>(post));
			}
		}
	}
}
