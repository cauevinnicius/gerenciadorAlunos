// meu molde de mensalidade
public class Mensalidade
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public decimal ValorMensalidade { get; set; }
    public DateTime DataVencimento { get; set; }
    public string Status { get; set; }
    public DateTime DataPagamento { get; set; }
}