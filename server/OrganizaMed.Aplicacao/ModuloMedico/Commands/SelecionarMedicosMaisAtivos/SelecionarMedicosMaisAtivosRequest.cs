using FluentResults;
using MediatR;

namespace OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarMedicosMaisAtivos;

public record SelecionarMedicosMaisAtivosRequest(DateTime inicioPeriodo, DateTime terminoPeriodo) :
    IRequest<Result<SelecionarMedicosMaisAtivosResponse>>;