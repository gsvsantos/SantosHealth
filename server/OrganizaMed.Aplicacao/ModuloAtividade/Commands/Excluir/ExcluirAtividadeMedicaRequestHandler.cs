using FluentResults;
using MediatR;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.EmailSender.Commands;
using OrganizaMed.Aplicacao.EmailSender.DTOs;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAtividade;

namespace OrganizaMed.Aplicacao.ModuloAtividade.Commands.Excluir;

public class ExcluirAtividadeMedicaRequestHandler(
    IRepositorioAtividadeMedica repositorioAtividadeMedica,
    IContextoPersistencia contexto,
    EnviarEmail enviarEmail
) : IRequestHandler<ExcluirAtividadeMedicaRequest, Result<ExcluirAtividadeMedicaResponse>>
{
    public async Task<Result<ExcluirAtividadeMedicaResponse>> Handle(
        ExcluirAtividadeMedicaRequest request, CancellationToken cancellationToken)
    {
        AtividadeMedica? atividadeSelecionada = await repositorioAtividadeMedica.SelecionarPorIdAsync(request.Id);

        if (atividadeSelecionada is null)
        {
            return Result.Fail(ErrorResults.NotFoundError(request.Id));
        }

        try
        {
            await repositorioAtividadeMedica.ExcluirAsync(atividadeSelecionada);

            await contexto.GravarAsync();

            if (EhAtividadeValidaParaEmail(atividadeSelecionada))
            {
                await enviarEmail.Execute(atividadeSelecionada, TipoEmailEnum.Cancelamento);
            }
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirAtividadeMedicaResponse());
    }

    private static bool EhAtividadeValidaParaEmail(AtividadeMedica atividade) =>
        atividade.Termino.HasValue && atividade.Termino.Value.Date >= DateTime.UtcNow.Date;
}