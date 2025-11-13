using Hangfire;
using OrganizaMed.Aplicacao.EmailSender.DTOs;
using OrganizaMed.Dominio.ModuloAtividade;

namespace OrganizaMed.Aplicacao.EmailSender.Commands;

public class AdicionarAtividade(IEmailSender emailSender)
{
    public async Task Execute(AtividadeMedica atividade, TipoEmailEnum tipoEmail)
    {
        await Task.Delay(1000);

        string[] medicos = atividade.Medicos.Select(m => m.Nome).ToArray();

        EmailDto? email = TipoEmail(tipoEmail.ToString(), atividade, medicos)
            ?? throw new Exception("Houve um erro interno ao enviar o email");

        BackgroundJob.Enqueue(() => emailSender.EnviarEmailAsync(email));
    }

    private static EmailDto? TipoEmail(string tipo, AtividadeMedica atividade, string[] medicos)
    {
        return tipo switch
        {
            "NovaAgenda" =>
                new()
                {
                    Para = atividade.Paciente!.Email,
                    Assunto = $"Confirmação de Agendamento - Santos Health",
                    Conteudo = $"Olá, {atividade.Paciente.Nome}!<br/>Sua {atividade.TipoAtividade} foi agendada com sucesso.<br/><br/>🗓 Data: {atividade.Inicio.ToShortDateString()}" +
                        $"<br/>⏰ Horário: {atividade.Inicio.ToShortTimeString()}<br/>👩‍⚕️ Médico(s): {string.Join(", ", medicos)}<br/>📍 Local: lá mesmo<br/>" +
                        $"<br/>Em caso de dúvidas, entre em contato pelo telefone (51) 99661-6244.<br/><br/>Atenciosamente,<br/>Equipe Santos Health"
                },
            "Reagenda" =>
                new()
                {
                    Para = atividade.Paciente!.Email,
                    Assunto = $"Confirmação de Reagendamento - Santos Health",
                    Conteudo = $"Olá, {atividade.Paciente.Nome}!<br/>Sua {atividade.TipoAtividade} foi reagendada com sucesso.<br/><br/>🗓 Data: {atividade.Inicio.ToShortDateString()}" +
                        $"<br/>⏰ Horário: {atividade.Inicio.ToShortTimeString()}<br/>👩‍⚕️ Médico(s): {string.Join(", ", medicos)}<br/>📍 Local: lá mesmo<br/>" +
                        $"<br/>Em caso de dúvidas, entre em contato pelo telefone (51) 99661-6244.<br/><br/>Atenciosamente,<br/>Equipe Santos Health"
                },
            "Lembrete" =>
                new()
                {
                    Para = atividade.Paciente!.Email,
                    Assunto = $"Lembrete: Sua {atividade.TipoAtividade} é amanhã as {atividade.Inicio.ToShortTimeString()} - Santos Health",
                    Conteudo = $"Olá, {atividade.Paciente.Nome}!<br/>Lembramos que você possui uma {atividade.TipoAtividade} agendada para amanhã.<br/><br/>🗓 Data: {atividade.Inicio.ToShortDateString()}" +
                        $"<br/>⏰ Horário: {atividade.Inicio.ToShortTimeString()}<br/>👩‍⚕️ Médico(s): {string.Join(", ", medicos)}<br/>📍 Local: lá mesmo<br/>" +
                        $"<br/>Por favor, chegue com 15 minutos de antecedência.<br/><br/>Atenciosamente,<br/>Equipe Santos Health" +
                        $"<br/>Se precisar reagendar, entre em contato pelo telefone (51) 99661-6244.<br/><br/>Atenciosamente,<br/>Equipe Santos Health"
                },
            "Cancelamento" =>
                new()
                {
                    Para = atividade.Paciente!.Email,
                    Assunto = $"Cancelamento de Agendamento - Santos Health",
                    Conteudo = $"Olá, {atividade.Paciente.Nome}!<br/>Informamos que sua {atividade.TipoAtividade} foi cancelada.<br/>" +
                        $"<br/>Dados do agendamento cancelado:<br/>🗓 Data: {atividade.Inicio.ToShortDateString()}" +
                        $"<br/>⏰ Horário: {atividade.Inicio.ToShortTimeString()}<br/>👩‍⚕️ Médico(s): {string.Join(", ", medicos)}<br/>📍 Local: lá mesmo<br/>" +
                        $"<br/>Para marcar um novo horário, entre em contato pelo telefone (51) 99661-6244.<br/><br/>Atenciosamente,<br/>Equipe Santos Health"
                },
            _ => null
        };
    }
}