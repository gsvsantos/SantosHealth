using FluentResults;
using MediatR;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Aplicacao.ModuloPaciente.Commands.SelecionarTodos;

public class SelecionarPacientesRequestHandler(
    IRepositorioPaciente repositorioPaciente
) : IRequestHandler<SelecionarPacientesRequest, Result<SelecionarPacientesResponse>>
{
    public async Task<Result<SelecionarPacientesResponse>> Handle(
        SelecionarPacientesRequest request, CancellationToken cancellationToken)
    {
        List<Paciente> registros = await repositorioPaciente.SelecionarTodosAsync();

        SelecionarPacientesResponse response = new()
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(r => new SelecionarPacientesDto(r.Id, r.Nome, r.Cpf, r.Email, r.Telefone))
        };

        return Result.Ok(response);
    }
}