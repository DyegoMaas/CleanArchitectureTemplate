using FluentValidation;

namespace Application.Books.SetBookContent
{
    public class SetBookContentRequestValidator : AbstractValidator<SetBookContentRequest>
    {
        public SetBookContentRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required. Cannot be empty.");
        }
    }
}