// =============================================================================
// JwtHandler.cs
// =============================================================================
using DotnetCuritiba.AuthLibs.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace DotnetCuritiba.AuthLibs.JwtGenerator
{
    public class JwtHandler
    {
        private readonly JwtCustomClaims _claims;
        private readonly RsaKeys _rsaKeys;
        
        public JwtHandler(RsaKeys rsaKeys, JwtCustomClaims claims)
        {
            _rsaKeys = rsaKeys ?? throw new ArgumentNullException(nameof(rsaKeys));
            _claims = claims ?? throw new ArgumentNullException(nameof(claims));
        }

        public string CreateToken()
        {
            ReadOnlySpan<char> bytesPrivateKey = _rsaKeys.PrivateKey;
            
            var tokenHandler = new JwtSecurityTokenHandler();

            using var rsa = RSA.Create();
            rsa.ImportFromPem(bytesPrivateKey);
            
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = _claims.Subject,
                SigningCredentials = signingCredentials
            };
            
            var jwt = tokenHandler.CreateToken(tokenDescriptor);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public bool ValidateToken(string token)
        {
            var bytesPublicKey = _rsaKeys.PublicKey?.ToCharArray();

            var tokenHandler = new JwtSecurityTokenHandler();

            using var rsa = RSA.Create();
            rsa.ImportFromPem(bytesPublicKey);

            var claimIss = _claims.Subject.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss);
            var claimAud = _claims.Subject.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud);

            var paramTokenValidator = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = claimIss!.Value,
                ValidAudience = claimAud!.Value,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                CryptoProviderFactory = new CryptoProviderFactory
                {
                    CacheSignatureProviders = false
                }
            };
            
            try
            {
                tokenHandler.ValidateToken(token, paramTokenValidator, out var securityToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}