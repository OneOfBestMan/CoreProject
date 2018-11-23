using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Model;

namespace Core.Helper
{
    /// <summary>
    /// Jwt helper.
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Gets the token claims.
        /// </summary>
        /// <returns>The token claims.</returns>
        /// <param name="sourceId">Source ıd.</param>
        /// <param name="applicationId">Application ıd.</param>
        /// <param name="userId">User ıd.</param>
        /// <param name="deviceId">Device ıd.</param>
        /// <param name="bundleId">Bundle ıd.</param>
        /// <param name="country">Country.</param>
        /// <param name="language">Language.</param>
        /// <param name="isAdmin">If set to <c>true</c> is admin.</param>
        public static IEnumerable<Claim> GetTokenClaims(Guid sourceId, Guid applicationId, Guid userId, string deviceId, string bundleId, string country, string language, bool isAdmin)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
                new Claim("sourceId", sourceId.ToString()),
                new Claim("applicationId", applicationId.ToString()),
                new Claim("deviceId", deviceId),
                new Claim("bundleId", bundleId),
                new Claim("country", country),
                new Claim("language", language),
                new Claim("isAdmin", isAdmin.ToString())
            };
        }

        /// <summary>
        /// Gets the jwt security token.
        /// </summary>
        /// <returns>The jwt security token.</returns>
        /// <param name="sourceId">Source ıd.</param>
        /// <param name="applicationId">Application ıd.</param>
        /// <param name="userId">User ıd.</param>
        /// <param name="deviceId">Device ıd.</param>
        /// <param name="bundleId">Bundle ıd.</param>
        /// <param name="country">Country.</param>
        /// <param name="language">Language.</param>
        /// <param name="isAdmin">If set to <c>true</c> is admin.</param>
        public static string GetJwtSecurityToken(Guid sourceId, Guid applicationId, Guid userId, string deviceId, string bundleId, string country, string language, bool isAdmin)
        {
            var token = new JwtSecurityToken(
                "",
                "",
                GetTokenClaims(sourceId, applicationId, userId, deviceId, bundleId, country, language, isAdmin),
                DateTime.UtcNow,
                DateTime.UtcNow.AddYears(1),
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("01b51b422b58a053bf55eda66ad50f7b")), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Valids the token.
        /// </summary>
        /// <returns><c>true</c>, if token was valided, <c>false</c> otherwise.</returns>
        /// <param name="token">Token.</param>
        public static bool ValidToken(this string token)
        {
            try
            {
                var jwtSTH = new JwtSecurityTokenHandler();
                var validateToken = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("01b51b422b58a053bf55eda66ad50f7b")),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                if (jwtSTH.CanValidateToken)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Decrypts the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="token">Token.</param>
        public static Token DecryptToken(this string token)
        {
            var payload = new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;

            return new Token
            {
                IsValid = ValidToken(token),
                ExpiryDate = payload.Exp.ToString(),
                UserId = Guid.Parse(payload.Jti),
                SourceId = Guid.Parse(payload["sourceId"].ToString()),
                ApplicationId = Guid.Parse(payload["applicationId"].ToString()),
                BundleId = payload["bundleId"].ToString(),
                Country = payload["country"].ToString(),
                DeviceId = payload["deviceId"].ToString(),
                IsAdmin = bool.Parse(payload["isAdmin"].ToString()),
                Language = payload["language"].ToString()
            };
        }
    }
}