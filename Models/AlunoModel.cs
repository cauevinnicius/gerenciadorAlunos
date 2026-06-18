// meu molde de aluno no DB
namespace GerenciadorAlunosV2.Models;
public class AlunoModel
{
    // Realizada a refatoração. Farei o usufruto das ViewModels para não expor minhas
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }  
    public string Email { get; set; }
    public string Celular { get; set; }
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;

}