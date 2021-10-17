using FluentValidation;

namespace CleanArchitectureTemplate.Application.Books.AddBook
{
    public class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookRequestValidator()
        {
            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Publisher)
                .NotEmpty().WithMessage("Publisher is required");
            
            RuleFor(x => x.Origin)
                .NotNull().WithMessage("Origin is required");
            RuleFor(x => x.Origin.Planet)
                .NotEmpty().WithMessage("Planet of origin is required")
                .Unless(x => x.Origin is null);
            RuleFor(x => x.Origin.System)
                .NotEmpty().WithMessage("System of origin is required")
                .Unless(x => x.Origin is null);;
        }
    }
}