using DotnetCuritiba.AuthLibs.JwtGenerator;
using DotnetCuritiba.AuthLibs.Models;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;

namespace DotnetCuritiba.AuthTest ;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GenerateTokenAndValidate()
        {
            var rsaKeys = new RsaKeys
            {
                PrivateKey = File.ReadAllText("private-pkcs8.pem"),
                PublicKey = File.ReadAllText("public.pem")
            };

            var now = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();
            var unixTimeSecondsPlus15Minutes = new DateTimeOffset(now).AddMinutes(15).ToUnixTimeSeconds();

            var jwtCustomClaims = new JwtCustomClaims();
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Sub, "appclient");
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Iss, "appclient");
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString());
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Nbf, unixTimeSeconds.ToString());
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Exp, unixTimeSecondsPlus15Minutes.ToString());
            jwtCustomClaims.AddClaim(JwtRegisteredClaimNames.Aud, "http://localhost:8080/realms/dotnetcuritiba/protocol/openid-connect/token");
            
            var jwtHandler = new JwtHandler(rsaKeys, jwtCustomClaims);
            var tokenJwt = jwtHandler.CreateToken();
            tokenJwt.Should().NotBeNull();
            
            var validateJwt = jwtHandler.ValidateToken(tokenJwt!);
            validateJwt.Should().BeTrue();
        }
    }