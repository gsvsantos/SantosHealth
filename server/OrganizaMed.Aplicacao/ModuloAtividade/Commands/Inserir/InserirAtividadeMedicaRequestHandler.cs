using FluentResults;
using FluentValidation;
using MediatR;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.EmailSender.Commands;
using OrganizaMed.Aplicacao.EmailSender.DTOs;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.Dominio.ModuloAutenticacao;
using OrganizaMed.Dominio.ModuloMedico;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Aplicacao.ModuloAtividade.Commands.Inserir;

public class InserirAtividadeMedicaRequestHandler(
    IRepositorioAtividadeMedica repositorioAtividadeMedica,
    IRepositorioMedico repositorioMedico,
    IRepositorioPaciente repositorioPaciente,
    IContextoPersistencia contexto,
    ITenantProvider tenantProvider,
    IValidator<AtividadeMedica> validador,
    IEmailSender emailSender
) : IRequestHandler<InserirAtividadeMedicaRequest, Result<InserirAtividadeMedicaResponse>>
{
    public async Task<Result<InserirAtividadeMedicaResponse>> Handle(
        InserirAtividadeMedicaRequest request, CancellationToken cancellationToken)
    {
        List<Medico> medicosSelecionados = await repositorioMedico.SelecionarMuitosPorId(request.Medicos);

        if (medicosSelecionados.Count == 0)
        {
            return Result.Fail(AtividadeMedicaErrorResults.MedicosNaoEncontradosError());
        }

        Paciente? pacienteSelecionado = await repositorioPaciente.SelecionarPorIdAsync(request.PacienteId);

        if (pacienteSelecionado is null)
        {
            return Result.Fail(AtividadeMedicaErrorResults.PacienteNaoEncontradoError());
        }

        AtividadeMedica atividade;

        if (request.TipoAtividade == TipoAtividadeMedica.Consulta)
        {
            atividade = new Consulta(
                inicio: request.Inicio,
                termino: request.Termino,
                medico: medicosSelecionados.First()
            );
        }
        else
        {
            atividade = new Cirurgia(
                inicio: request.Inicio,
                termino: request.Termino,
                medicos: medicosSelecionados
            );
        }

        atividade.PacienteId = pacienteSelecionado.Id;
        atividade.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

        FluentValidation.Results.ValidationResult resultadoValidacao =
            await validador.ValidateAsync(atividade, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            List<string> erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        try
        {
            await repositorioAtividadeMedica.InserirAsync(atividade);

            await contexto.GravarAsync();

            AdicionarAtividade adicionarAtividade = new(emailSender);
            await adicionarAtividade.Execute(atividade, TipoEmailEnum.NovaAgenda);
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new InserirAtividadeMedicaResponse(atividade.Id));
    }
}