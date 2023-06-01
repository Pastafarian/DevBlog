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
			private readonly Context _context;
			private readonly IMapper _mapper;
			private readonly IImageStorageService _imageStorageService;
			
			public Handler(Context context, IMapper mapper, IImageStorageService imageStorageService)
			{
				_mapper = mapper;
				_imageStorageService = imageStorageService;
				_context = context;
			}

			public async Task<ApiResponse<PostDto>> Handle(Command command, CancellationToken cancellationToken)
			{
				var post = _mapper.Map<CreatePostDto, Post>(command.Post);

				if (await _context.Posts.AnyAsync(x => x.Slug == command.Post.Slug, cancellationToken)) return ApiResponse<PostDto>.BadRequest("A post with that slug already exists");

				if (!string.IsNullOrWhiteSpace(command.Post.HeaderImage))
					post.HeaderImage = await _imageStorageService.StoreImage(command.Post.HeaderImage, command.Post.Title, cancellationToken);

				await _context.Posts.AddAsync(post, cancellationToken);

				await _context.SaveChangesAsync(cancellationToken);

				return ApiResponse<PostDto>.Ok(_mapper.Map<PostDto>(post));
			}
		}
	}
}
