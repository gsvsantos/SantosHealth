using Microsoft.EntityFrameworkCore;
using OrganizaMed.Dominio.Compartilhado;

namespace OrganizaMed.Infraestrutura.Orm.Compartilhado;

public class RepositorioBase<TEntidade> where TEntidade : EntidadeBase
{
    protected readonly IContextoPersistencia context;
    protected readonly DbSet<TEntidade> registros;

    public RepositorioBase(IContextoPersistencia context)
    {
        this.context = context;
        this.registros = ((DbContext)this.context).Set<TEntidade>();
    }

    public async Task<Guid> InserirAsync(TEntidade registro)
    {
        await this.registros.AddAsync(registro);

        return registro.Id;
    }

    public async Task<bool> EditarAsync(TEntidade registro)
    {
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntidade> rastreador = this.registros.Update(registro);

        return await Task.Run(() => rastreador.State == EntityState.Modified);
    }

    public async Task<bool> ExcluirAsync(TEntidade registro)
    {
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntidade> rastreador = this.registros.Remove(registro);

        return await Task.Run(() => rastreador.State == EntityState.Deleted);
    }

    public virtual async Task<List<TEntidade>> SelecionarTodosAsync() => await this.registros.ToListAsync();

    public virtual async Task<TEntidade?> SelecionarPorIdAsync(Guid id) => await this.registros.SingleOrDefaultAsync(x => x.Id == id);
}