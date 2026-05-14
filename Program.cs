using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using GerenciadorAlunos.Contexts;
using GerenciadorAlunos.Repositories;
using GerenciadorAlunos.UI;

namespace GerenciadorAlunos;


class Program
{
    static void Main(string[] args)
    {
        // Primeira coisa que preciso fazer agora é ler meu appsetings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        // depois preciso configurar o pacote de configurações do EF
        var optionsBuilder = new DbContextOptionsBuilder<GerenciadorAlunosDbContext>();
        optionsBuilder.UseMySQL(connectionString);

       using (var context = new GerenciadorAlunosDbContext(optionsBuilder.Options))
        {
            // faço a instancia dos novos repositórios e incluo o context
            AlunoRepository alunoRep = new AlunoRepository(context);
            MensalidadeRepository mensalidadeRep = new MensalidadeRepository(context);

             // por fim, instancio meu menu principal e incluo os dois repositorios
            MenuPrincipal menu = new MenuPrincipal(alunoRep, mensalidadeRep);

            menu.ExibirMenu();
        }
    }
}