using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceManagement.Core.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceManagement.Service.Authentication
{
	public class JwtTokenGeneration
	{
		private readonly IConfiguration _configuration;

		public JwtTokenGeneration(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, user.Role),
				new Claim("UserId", user.UserId.ToString())
			};

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			var creds = new SigningCredentials(
				key,
				SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(2),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
