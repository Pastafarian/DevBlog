using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Application.Dtos;
using DevBlog.Application.Response;
using DevBlog.Application.Services;
using DevBlog.Domain;

namespace DevBlog.Application.Handlers.Command
{
	public class UpdatePost
	{
		public class Command : IRequest<ApiResponse<PostDto>>
		{
			public PostDto Post { get; }

			public Command(PostDto post)
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
				var post = context.Posts.SingleOrDefault(x => x.Id == command.Post.Id);

				if (post == null) return ApiResponse<PostDto>.NotFound($"Unable to find article with id '{command.Post.Id}'");

				if (await context.Posts.AnyAsync(x => x.Slug == command.Post.Slug && x.Id != command.Post.Id, cancellationToken)) return ApiResponse<PostDto>.BadRequest("A post with that slug already exists");

				mapper.Map(command.Post, post);

				if (!string.IsNullOrWhiteSpace(command.Post.HeaderImage))
					post.HeaderImage = await imageStorageService.StoreImage(command.Post.HeaderImage, command.Post.Title, cancellationToken);

				await context.SaveChangesAsync(cancellationToken);

				return ApiResponse<PostDto>.Ok(mapper.Map<PostDto>(post));
			}
		}
	}
}
