using FluentValidation;

namespace DevBlog.Application.Requests.Validators
{
    public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequestDto>
    {
        public UpdatePostRequestValidator()
        {
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
