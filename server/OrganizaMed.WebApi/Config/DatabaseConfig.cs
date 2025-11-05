using OrganizaMed.Infraestrutura.Orm.Compartilhado;

namespace OrganizaMed.WebApi.Config;

public static class DatabaseConfig
{
    public static bool AutoMigrateDatabase(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        OrganizaMedDbContext dbContext = scope.ServiceProvider.GetRequiredService<OrganizaMedDbContext>();

        bool migracaoConcluida = MigradorBancoDados.AtualizarBancoDados(dbContext);

        return migracaoConcluida;
    }
}