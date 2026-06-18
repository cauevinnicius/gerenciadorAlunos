using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GerenciadorAlunosV2.Contexts;
// Basicamente, aqui seria a minha nova classe de conexão. Agora será "Factory"
// Classe responsavel pela criacao do contexto de banco de dados 
public class GerenciadorAlunosDbContextFactory : IDesignTimeDbContextFactory<GerenciadorAlunosDbContext>
{
    public GerenciadorAlunosDbContext CreateDbContext(string[] args)
    {
        // Não deixo minhas configurações de conexão expostas, mas sim uma configuração para que vá até a raiz do projeto e leia meu appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // montagem do construtor do Entity Framework
        var builder = new DbContextOptionsBuilder<GerenciadorAlunosDbContext>();

        // montagem da string de conexão pelo nome que está disposto no JSON
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // utilização do MySQL 
        builder.UseMySQL(connectionString);

        // retorna uma nova instancia do contexto de banco de dados configurado
        return new GerenciadorAlunosDbContext(builder.Options);
    }
}
