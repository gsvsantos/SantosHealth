using OrganizaMed.Dominio.ModuloAutenticacao;

namespace OrganizaMed.Dominio.Compartilhado;

public abstract class EntidadeBase
{
    public Guid Id { get; set; }

    protected EntidadeBase() => this.Id = Guid.NewGuid();

    public Guid UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}