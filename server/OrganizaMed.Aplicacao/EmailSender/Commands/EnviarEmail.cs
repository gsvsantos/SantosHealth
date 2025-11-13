using Hangfire;
using OrganizaMed.Aplicacao.EmailSender.DTOs;
using OrganizaMed.Aplicacao.EmailSender.Helpers;
using OrganizaMed.Dominio.ModuloAtividade;

namespace OrganizaMed.Aplicacao.EmailSender.Commands;

public class EnviarEmail(IEmailSender emailSender)
{
    public async Task Execute(AtividadeMedica atividade, TipoEmailEnum tipoEmail)
    {
        await Task.Delay(1000);

        string[] medicos = atividade.Medicos.Select(m => m.Nome).ToArray();

        EmailDto? email = EmailHelper.TipoEmail(tipoEmail.ToString(), atividade, medicos)
            ?? throw new Exception("Houve um erro ao gerar o email");

        BackgroundJob.Enqueue(() => emailSender.EnviarEmailAsync(email));

        string jobId = $"Lembrete-{atividade.Id}";

        if (tipoEmail == TipoEmailEnum.NovaAgenda || tipoEmail == TipoEmailEnum.Reagenda)
        {
            EmailDto? emailLembrete = EmailHelper.TipoEmail("Lembrete", atividade, medicos);

            RecurringJob.AddOrUpdate(
                jobId,
                () => AgendarLembrete(jobId, atividade.Inicio.Date, emailLembrete),
                Cron.Daily(6, 0));
        }
        else if (tipoEmail == TipoEmailEnum.Cancelamento)
        {
            RecurringJob.RemoveIfExists(jobId);
        }
    }

    public void AgendarLembrete(string jobId, DateTime dataAtividade, EmailDto? email)
    {
        DateTime hoje = DateTime.UtcNow.Date;

        if (dataAtividade == hoje.AddDays(1))
        {
            if (email != null)
            {
                emailSender.EnviarEmailAsync(email);
            }

            RecurringJob.RemoveIfExists(jobId);
        }
        else if (dataAtividade <= hoje)
        {
            RecurringJob.RemoveIfExists(jobId);
        }
    }
}