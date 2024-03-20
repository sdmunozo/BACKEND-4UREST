using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace NetShip.Utilities
{
    public static class StringUtils
    {
        public static string NormalizeUrlName(string name)
        {
            var normalized = name.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string withoutDiacritics = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return Regex.Replace(withoutDiacritics, @"[^a-zA-Z0-9\s-]", "") // Elimina caracteres especiales
                        .Replace('ñ', 'n')
                        .Replace('Ñ', 'N')
                        .Replace(' ', '-') // Reemplaza espacios por guiones
                        .ToLower(); // Convierte a minúsculas
        }
    }

}
