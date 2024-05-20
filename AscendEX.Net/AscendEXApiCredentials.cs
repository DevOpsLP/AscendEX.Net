using System.Security;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;

namespace AscendEX.Net
{
    public class AscendEXApiCredentials : ApiCredentials
    {
        public AscendEXApiCredentials(string key, string secret) : base(key, secret)
        {
        }

        public AscendEXApiCredentials(SecureString key, SecureString secret) : base(key, secret)
        {
        }

        public AscendEXApiCredentials(string key, string secret, ApiCredentialsType credentialsType) 
            : base(key, secret, credentialsType)
        {
        }

        public AscendEXApiCredentials(SecureString key, SecureString secret, ApiCredentialsType credentialsType) 
            : base(key, secret, credentialsType)
        {
        }

        public AscendEXApiCredentials(Stream inputStream, string? identifierKey = null, string? identifierSecret = null) 
            : base(inputStream, identifierKey, identifierSecret)
        {
        }

        public override ApiCredentials Copy()
        {
            return new AscendEXApiCredentials(Key.GetString(), Secret.GetString(), this.CredentialType);
        }
    }
}
