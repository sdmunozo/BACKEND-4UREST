namespace NetShip.Utilities
{
    public class Tools
    {
        public static string RequiredFieldMessage = "El campo {PropertyName} es requerido.";
        public static string MaximumLengthMessage = "El campo {PropertyName} debe tener menos de {MaxLength} caracteres.";
        public static string MinimumLengthMessage = "El campo {PropertyName} debe tener al menos {MinLength} caracteres.";
        public static string FirstLetterUppercaseMessage = "El campo {PropertyName} debe comenzar con mayúsculas.";
        public static string EmailInvalid = "El campo {PropertyName} debe ser un email válido.";
        public static string EmailExist = "El campo {PropertyName} ya está registrado.";
        public static string PasswordSecure = "El campo {PropertyName} debe de contener al menos 6 caracteres, un dígito, una letra mayúscula y una minúscula.";




        /*

       public static bool IsFieldValid(string value, int minLength, int maxLength)
       {
           return !string.IsNullOrEmpty(value) && value.Length >= minLength && value.Length <= maxLength;
       }

       public static bool IsEmailValid(string email)
       {
           // You could use a regular expression to validate the email format here.
           return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
       }

       // This is a simplification, uniqueness should be checked against the database.
       public static bool IsUnique(string value, string field, string table)
       {
           // Implement logic to check for uniqueness in the database.
           return true; // Simplification for example purposes.
       }

       public static bool IsPasswordSecure(string password)
       {
           // Validate the length and required characters for the password.
           return password.Length >= 8 &&
                  password.Any(char.IsUpper) &&
                  password.Any(char.IsLower) &&
                  password.Any(char.IsDigit) &&
                  password.Any(ch => !char.IsLetterOrDigit(ch));
       }

       /*
       public static string EncryptPassword(string password)
       {
           // Implement encryption logic.
           return Convert.ToBase64String(Encrypt(password));
       }

       // Assuming we have an Encrypt() function that encrypts the password.

        * private static byte[] Encrypt(string password)
       {
           // Implement encryption algorithm.

       } */

    }
}
