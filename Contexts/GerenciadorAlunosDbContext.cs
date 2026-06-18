using Microsoft.EntityFrameworkCore;
using GerenciadorAlunosV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// Através dos Contexts eu posso criar e/ou gerenciar as minhas tabelas do meu DB
namespace GerenciadorAlunosV2.Contexts;

// com o Identity, eu passei a herdar de IdentityDbContext
public class GerenciadorAlunosDbContext : IdentityDbContext <IdentityUser>
{
    // Dúvida: a DbSet está referenciando a minha classe molde Aluno.cs e a Mensalidade.cs e dando os nomes de "Alunos" e "Mensalidades"?
    // R: O DbSet está sim referenciando os moldes, pois representa a tabela como um todo. A "Alunos" seria a variável que será utilizada para acessar a referida tabela.
    // Dúvida: Por que aqui temos novos get e sets? Nas minhas moldes já possuo as regras dos gets e sets criadas. Como fica?
    // R: Esses get e set seriam pertencentes à tabela, e não aos campos do Aluno e Mensalidade. As regras criadas para as duas ficam à salvo e respeitadas pelo EF.
    public DbSet<AlunoModel> Alunos { get; set; }
    public DbSet<MensalidadeModel> Mensalidades { get; set; }
    public DbSet<UsuarioModel> Usuarios { get; set; }
    
    // Dúvida: seria um construtor padrão?
    // R: Sim. A classe GerenciadorAlunosContext herda da classe DbContext. O DbContextOptions é o pacote de configurações que contém a minha string de conexão com o MySQL.
    // E o que é o : base(options)?
    // R: Basicamente significa que estou herdando as opções da classe pai "base" (DbContext, neste caso).
    public GerenciadorAlunosDbContext(DbContextOptions<GerenciadorAlunosDbContext> options) : base(options) { }

    // Dúvida: seria uma sobrescrição de um método do próprio entity framework? por que? 
    // R: Sim. A classe pai (DbContext) já possui um método chamado OnModelCreating, o qual é originalmente vazio ou que faz apenas o mapeamento básico. 
    // Como minhas tabelas no DB estão em snake_case, preciso dizer pro EF o que é o que. 
    // O ModelBuilder é a ferramenta que será utilizada para dar ordens sobre a estrutura do banco.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // aqui ta a cirurgia q tive que pedi ajuda pra IA pra fazer. Lembrando que o MYSQL não tem uma cadeia de textos nativamente (string[])
        base.OnModelCreating(modelBuilder);

        // vou sair procurando por colunas do tipo "string[]"
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var propriedadesArray = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string[]));

            foreach (var propriedade in propriedadesArray)
            {
                // transformar o Array ["A", "B"] na string "A;B" para salvar no banco
                propriedade.SetValueConverter(
                    new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<string[], string>(
                        array => string.Join(";", array),
                        texto => texto.Split(";", System.StringSplitOptions.RemoveEmptyEntries)));
            }
        }
        // Mapeamento da tabela Alunos
        modelBuilder.Entity<AlunoModel>().ToTable("alunos");
        modelBuilder.Entity<AlunoModel>().Property(a => a.Id).HasColumnName("id");
        modelBuilder.Entity<AlunoModel>().Property(a => a.Nome).HasColumnName("nome");
        modelBuilder.Entity<AlunoModel>().Property(a => a.Cpf).HasColumnName("cpf");
        modelBuilder.Entity<AlunoModel>().Property(a => a.Email).HasColumnName("email");
        modelBuilder.Entity<AlunoModel>().Property(a => a.Celular).HasColumnName("celular");

        // Mapeamento da tabela Mensalidades
        modelBuilder.Entity<MensalidadeModel>().ToTable("mensalidades");
        modelBuilder.Entity<MensalidadeModel>().Property(m => m.Id).HasColumnName("id");
        modelBuilder.Entity<MensalidadeModel>().Property(m => m.AlunoId).HasColumnName("aluno_id");
        modelBuilder.Entity<MensalidadeModel>().Property(m => m.ValorMensalidade).HasColumnName("valor");
        modelBuilder.Entity<MensalidadeModel>().Property(m => m.DataVencimento).HasColumnName("data_vencimento");
        modelBuilder.Entity<MensalidadeModel>().Property(m => m.Status).HasColumnName("status");
        modelBuilder.Entity<MensalidadeModel>().Property(m => m.DataPagamento).HasColumnName("data_pagamento");

    }
}
