using Microsoft.EntityFrameworkCore;
using GerenciadorAlunos.Entities;
// Através dos Contexts eu posso criar e/ou gerenciar as minhas tabelas do meu DB
namespace GerenciadorAlunos.Contexts;

public class GerenciadorAlunosDbContext : DbContext
{
    // Dúvida: a DbSet está referenciando a minha classe molde Aluno.cs e a Mensalidade.cs e dando os nomes de "Alunos" e "Mensalidades"?
    // R: O DbSet está sim referenciando os moldes, pois representa a tabela como um todo. A "Alunos" seria a variável que será utilizada para acessar a referida tabela.
    // Dúvida: Por que aqui temos novos get e sets? Nas minhas moldes já possuo as regras dos gets e sets criadas. Como fica?
    // R: Esses get e set seriam pertencentes à tabela, e não aos campos do Aluno e Mensalidade. As regras criadas para as duas ficam à salvo e respeitadas pelo EF.
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Mensalidade> Mensalidades { get; set; }

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
        // Mapeamento da tabela Alunos
        modelBuilder.Entity<Aluno>().ToTable("alunos");
        modelBuilder.Entity<Aluno>().Property(a => a.Id).HasColumnName("id");
        modelBuilder.Entity<Aluno>().Property(a => a.Nome).HasColumnName("nome");
        modelBuilder.Entity<Aluno>().Property(a => a.Cpf).HasColumnName("cpf");
        modelBuilder.Entity<Aluno>().Property(a => a.Email).HasColumnName("email");
        modelBuilder.Entity<Aluno>().Property(a => a.Celular).HasColumnName("celular");

        // Mapeamento da tabela Mensalidades
        modelBuilder.Entity<Mensalidade>().ToTable("mensalidades");
        modelBuilder.Entity<Mensalidade>().Property(m => m.Id).HasColumnName("id");
        modelBuilder.Entity<Mensalidade>().Property(m => m.AlunoId).HasColumnName("aluno_id");
        modelBuilder.Entity<Mensalidade>().Property(m => m.ValorMensalidade).HasColumnName("valor");
        modelBuilder.Entity<Mensalidade>().Property(m => m.DataVencimento).HasColumnName("data_vencimento");
        modelBuilder.Entity<Mensalidade>().Property(m => m.Status).HasColumnName("status");
        modelBuilder.Entity<Mensalidade>().Property(m => m.DataPagamento).HasColumnName("data_pagamento");
    }
}
