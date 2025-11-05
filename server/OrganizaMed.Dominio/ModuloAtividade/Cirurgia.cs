using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Dominio.ModuloAtividade;

public class Cirurgia : AtividadeMedica
{
    public override TipoAtividadeMedica TipoAtividade
    {
        get => TipoAtividadeMedica.Cirurgia;
        set => this.tipoAtividade = value;
    }

    protected Cirurgia() { }

    public Cirurgia(DateTime inicio, DateTime? termino) : base(inicio, termino)
    {
    }

    public Cirurgia(DateTime inicio, DateTime? termino, List<Medico> medicos) : base(inicio, termino)
    {
        foreach (Medico medico in medicos)
        {
            this.Medicos.Add(medico);
            medico.RegistrarAtividade(this);
        }
    }

    public override TimeSpan ObterPeriodoDescanso() => TimeSpan.FromHours(4);
}