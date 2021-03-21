using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DevBlog.Domain;

namespace DevBlog.Application.Handlers.Command
{
	public class DeletePost
	{
		public class Command : IRequest
		{
			public int PostId { get; set; }

			public Command(int postId)
			{
				PostId = postId;
			}
		}


		public class Handler : AsyncRequestHandler<Command>
		{
			private readonly Context context;

			public Handler(Context context)
			{
				this.context = context;
			}

			protected override async Task Handle(Command command, CancellationToken cancellationToken)
			{
				var post = await context.Posts.SingleAsync(x => x.Id == command.PostId, cancellationToken);
				context.Posts.Remove(post);
				await context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}