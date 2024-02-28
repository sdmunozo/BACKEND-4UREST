using Microsoft.IdentityModel.Tokens;

namespace NetShip.Utilities
{
    public static class Keys
    {
        public const string OwnIssuer = "4uRest-App";
        private const string KeysSection = "Authentication:Schemes:Bearer:SigningKeys";
        private const string IssuerSection = "Issuer";
        private const string ValueSection = "Value";

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration)
            => GetKey(configuration, OwnIssuer);

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(KeysSection)
                .GetChildren()
                .SingleOrDefault(key => key[IssuerSection] == issuer);

            if(signingKey is not null && signingKey[ValueSection] is string KeyValue) {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(KeyValue));
            }
        }

        public static IEnumerable<SecurityKey> GetAllKey(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(KeysSection)
                .GetChildren();

            foreach(var signingKey in signingKeys) {

                if (signingKey[ValueSection] is string KeyValue)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(KeyValue));
                }
            }


        }
    }
}
