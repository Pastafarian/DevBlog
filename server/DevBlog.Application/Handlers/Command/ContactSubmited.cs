using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DevBlog.Application.Requests;
using DevBlog.Application.Requests.Validators;
using DevBlog.Application.Response;
using DevBlog.Domain;
using DevBlog.Domain.Entities;

namespace DevBlog.Application.Handlers.Command
{
	public class SubmitContact
	{
		public class Command : IRequest<ApiResponse<EntityCreatedResponseDto>>
		{
			public SubmitContactRequestDto Request { get; }

			public Command(SubmitContactRequestDto request)
			{
				Request = request;
			}
		}

		public class Handler : IRequestHandler<Command, ApiResponse<EntityCreatedResponseDto>>
		{
			private readonly Context context;
			private readonly IMapper mapper;
            private readonly SubmitContactRequestValidator validator;

            public Handler(Context context, IMapper mapper, SubmitContactRequestValidator validator)
			{
				this.context = context;
				this.mapper = mapper;
                this.validator = validator;
            }

			public async Task<ApiResponse<EntityCreatedResponseDto>> Handle(Command command, CancellationToken cancellationToken)
            {
                var validationResponse = await validator.ValidateAsync(command.Request, cancellationToken);

                if (!validationResponse.IsValid)
                    return ApiResponse<EntityCreatedResponseDto>.BadRequest(validationResponse.ToString());

				var contact = mapper.Map<Contact>(command.Request);
				await context.Contacts.AddAsync(contact, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				return ApiResponse<EntityCreatedResponseDto>.Ok(EntityCreatedResponseDto.Create(contact.Id)); 
			}
		}
	}
}