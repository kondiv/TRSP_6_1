using Application.Features.Cars.Create;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Tests.Unit.Application.Features.CreateCar;

public class ValidatorTests
{
    private readonly IValidator<CreateCarCommand> _validator;

    public ValidatorTests()
    {
        _validator = new CreateCarCommandValidator();
    }

    public static TheoryData<string> InvalidLengthStrings()
    {
        return
        [
            new string('f', 1),
            new string('f', 1000),
            String.Empty
        ];
    }

    [Fact]
    public void Validate_WhenMarkIsNull_And_ModelIsValid_ShouldReturnResultInvalid_With_MarkError()
    {
        // Arrange
        string? mark = null;
        string model = "Priora";

        var command = new CreateCarCommand(mark!, model);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Mark).WithErrorMessage("Mark is required");
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Fact]
    public void Validate_WhenModelIsNull_And_MarkIsValid_ShouldReturnResultINvalid_With_ModelError()
    {
        // Arrange
        string mark = "Lada";
        string? model = null;

        var command = new CreateCarCommand(mark, model!);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Model).WithErrorMessage("Model is required");
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Mark);
    }

    [Theory]
    [MemberData(nameof(InvalidLengthStrings))]
    public void Validate_WhenMarkLengthIsNotValid_ShouldReturnResultInvalid_With_MarkError(string mark)
    {
        // Arrange
        var model = "Valid";

        var command = new CreateCarCommand(mark, model);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Mark);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Theory]
    [MemberData(nameof(InvalidLengthStrings))]
    public void Validate_WhenModelLengthIsNotValid_ShouldReturnResultInvalid_With_MarkError(string model)
    {
        // Arrange
        var mark = "Valid";

        var command = new CreateCarCommand(mark, model);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Model);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Mark);
    }

    [Theory]
    [InlineData("Lada", "Priora")]
    [InlineData("Mercedez-Benz", "S63 AMG")]
    [InlineData("BMW", "M5")]
    [InlineData("Chevrolet", "Tahoe")]
    [InlineData("Cadillac", "Escalade")]
    [InlineData("Lamborghini", "Urus")]
    public void Validate_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors(string mark, string model)
    {
        // Arrange
        var command = new CreateCarCommand(mark, model);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}
