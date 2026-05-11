// meu molde de aluno
// a ideia de dispor propriedades nos gets e sets seria dispor regras para ser possível ler ou inserir informações nas variáveis e de que maneira.
// com isso, seria feito uma variavel privada e uma publica. A publica seria tipo um "porteiro" e a privada a "balada"/objetivo (backing field). 
// Guard Clause: validar tudo primeiro. Se der erro, aborte a missão. Se houver sucesso em todos os testes, conclua a missão com sucesso.
public class Aluno
{
    // não seria necessário aplicar regras ao ID neste caso.
    public int Id { get; set; }

    private string _nome;
    public string Nome
    {
        get
        {
            return _nome;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("O nome do aluno não pode ser vazio ou nulo.");
            }
            _nome = value;
        }
    }

    private string _cpf;
    public string Cpf
    {
        get
        {
            return _cpf;
        }
        set
        {
            // passei a ideia das validações que antes ocorriam lá no meu MenuAluno.cs pra cá.
            string cpfLimpo = value.Replace(".", "").Replace("-", "");

            if (cpfLimpo.Length != 11)
            {
                throw new ArgumentException("O CPF deverá conter exatamente 11 números.");
            }

            // eu não preciso colocar um else, pois a sentença acima já interrompe o erro. Se o código passar do if, é pq o CPF já é válido

            string cpfFormatado = cpfLimpo.Insert(3, ".").Insert(7, ".").Insert(11, "-");
            _cpf = cpfFormatado;
        }
    } 
    
    public string Email { get; set; }

    private string _celular;
    public string Celular
    {
        get
        {
            return _celular;
        }
        set
        {
            string celularLimpo = value.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");

            if (celularLimpo.Length != 11)
            {
                throw new ArgumentException("O número de celular deve conter exatamente 11 dígitos (DDD + 9 números).");
            }
            
            string ddd = celularLimpo.Substring(0, 2); // pega os 2 primeiros
            string primeiraParte = celularLimpo.Substring(2, 5); // pega os 5 próximos digitos
            string segundaParte = celularLimpo.Substring(7, 4);  // pega os 4 últimos digitos

            string celularFormatado = $"({ddd}) {primeiraParte}-{segundaParte}";

            _celular = celularFormatado;
        }
    }
}