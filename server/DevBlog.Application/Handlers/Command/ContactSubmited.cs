using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DevBlog.Application.Requests;
using DevBlog.Application.Requests.Validators;
using DevBlog.Application.Response;
using DevBlog.Domain;
using DevBlog.Domain.Entities;
using DevBlog.Application.Services;
using Microsoft.Extensions.Logging;

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
			private readonly Context _context;
			private readonly IMapper _mapper;
            private readonly SubmitContactRequestValidator _validator;
            private readonly ISendGridService _sendGridService;
            private readonly ILogger<Handler> _logger;

            public Handler(Context context, IMapper mapper, SubmitContactRequestValidator validator, ISendGridService sendGridService, ILogger<Handler> logger)
			{
				_context = context;
				_mapper = mapper;
                _validator = validator;
                _sendGridService = sendGridService;
                _logger = logger;
            }

			public async Task<ApiResponse<EntityCreatedResponseDto>> Handle(Command command, CancellationToken cancellationToken)
            {
                _logger.LogDebug("SubmitContact.Handler called. With message: Name:{Name}, Email:{Email}, Message:{Message}. ", command.Request.Name, command.Request.Email, command.Request.Message);

                var validationResponse = await _validator.ValidateAsync(command.Request, cancellationToken);

                if (!validationResponse.IsValid)
                    return ApiResponse<EntityCreatedResponseDto>.BadRequest(validationResponse.ToString());

				var contact = _mapper.Map<Contact>(command.Request);
				await _context.Contacts.AddAsync(contact, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);

                await _sendGridService.SendEmailAsync(contact.Name, contact.Email, contact.Message);

                return ApiResponse<EntityCreatedResponseDto>.Ok(EntityCreatedResponseDto.Create(contact.Id)); 
			}
		}
	}
}