using HotelSearchService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelSearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method for user login
        /// </summary>
        /// <param name="userLogin">Object with username and password</param>
        /// <returns>Bearer token</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {
            if (IsValidUser(userLogin))
            {
                var token = GenerateJwtToken(userLogin.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Checks if user is valid
        /// </summary>
        /// <param name="userLogin">DTO with username and password parameters</param>
        /// <returns>true</returns>
        private bool IsValidUser(UserLoginDto userLogin)
        {
            // Validate the user credentials (this is just a demo, so we accept any user)
            // In a real application, you would check the user credentials against a database
            return true;
        }

        /// <summary>
        /// Generates JWT token
        /// </summary>
        /// <param name="username">DTO with username and password parameters</param>
        /// <returns>Bearer token</returns>
        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiryMinutes = jwtSettings.GetValue<int>("ExpiryMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
