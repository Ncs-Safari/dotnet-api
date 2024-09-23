using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Services.Configurations;

namespace UserManagement.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        private static int RandomNumber(int min, int max)
        {
            Random random = new();
            return random.Next(min, max);
        }
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new();
            Random random = new();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public string GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(JwtSettings.AppSecret)),
                    SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, !string.IsNullOrEmpty(user.LastName)?user.LastName:"-"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userid", user.Id.ToString())

            };
            var securityToken = new JwtSecurityToken(
                issuer: JwtSettings.AppIssuer,
                audience: JwtSettings.AppAudience,
                expires: _dateTimeProvider.UtcNow.AddMinutes(JwtSettings.AppExpireMinutes),
//                expires: DateTime.Now.AddMinutes(JwtSettings.AppExpireMinutes),
                claims: claims,
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public string GenerateTokenForgotPassword(User user)
        {
            var signingCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(JwtSettings.UserSecret)),
                                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, !string.IsNullOrEmpty(user.LastName)?user.LastName:"-"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userid", user.Id.ToString())

            };
            var securityToken = new JwtSecurityToken(
                issuer: JwtSettings.UserIssuer,
                audience: JwtSettings.UserAudience,
                expires: _dateTimeProvider.UtcNow.AddSeconds(JwtSettings.UserExpireSeconds),
//                expires: DateTime.Now.AddSeconds(JwtSettings.UserExpireSeconds),
                claims: claims,
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public string ValidateTokenForgotPassword(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.UserSecret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "userid").Value;

                // return user id from JWT token if validation successful
                return userId;
            }
            catch(System.Exception ex)
            {
                // return null if validation fails
                Console.WriteLine(ex.Message);
                throw new ApplicationException(ex.Message);
//                return null;
            }
        }
        public async Task<string> GeneratePassword()
        {
            StringBuilder builder = new();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return await Task.FromResult(builder.ToString());

        }

    }
}