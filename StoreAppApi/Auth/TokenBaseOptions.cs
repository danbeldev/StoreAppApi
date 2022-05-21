using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StoreAppApi.Auth
{
    public class TokenBaseOptions
    {
        public const string ISSUER = "StoreAppApi";

        public const string AUDIENCE = "StoreAppClient";

        const string KEY = "xPhvXHvHFRWKxVntrPkv3vUqccTUf2wD9hEMW6APzUnCcRE4SuWeumEtdZUbQqAwtHQxtXb4usC5yy8P4uGNF7cUmzK2StkgGEpJn5qxVKdvhtEq8KDtfmYH2JKxMZYgS4R2LgdZDL5VQdmHXJBHywWzUJSfbf48E2THggwkVusNFCJna6NYgsSaQTF4SU93gnsrm3Yg4gtXdCTxY4PqDks3yBXSmfqsTGfcazzeCM8QWgyNV6BRynhjgZWxPAkc";

        public const int LIFETIME = 14;

        public static SymmetricSecurityKey GetSymmetricSecurityKey() 
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
