using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Application.Dtos;
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
			public CreatePostDto Post { get; }

			public Command(CreatePostDto post)
			{
				Post = post;
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
				var post = mapper.Map<CreatePostDto, Post>(command.Post);

				if (await context.Posts.AnyAsync(x => x.Slug == command.Post.Slug, cancellationToken)) return ApiResponse<PostDto>.BadRequest("A post with that slug already exists");

				if (!string.IsNullOrWhiteSpace(command.Post.HeaderImage))
					post.HeaderImage = await imageStorageService.StoreImage(command.Post.HeaderImage, command.Post.Title, cancellationToken);

				await context.Posts.AddAsync(post, cancellationToken);

				await context.SaveChangesAsync(cancellationToken);

				return ApiResponse<PostDto>.Ok(mapper.Map<PostDto>(post));
			}
		}
	}
}
