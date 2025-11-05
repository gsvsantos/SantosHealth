using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.Editar;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.Excluir;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.Inserir;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.SelecionarPorId;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.SelecionarTodos;
using OrganizaMed.WebApi.Extensions;

namespace OrganizaMed.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/pacientes")]
public class PacienteController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirPacienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirPacienteRequest request)
    {
        FluentResults.Result<InserirPacienteResponse> resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarPacienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarPacientePartialRequest request)
    {
        EditarPacienteRequest editarRequest = new(
            id,
            request.Nome,
            request.Cpf,
            request.Email,
            request.Telefone
        );

        FluentResults.Result<EditarPacienteResponse> resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirPacienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        ExcluirPacienteRequest excluirRequest = new(id);

        FluentResults.Result<ExcluirPacienteResponse> resultado = await mediator.Send(excluirRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarPacientesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        FluentResults.Result<SelecionarPacientesResponse> resultado = await mediator.Send(new SelecionarPacientesRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarPacientePorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        SelecionarPacientePorIdRequest selecionarPorIdRequest = new(id);

        FluentResults.Result<SelecionarPacientePorIdResponse> resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}