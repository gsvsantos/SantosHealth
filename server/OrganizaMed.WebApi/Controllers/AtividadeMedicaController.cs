using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.Editar;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.Excluir;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.Inserir;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.SelecionarPorId;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.SelecionarTodos;
using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.WebApi.Extensions;

namespace OrganizaMed.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/atividades-medicas")]
public class AtividadeMedicaController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirAtividadeMedicaResponse), 200)]
    public async Task<IActionResult> Inserir(InserirAtividadeMedicaRequest request)
    {
        FluentResults.Result<InserirAtividadeMedicaResponse> response = await mediator.Send(request);

        return response.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarAtividadeMedicaResponse), 200)]
    public async Task<IActionResult> Editar(Guid id, EditarAtividadeMedicaPartialRequest partialRequest)
    {
        EditarAtividadeMedicaRequest request = new(
            id,
            partialRequest.Inicio,
            partialRequest.Termino,
            partialRequest.Medicos
        );

        FluentResults.Result<EditarAtividadeMedicaResponse> response = await mediator.Send(request);

        return response.ToHttpResponse();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirAtividadeMedicaResponse), 200)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        ExcluirAtividadeMedicaRequest request = new(id);

        FluentResults.Result<ExcluirAtividadeMedicaResponse> response = await mediator.Send(request);

        return response.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarAtividadesMedicasResponse), 200)]
    public async Task<IActionResult> SelecionarTodos([FromQuery] TipoAtividadeMedica? tipoAtividade)
    {
        SelecionarAtividadesMedicasRequest request = new(tipoAtividade);

        FluentResults.Result<SelecionarAtividadesMedicasResponse> response = await mediator.Send(request);

        return response.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarAtividadeMedicaPorIdResponse), 200)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        SelecionarAtividadeMedicaPorIdRequest selecionarPorIdRequest = new(id);

        FluentResults.Result<SelecionarAtividadeMedicaPorIdResponse> resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}