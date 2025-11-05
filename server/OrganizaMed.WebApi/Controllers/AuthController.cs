using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrganizaMed.Aplicacao.ModuloAutenticacao.Commands.Autenticar;
using OrganizaMed.Aplicacao.ModuloAutenticacao.Commands.Registrar;
using OrganizaMed.Dominio.ModuloAutenticacao;
using OrganizaMed.WebApi.Extensions;

namespace OrganizaMed.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator, SignInManager<Usuario> signInManager) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioRequest request)
    {
        FluentResults.Result<Aplicacao.ModuloAutenticacao.DTOs.TokenResponse> tokenResult = await mediator.Send(request);

        return tokenResult.ToHttpResponse();
    }

    [HttpPost("autenticar")]
    public async Task<IActionResult> Autenticar(AutenticarUsuarioRequest request)
    {
        FluentResults.Result<Aplicacao.ModuloAutenticacao.DTOs.TokenResponse> tokenResult = await mediator.Send(request);

        return tokenResult.ToHttpResponse();
    }

    [HttpPost("sair")]
    [Authorize]
    public async Task<IActionResult> Sair()
    {
        await signInManager.SignOutAsync();

        return Ok();
    }
}