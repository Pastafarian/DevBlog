using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevBlog.Application.Response;
using DevBlog.Application.Services;
using DevBlog.Domain;
using DevBlog.Domain.Entities;

namespace DevBlog.Application.Handlers.Command
{
	public class UploadImage
	{
		public class Command : IRequest<ApiResponse<ImageUploadedResponseDto>>
		{
			public byte[] Content { get; }
			public string FileName { get; }

			public Command() { }

			public Command(byte[] content, string fileName)
			{
				Content = content;
				FileName = fileName;
			}
		}

		public class Handler : IRequestHandler<Command, ApiResponse<ImageUploadedResponseDto>>
		{
			private readonly Context _context;
			private readonly IImageStorageService _imageStorageService;

			public Handler(Context context, IImageStorageService imageStorageService)
			{
				_context = context;
				_imageStorageService = imageStorageService;
			}

			public async Task<ApiResponse<ImageUploadedResponseDto>> Handle(Command command, CancellationToken cancellationToken)
			{
				var imageUrl = await _imageStorageService.StoreImage(command.Content, command.FileName, cancellationToken);

				await _context.File.AddAsync(new File { FileName = imageUrl }, cancellationToken);

				await _context.SaveChangesAsync(cancellationToken);

				return ApiResponse<ImageUploadedResponseDto>.Ok(new ImageUploadedResponseDto(imageUrl));
			}
		}
	}
}
