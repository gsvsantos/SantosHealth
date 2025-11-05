using Microsoft.AspNetCore.Identity;

namespace OrganizaMed.Dominio.ModuloAutenticacao;

public class Usuario : IdentityUser<Guid>
{
    public Usuario()
    {
        this.Id = Guid.NewGuid();
        this.EmailConfirmed = true;
    }
}