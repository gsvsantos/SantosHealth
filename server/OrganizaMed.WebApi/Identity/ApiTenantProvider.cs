using OrganizaMed.Dominio.ModuloAutenticacao;
using System.Security.Claims;

namespace OrganizaMed.WebApi.Identity;

public class ApiTenantProvider(IHttpContextAccessor contextAccessor) : ITenantProvider
{
    public Guid? UsuarioId
    {
        get
        {
            Claim? claimId = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claimId == null)
            {
                return null;
            }

            return Guid.Parse(claimId.Value);
        }
    }
}