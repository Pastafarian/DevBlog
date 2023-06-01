using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Domain;
using DevBlog.Application.Response;

namespace DevBlog.Application.Handlers.Command
{
	public class DeletePost
	{
		public class Command : IRequest<ApiResponse>
		{
			public int PostId { get; set; }

			public Command(int postId)
			{
				PostId = postId;
			}
		}

		public class Handler : IRequestHandler<Command, ApiResponse>
		{
			private readonly Context _context;

			public Handler(Context context)
			{
				_context = context;
			}

			public async Task<ApiResponse> Handle(Command command, CancellationToken cancellationToken)
			{
                try
                {
                    var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == command.PostId, cancellationToken);

                    if (post == null)
                        return ApiResponse.NotFound($"Error: Unable to find post with id {command.PostId} to delete.");

                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync(cancellationToken);
                    return ApiResponse.Ok();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
				
            }
		}
	}
}