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
    // A partir de agora, com os async/await, temos uma Task ao invés de um void, por exemplo.
    // pelo padrão, os nomes tem a inclusão do "Async"
    public async Task CadastrarAsync(Aluno novoAluno)
    {
        try
        {
            _context.Alunos.Add(novoAluno); // adição do nosso objeto novoAluno na memória
            // dai no SaveChanges eu incluo um await 
            await _context.SaveChangesAsync(); // o próprio EF montar o insert sozinho e executa :O
        }
        catch (Exception excecao) //Catch trata exceções apenas. 
        {
            Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
        }
    }

    // Perguntar pro Sérgio: pesquisando na internet, vi que não seria uma boa prática inserir try/catch em todos. Para métodos de leitura (listar, selecionar, verificarpendencias) não faz sentido, mas tão somente para escritas (cadatro, alterar e deletar)
    public async Task<List<Aluno>> ListarAsync()
    {
        return await _context.Alunos.ToListAsync(); // literalmente 46 linhas se transformaram em 4.
    }

    public async Task<List<Aluno>> SelecionarAsync(string parametroBusca)
    {
        // só para eu não esquecer: tive a ideia de deixar algo mais dinamico, buscando tanto pelo nome ou pelo ID, por exemplo. Se for digitado um numero, então o usuário digitou um ID. Se não, o usuário digitou um nome.
        if (int.TryParse(parametroBusca, out int idBusca))
        {
            return await _context.Alunos.Where(a => a.Id == idBusca).ToListAsync();
        }
        else
        {
            // eu não precisaria necessariamente abrir esse else. Fiz apenas para organizar melhor
            return await _context.Alunos.Where(a => a.Nome.Contains(parametroBusca)).ToListAsync();
        }
    }

    // Método para alterar o cadastro. Agora também só passo o Aluno e o objeto alunoEditado
   public async Task AlterarAsync(Aluno alunoEditado)
    {
        try
        {
            // O EF busca o aluno, vê o que mudou e faz o UPDATE apenas do necessário
            _context.Alunos.Update(alunoEditado);
            await _context.SaveChangesAsync();
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Erro ao alterar: {excecao.Message}");
        }
    }

    // Método para deletar o cadastro
    public async Task DeletarAsync(int id)
    {
        try
        {
            var aluno = _context.Alunos.Find(id); // Busca o aluno pelo ID
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Erro ao deletar: {excecao.Message}");
        }
    }
}