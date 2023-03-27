// =============================================================================
// JwtCustomClaims.cs
// =============================================================================
using System.Security.Claims;

namespace DotnetCuritiba.AuthLibs.JwtGenerator
{
    public class JwtCustomClaims
    {
        public ClaimsIdentity Subject { get; }
        
        public JwtCustomClaims()
        {
            Subject = new ClaimsIdentity();
        }
        
        public void AddClaim(string claim, string value)
        {
            Subject.AddClaim(new Claim(claim, value));
        }
    }
}