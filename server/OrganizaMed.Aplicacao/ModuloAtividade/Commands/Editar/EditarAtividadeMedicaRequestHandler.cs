using FluentResults;
using FluentValidation;
using MediatR;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.EmailSender.Commands;
using OrganizaMed.Aplicacao.EmailSender.DTOs;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Aplicacao.ModuloAtividade.Commands.Editar;

public class EditarAtividadeMedicaRequestHandler(
    IRepositorioAtividadeMedica repositorioAtividadeMedica,
    IRepositorioMedico repositorioMedico,
    IContextoPersistencia contexto,
    IValidator<AtividadeMedica> validador,
    IEmailSender emailSender
) : IRequestHandler<EditarAtividadeMedicaRequest, Result<EditarAtividadeMedicaResponse>>
{
    public async Task<Result<EditarAtividadeMedicaResponse>> Handle(
        EditarAtividadeMedicaRequest request, CancellationToken cancellationToken)
    {
        AtividadeMedica? atividadeSelecionada = await repositorioAtividadeMedica.SelecionarPorIdAsync(request.Id);

        if (atividadeSelecionada is null)
        {
            return Result.Fail(ErrorResults.NotFoundError(request.Id));
        }

        atividadeSelecionada.Inicio = request.Inicio;
        atividadeSelecionada.Termino = request.Termino;
        atividadeSelecionada.Medicos = await repositorioMedico.SelecionarMuitosPorId(request.Medicos);

        FluentValidation.Results.ValidationResult resultadoValidacao =
            await validador.ValidateAsync(atividadeSelecionada, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            List<string> erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        try
        {
            await repositorioAtividadeMedica.EditarAsync(atividadeSelecionada);

            await contexto.GravarAsync();

            AdicionarAtividade adicionarAtividade = new(emailSender);
            await adicionarAtividade.Execute(atividadeSelecionada, TipoEmailEnum.Reagenda);
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarAtividadeMedicaResponse(atividadeSelecionada.Id));
    }
}