using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(Doctor doctor)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, doctor.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, doctor.Email.Value),
            new("doctor", "doctor"),
        };


        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1), 
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}