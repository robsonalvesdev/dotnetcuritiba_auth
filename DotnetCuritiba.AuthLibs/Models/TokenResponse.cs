// =============================================================================
// TokenResponse.cs
// =============================================================================
using Newtonsoft.Json;

namespace DotnetCuritiba.AuthLibs.Models
{
    public class TokenResponse
    {
        /// <summary>
        /// Token de acesso
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; init; }

        /// <summary>
        /// Tempo em segundos.
        /// </summary>
        [JsonProperty("expires_in")]
        public ulong ExpiresIn { get; init; }

        [JsonProperty("refresh_expires_in")]
        public ulong RefreshExpiresIn { get; init; }

        /// <summary>
        /// Tipo de token (Ex: Bearer)
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; init; }

        [JsonProperty("id_token")]
        public string IdToken { get; init; }

        [JsonProperty("not-before-policy")]
        public int NotBeforePolicy { get; init; }

        /// <summary>
        /// Escopo de acesso do end-point
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; init; }
    }
}