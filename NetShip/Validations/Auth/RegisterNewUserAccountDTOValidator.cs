using FluentValidation;
using NetShip.DTOs.Auth;
using NetShip.Utilities;
using System.Runtime.ConstrainedExecution;

namespace NetShip.Validations.Auth
{
    public class RegisterNewUserAccountDTOValidator : AbstractValidator<RegisterNewUserAccountDTO>
    {

        public RegisterNewUserAccountDTOValidator()
        {
            // Validacion de user_first_name
            RuleFor(x => x.UserFirstName).NotEmpty().WithMessage(Tools.RequiredFieldMessage)
                .MinimumLength(2).WithMessage(Tools.MinimumLengthMessage)
                .MaximumLength(50).WithMessage(Tools.MaximumLengthMessage);

            // Validacion de user_last_name
            RuleFor(x => x.UserLastName).NotEmpty().WithMessage(Tools.RequiredFieldMessage)
                .MinimumLength(2).WithMessage(Tools.MinimumLengthMessage)
                .MaximumLength(50).WithMessage(Tools.MaximumLengthMessage);

            // Validacion de user_email
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage(Tools.RequiredFieldMessage)
                .MinimumLength(4).WithMessage(Tools.MinimumLengthMessage)
                .MaximumLength(256).WithMessage(Tools.MaximumLengthMessage)
                .EmailAddress().WithMessage(Tools.EmailInvalid)
                .Must((dto, email) => IsUnique(email)).WithMessage(Tools.EmailExist);

            // Validacion de user_password
            RuleFor(x => x.UserPassword).NotEmpty().WithMessage(Tools.RequiredFieldMessage)
                .MinimumLength(6).WithMessage(Tools.MinimumLengthMessage)
                .MaximumLength(50).WithMessage(Tools.MaximumLengthMessage)
                .Must((dto, password) => IsPasswordSecure(password)).WithMessage(Tools.PasswordSecure);

            // Validacion de brand_name
            RuleFor(x => x.BrandName).NotEmpty().WithMessage(Tools.RequiredFieldMessage)
                .MinimumLength(2).WithMessage(Tools.MinimumLengthMessage)
                .MaximumLength(50).WithMessage(Tools.MaximumLengthMessage);

            // Validacion de branch_name
            RuleFor(x => x.BranchName).NotEmpty().WithMessage(Tools.RequiredFieldMessage)
                .MinimumLength(2).WithMessage(Tools.MinimumLengthMessage)
                .MaximumLength(50).WithMessage(Tools.MaximumLengthMessage);
        }

        
        public static bool IsUnique(string email)
        {
            // Falta agregar validacion de que sea email unico
            return true; 
        }

        public static bool IsPasswordSecure(string password)
        {
            return password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit);
        }
    }
}
