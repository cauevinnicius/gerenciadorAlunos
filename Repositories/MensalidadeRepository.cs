using GerenciadorAlunos.Entities;
using GerenciadorAlunos.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorAlunos.Repositories;

public class MensalidadeRepository

{
    private readonly GerenciadorAlunosDbContext _context;

    public MensalidadeRepository(GerenciadorAlunosDbContext context)
    {
        _context = context;
    }

    public void LancarMensalidade(Mensalidade novaMensalidade)
    {
        try
        {
            novaMensalidade.DataVencimento = DateTime.Now.AddDays(30);
            _context.Mensalidades.Add(novaMensalidade);
            _context.SaveChanges();
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
        }
    }

    // modificação de void para um bool -> true se deu certo, false se deu ruim.
    public bool RegistrarPagamento(int idMensalidade, DateTime dataPagamento)
    {
        var mensalidade = _context.Mensalidades.Find(idMensalidade);

        if (mensalidade != null)
        {
            mensalidade.Status = "pago";
            mensalidade.DataPagamento = dataPagamento;

            _context.SaveChanges();
            return true;
        }

        return false;
    }
    public List<Mensalidade> ListarMensalidades()
    {
        return _context.Mensalidades.ToList();
    }
    public List<Mensalidade> VerificaPendencias(int alunoId)
    {
        return _context.Mensalidades
            .Where(m => m.AlunoId == alunoId && m.Status == "pendente")
            .ToList();
    }
    public bool EditarMensalidade(Mensalidade mensalidadeEditada)
    {
        try
        {
            _context.Mensalidades.Update(mensalidadeEditada);
            return _context.SaveChanges() > 0; // salvar apenas se for maior que zero
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Falha ao alterar no banco de dados: {excecao.Message}");
            return false;
        }
    }

    public void ExcluirMensalidade(int id)
    {
       var mensalidade = _context.Mensalidades.Find(id);
        if (mensalidade != null)
        {
            _context.Mensalidades.Remove(mensalidade);
            _context.SaveChanges();
        }
    }
}