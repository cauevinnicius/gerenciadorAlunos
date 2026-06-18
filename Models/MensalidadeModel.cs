// meu molde de mensalidade no DB
namespace GerenciadorAlunosV2.Models;
public class MensalidadeModel
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public DateTime DataVencimento { get; set; }
    // necessária a inclusão do ?, significando que é possível ser nula a data de pagamento.
    public DateTime? DataPagamento { get; set; }
    public decimal ValorMensalidade { get; set; }
    public string Status { get; set; }    
}