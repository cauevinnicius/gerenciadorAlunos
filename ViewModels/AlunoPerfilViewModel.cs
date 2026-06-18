using GerenciadorAlunosV2.Models;
namespace GerenciadorAlunosV2.ViewModels;

public class AlunoPerfilViewModel
{
    public int IdAluno { get; set; }
    public string NomeAluno { get; set; }
    public string CpfAluno { get; set; }  
    public string EmailAluno { get; set; }
    public string CelularAluno { get; set; }
    public DateTime DataNascimentoAluno { get; set; }
    public DateTime DataCadastroAluno { get; set; }
    public List<MensalidadeModel> HistoricoMensalidades { get; set; }
}