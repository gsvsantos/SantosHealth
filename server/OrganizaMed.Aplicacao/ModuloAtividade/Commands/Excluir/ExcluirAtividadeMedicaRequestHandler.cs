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
    IEmailSender emailSender
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

            if (!VerificarSeEhAntiga(atividadeSelecionada))
            {
                AdicionarAtividade adicionarAtividade = new(emailSender);
                await adicionarAtividade.Execute(atividadeSelecionada, TipoEmailEnum.Cancelamento);
            }
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirAtividadeMedicaResponse());
    }

    private static bool VerificarSeEhAntiga(AtividadeMedica atividade) => atividade.Termino < DateTime.UtcNow;
}