using FluentResults;
using MediatR;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarMedicosMaisAtivos;

public class SelecionarMedicosMaisAtivosRequestHandler(
    IRepositorioMedico repositorioMedico
) : IRequestHandler<SelecionarMedicosMaisAtivosRequest, Result<SelecionarMedicosMaisAtivosResponse>>
{
    public async Task<Result<SelecionarMedicosMaisAtivosResponse>> Handle(SelecionarMedicosMaisAtivosRequest request, CancellationToken cancellationToken)
    {
        List<RegistroDeHorasTrabalhadas> registros = await repositorioMedico.SelecionarMedicosMaisAtivosPorPeriodo(
            request.inicioPeriodo,
            request.terminoPeriodo
        );

        SelecionarMedicosMaisAtivosResponse response = new()
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros.Select(m => new SelecionarRegistroDeHorasTrabalhadasDto(
                m.MedicoId, m.Medico, m.TotalDeHorasTrabalhadas
            ))
        };

        return Result.Ok(response);
    }
}