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

    public async Task LancarMensalidadeAsync(Mensalidade novaMensalidade)
    {
        try
        {
            novaMensalidade.DataVencimento = DateTime.Now.AddDays(30);
            _context.Mensalidades.Add(novaMensalidade);
            await _context.SaveChangesAsync();
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
        }
    }

    public async Task RegistrarPagamentoAsync(int idMensalidade, DateTime dataPagamento)
    {
        try
        {
            var mensalidade = await _context.Mensalidades.FindAsync(idMensalidade);

            if (mensalidade != null)
            {
                mensalidade.Status = "pago";
                mensalidade.DataPagamento = dataPagamento;

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Falha ao registrar: {excecao}");
        }
    }
    public async Task<List<Mensalidade>> ListarMensalidadesAsync()
    {
        return await _context.Mensalidades.ToListAsync();
    }

    // a escrita de retorno está dessa forma para fins de organização. De fato, seria posto tudo em uma única linha
    public async Task<List<Mensalidade>> VerificaPendenciasAsync(int alunoId)
    {
        return await _context.Mensalidades
            .Where(m => m.AlunoId == alunoId && m.Status == "pendente")
            .ToListAsync();
    }

    public async Task<bool> EditarMensalidadeAsync(Mensalidade mensalidadeEditada)
    {
        try
        {
            _context.Mensalidades.Update(mensalidadeEditada);
            return await _context.SaveChangesAsync() > 0; // salvar apenas se for maior que zero
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Falha ao alterar no banco de dados: {excecao.Message}");
            return false;
        }
    }

    public async Task ExcluirMensalidadeAsync(int id)
    {
        try
        {
            var mensalidade = await _context.Mensalidades.FindAsync(id);
            if (mensalidade != null)
            {
                _context.Mensalidades.Remove(mensalidade);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception excecao)
        {
            Console.WriteLine($"Falha ao alterar no banco de dados: {excecao.Message}");
        }
    }
}