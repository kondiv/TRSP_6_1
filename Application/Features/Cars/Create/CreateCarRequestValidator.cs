using FluentValidation;

namespace Application.Features.Cars.Create;

public sealed class CreateCarRequestValidator : AbstractValidator<CreateCarRequest>
{
    public const int MarkMaxLength = 64;
    
    public const int MarkMinLength = 2;
    
    public const int ModelMaxLength = MarkMaxLength;
    
    public const int ModelMinLength = MarkMinLength;

    public CreateCarRequestValidator()
    {
        RuleFor(r => r.Mark)
            .NotNull().WithMessage("Mark is required")
            .MinimumLength(MarkMinLength).WithMessage($"Mark name is too short. Minimum length {MarkMinLength}")
            .MaximumLength(MarkMaxLength).WithMessage($"Mark name is too long. Maximum length {MarkMaxLength}");

        RuleFor(r => r.Model)
            .NotNull().WithMessage("Model is required")
            .MinimumLength(ModelMinLength).WithMessage($"Model name is too short. Minimum length {ModelMinLength}")
            .MaximumLength(ModelMaxLength).WithMessage($"Model name is too long. Maximum length {ModelMaxLength}");
    }
}
