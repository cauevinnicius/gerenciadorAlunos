using GerenciadorAlunosV2.Models;
namespace GerenciadorAlunosV2.ViewModels;

public class MensalidadePerfilViewModel
{
    public int IdMensalidade { get; set; }
    public int IdAluno { get; set; }
    public string NomeAluno { get; set; }
    public string CpfAluno { get; set; }
    public decimal ValorMensalidade { get; set; }
    public DateTime DataVencimento { get; set; }
    
}