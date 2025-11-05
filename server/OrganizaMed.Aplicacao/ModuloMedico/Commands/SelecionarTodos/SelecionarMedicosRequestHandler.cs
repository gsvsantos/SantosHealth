using FluentResults;
using MediatR;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarTodos;

public class SelecionarMedicosRequestHandler(
    IRepositorioMedico repositorioMedico
) : IRequestHandler<SelecionarMedicosRequest, Result<SelecionarMedicosResponse>>
{
    public async Task<Result<SelecionarMedicosResponse>> Handle(SelecionarMedicosRequest request, CancellationToken cancellationToken)
    {
        List<Medico> registros = await repositorioMedico.SelecionarTodosAsync();

        SelecionarMedicosResponse response = new()
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(r => new SelecionarMedicosDto(r.Id, r.Nome, r.Crm))
        };

        return Result.Ok(response);
    }
}