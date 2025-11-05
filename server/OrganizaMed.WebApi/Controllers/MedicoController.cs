using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.Editar;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.Excluir;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.Inserir;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarMedicosMaisAtivos;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarPorId;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarTodos;
using OrganizaMed.WebApi.Extensions;

namespace OrganizaMed.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/medicos")]
public class MedicoController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(typeof(InserirMedicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirMedicoRequest request)
    {
        FluentResults.Result<InserirMedicoResponse> resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarMedicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarMedicoPartialRequest request)
    {
        EditarMedicoRequest editarRequest = new(
            id,
            request.Nome,
            request.Crm
        );

        FluentResults.Result<EditarMedicoResponse> resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirMedicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        ExcluirMedicoRequest excluirRequest = new(id);

        FluentResults.Result<ExcluirMedicoResponse> resultado = await mediator.Send(excluirRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarMedicosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        FluentResults.Result<SelecionarMedicosResponse> resultado = await mediator.Send(new SelecionarMedicosRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarMedicoPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        SelecionarMedicoPorIdRequest selecionarPorIdRequest = new(id);

        FluentResults.Result<SelecionarMedicoPorIdResponse> resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet("top-10")]
    [ProducesResponseType(typeof(SelecionarMedicosMaisAtivosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarMedicosMaisAtivosPorPeriodo(
        [FromQuery] DateTime inicioPeriodo, [FromQuery] DateTime terminoPeriodo)
    {
        FluentResults.Result<SelecionarMedicosMaisAtivosResponse> resultado = await mediator.Send(new SelecionarMedicosMaisAtivosRequest(
            inicioPeriodo,
            terminoPeriodo
        ));

        return resultado.ToHttpResponse();
    }
}
