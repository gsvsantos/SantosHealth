using FluentResults;
using MediatR;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Aplicacao.ModuloMedico.Commands.SelecionarPorId;

public class SelecionarMedicoPorIdRequestHandler(
    IRepositorioMedico repositorioMedico
) : IRequestHandler<SelecionarMedicoPorIdRequest, Result<SelecionarMedicoPorIdResponse>>
{
    public async Task<Result<SelecionarMedicoPorIdResponse>> Handle(SelecionarMedicoPorIdRequest request, CancellationToken cancellationToken)
    {
        Medico? medicoSelecionado = await repositorioMedico.SelecionarPorIdAsync(request.Id);

        if (medicoSelecionado is null)
        {
            return Result.Fail(ErrorResults.NotFoundError(request.Id));
        }

        SelecionarMedicoPorIdResponse resposta = new(
            medicoSelecionado.Id,
            medicoSelecionado.Nome,
            medicoSelecionado.Crm
        );

        return Result.Ok(resposta);
    }
}