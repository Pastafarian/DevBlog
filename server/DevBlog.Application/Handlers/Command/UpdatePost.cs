using System;
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
using DevBlog.Application.Requests.Validators;
using DevBlog.Application.Requests;

namespace DevBlog.Application.Handlers.Command
{
	public class UpdatePost
	{
		public class Command : IRequest<ApiResponse<PostDto>>
		{
			public UpdatePostRequestDto Post { get; }

			public Command(UpdatePostRequestDto post)
			{
				Post = post;
			}
		}

		public class Handler : IRequestHandler<Command, ApiResponse<PostDto>>
		{
			private readonly Context _context;
			private readonly IMapper _mapper;
			private readonly IImageStorageService _imageStorageService;
            private readonly UpdatePostRequestValidator _updatePostRequestValidator;

            public Handler(Context context, IMapper mapper, IImageStorageService imageStorageService, UpdatePostRequestValidator updatePostRequestValidator)
			{
				_mapper = mapper;
				_imageStorageService = imageStorageService;
                _updatePostRequestValidator = updatePostRequestValidator;
                _context = context;
			}

			public async Task<ApiResponse<PostDto>> Handle(Command command, CancellationToken cancellationToken)
            {
                var validationResult = await _updatePostRequestValidator.ValidateAsync(command.Post, cancellationToken);
                var post = _context.Posts.SingleOrDefault(x => x.Id == command.Post.Id);

				if (post == null) return ApiResponse<PostDto>.NotFound($"Unable to find article with id '{command.Post.Id}'");

				if (await _context.Posts.AnyAsync(x => x.Slug == command.Post.Slug && x.Id != command.Post.Id, cancellationToken)) return ApiResponse<PostDto>.BadRequest("A post with that slug already exists");

                try
                {
                    _mapper.Map(command.Post, post);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
				

				if (!string.IsNullOrWhiteSpace(command.Post.HeaderImage))
					post.HeaderImage = await _imageStorageService.StoreImage(command.Post.HeaderImage, command.Post.Title, cancellationToken);

				await _context.SaveChangesAsync(cancellationToken);

				return ApiResponse<PostDto>.Ok(_mapper.Map<PostDto>(post));
			}
		}
	}
}
