using FluentValidation.Results;

namespace RetroBot.Application.Validators;

public static class ValidatorExtension
{
    public static string GetCombinedErrorMessage(this ValidationResult validationResult)
    {
        return string.Join(".", validationResult.Errors.Select(e => e.ErrorMessage));
    }
}