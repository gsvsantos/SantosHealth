using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrganizaMed.Aplicacao.ModuloAutenticacao.DTOs;
using OrganizaMed.Dominio.ModuloAutenticacao;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrganizaMed.Aplicacao.ModuloAutenticacao.Services;

public class JwtProvider : ITokenProvider
{
    private readonly string? chaveJwt;
    private readonly DateTime dataExpiracaoJwt;
    private readonly string? audienciaValida;

    public JwtProvider(IConfiguration config)
    {
        this.chaveJwt = config["JWT_GENERATION_KEY"];

        if (string.IsNullOrEmpty(this.chaveJwt))
        {
            throw new ArgumentException("Chave de geração de tokens não configurada");
        }

        this.audienciaValida = config["JWT_AUDIENCE_DOMAIN"];

        if (string.IsNullOrEmpty(this.audienciaValida))
        {
            throw new ArgumentException("Audiência válida para transmissão de tokens não configurada");
        }

        this.dataExpiracaoJwt = DateTime.UtcNow.AddMinutes(5);
    }

    public IAccessToken GerarTokenDeAcesso(Usuario usuario)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        byte[] chaveEmBytes = Encoding.ASCII.GetBytes(this.chaveJwt!);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = "OrganizaMed",
            Audience = this.audienciaValida,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName!)
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(chaveEmBytes),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Expires = this.dataExpiracaoJwt,
            NotBefore = DateTime.UtcNow
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        string tokenString = tokenHandler.WriteToken(token);

        return new TokenResponse()
        {
            Chave = tokenString,
            DataExpiracao = this.dataExpiracaoJwt,
            Usuario = new UsuarioAutenticadoDto
            {
                Id = usuario.Id,
                UserName = usuario.UserName!,
                Email = usuario.Email!
            }
        };
    }
}