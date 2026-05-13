// meu molde de mensalidade
public class Mensalidade
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime DataPagamento { get; set; }

    private decimal _valorMensalidade;
    public decimal ValorMensalidade
    {
        get { return _valorMensalidade; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("O valor da mensalidade não pode ser negativo.");
            }
            _valorMensalidade = value;
        }
    }

    private string _status;
    public string Status
    {
        get { return _status; }
        set
        {
            string statusFormatado = value.ToLower().Trim();

            // o status TEM que ser um desses três por causa do enum feito no banco.
            if (statusFormatado != "pendente" && statusFormatado != "pago" && statusFormatado != "cancelado")
            {
                throw new ArgumentException("Status inválido. Use apenas: pendente, pago ou cancelado.");
            }

            _status = statusFormatado;
        }
    }
}