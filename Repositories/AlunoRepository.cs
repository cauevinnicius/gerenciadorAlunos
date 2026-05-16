//Importação do pacote MySQL
using GerenciadorAlunos.Contexts;
using GerenciadorAlunos.Entities;
using Microsoft.EntityFrameworkCore;
namespace GerenciadorAlunos.Repositories;

public class AlunoRepository
{
    // Teremos um atributo para armazenar, agora, meu context, no qual possui as configurações de conexão.
    // Readonly -> modificações possíveis apenas dentro do método construtor
    private readonly GerenciadorAlunosDbContext _context;

    // Método construtor para retornar o parâmetro de conexão
    public AlunoRepository(GerenciadorAlunosDbContext context)
    {
        _context = context;
    }

    // Método para efetuar o cadastro. Agora eu posso simplesmente chamar meu molde Aluno e criar um objeto novoAluno (WTF)
    // Perguntar pro Sérgio: Pensei em por um bool, retornando true após o savechanges e false no catch
    public void Cadastrar(Aluno novoAluno)
    {
        try
        {
            _context.Alunos.Add(novoAluno); // adição do nosso objeto novoAluno na memória
            _context.SaveChanges(); // o próprio EF montar o insert sozinho e executa :O
        }
        catch (Exception excecao) //Catch trata exceções apenas. 
        {
            Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
        }
    }

    // Perguntar pro Sérgio: pesquisando na internet, vi que não seria uma boa prática inserir try/catch em TODOS. Para métodos de leitura (listar, selecionar, verificarpendencias) não faz sentido, mas tão somente para escritas (cadatro, alterar e deletar)
    public List<Aluno> Listar()
    {
        return _context.Alunos.ToList(); // literalmente 46 linhas se transformaram em 4.
    }

    public List<Aluno> Selecionar(string parametroBusca)
    {
        // só para eu não esquecer: tive a ideia de deixar algo mais dinamico, buscando tanto pelo nome ou pelo ID, por exemplo. Se for digitado um numero, então o usuário digitou um ID. Se não, o usuário digitou um nome.
        if (int.TryParse(parametroBusca, out int idBusca))
        {
            return _context.Alunos.Where(a => a.Id == idBusca).ToList();
        }
        else
        {
            // eu não precisaria necessariamente abrir esse else. Fiz apenas para organizar melhor
            return _context.Alunos.Where(a => a.Nome.Contains(parametroBusca)).ToList();
        }
    }

    // Método para alterar o cadastro. Agora também só passo o Aluno e o objeto alunoEditado
   public void Alterar(Aluno alunoEditado)
    {
        try
        {
            // O EF busca o aluno, vê o que mudou e faz o UPDATE apenas do necessário
            _context.Alunos.Update(alunoEditado);
            _context.SaveChanges();
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Erro ao alterar: {excecao.Message}");
        }
    }

    // Método para deletar o cadastro
    public void Deletar(int id)
    {
        var aluno = _context.Alunos.Find(id); // Busca o aluno pelo ID
        if (aluno != null)
        {
            _context.Alunos.Remove(aluno);
            _context.SaveChanges();
        }
    }
}