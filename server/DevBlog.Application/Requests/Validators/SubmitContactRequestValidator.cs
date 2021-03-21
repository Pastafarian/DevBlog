using FluentValidation;

namespace DevBlog.Application.Requests.Validators
{
    public class SubmitContactRequestValidator : AbstractValidator<SubmitContactRequestDto>
    {
        public SubmitContactRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        }
    }
}