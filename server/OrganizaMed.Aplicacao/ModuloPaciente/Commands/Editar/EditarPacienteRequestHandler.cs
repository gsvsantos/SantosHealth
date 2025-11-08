using FluentResults;
using FluentValidation;
using MediatR;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Aplicacao.ModuloPaciente.Commands.Editar;

public class EditarPacienteRequestHandler(
    IRepositorioPaciente repositorioPaciente,
    IContextoPersistencia contexto,
    IValidator<Paciente> validador
) : IRequestHandler<EditarPacienteRequest, Result<EditarPacienteResponse>>
{
    public async Task<Result<EditarPacienteResponse>> Handle(EditarPacienteRequest request, CancellationToken cancellationToken)
    {
        Paciente? pacienteSelecionado = await repositorioPaciente.SelecionarPorIdAsync(request.Id);

        if (pacienteSelecionado == null)
        {
            return Result.Fail(ErrorResults.NotFoundError(request.Id));
        }

        FluentValidation.Results.ValidationResult resultadoValidacao =
            await validador.ValidateAsync(pacienteSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            List<string> erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        List<Paciente> pacientes = await repositorioPaciente.SelecionarTodosAsync();

        if (CpfDuplicado(request.Cpf, pacientes))
        {
            return Result.Fail(PacienteErrorResults.CpfDuplicadoError(request.Cpf));
        }

        pacienteSelecionado.Nome = request.Nome;
        pacienteSelecionado.Cpf = request.Cpf;
        pacienteSelecionado.Email = request.Email;
        pacienteSelecionado.Telefone = request.Telefone;

        try
        {
            await repositorioPaciente.EditarAsync(pacienteSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarPacienteResponse(pacienteSelecionado.Id));
    }

    private bool CpfDuplicado(string cPF, IEnumerable<Paciente> pacientes)
    {
        return pacientes
            .Any(registro => string.Equals(
                registro.Cpf,
                cPF,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}