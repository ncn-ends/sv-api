using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class DiscordIdAttribute : ValidationAttribute
{
    private readonly string[] _allowedCodes;

    public DiscordIdAttribute(params string[] allowedCodes)
    {
        _allowedCodes = allowedCodes;
    }

    protected override ValidationResult IsValid(
        object value, ValidationContext context)
    {
        var code = value as string;
        if (code == null) return ValidationResult.Success;
        var rg = new Regex(@"^.{2,32}#[0-9]{4}$");


        if (!rg.IsMatch(code))
        {
            return new ValidationResult("Not a valid Discord ID");
        }

        return ValidationResult.Success;
    }
}