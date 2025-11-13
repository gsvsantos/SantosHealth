namespace OrganizaMed.Aplicacao.EmailSender.DTOs;

public class EmailDto
{
    public string Para { get; set; } = null!;
    public string Assunto { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
}
public enum TipoEmailEnum
{
    NovaAgenda,
    Reagenda,
    Lembrete,
    Cancelamento
}