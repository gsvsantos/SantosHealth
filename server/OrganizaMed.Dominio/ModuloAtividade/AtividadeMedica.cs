using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloMedico;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Dominio.ModuloAtividade;

public enum TipoAtividadeMedica
{
    Consulta,
    Cirurgia
}

public abstract class AtividadeMedica : EntidadeBase
{
    public Guid PacienteId { get; set; }
    public Paciente? Paciente { get; set; }
    public DateTimeOffset Inicio { get; set; }
    public DateTimeOffset? Termino { get; set; }
    public bool ConfirmacaoEnviada { get; set; }
    public List<Medico> Medicos { get; set; }

    protected TipoAtividadeMedica tipoAtividade;
    public abstract TipoAtividadeMedica TipoAtividade { get; set; }

    protected AtividadeMedica() => this.Medicos = [];

    protected AtividadeMedica(DateTimeOffset inicio, DateTimeOffset? termino) : this()
    {
        this.Inicio = inicio.ToUniversalTime();
        this.Termino = termino?.ToUniversalTime();
    }

    public abstract TimeSpan ObterPeriodoDescanso();

    public void AdicionarMedico(Medico medicoParaAdicionar)
    {
        if (this.Medicos.Contains(medicoParaAdicionar))
        {
            return;
        }

        this.Medicos.Add(medicoParaAdicionar);

        medicoParaAdicionar.RegistrarAtividade(this);
    }

    public void RemoverMedico(Medico medicoParaRemover)
    {
        if (!this.Medicos.Contains(medicoParaRemover))
        {
            return;
        }

        this.Medicos.Remove(medicoParaRemover);

        medicoParaRemover.RemoverAtividade(this);
    }
}