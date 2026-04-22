// Importacao do pacote MySQL, permitindo-se seu uso
using MySql.Data.MySqlClient;

public class Conexao
{
    // Precisamos realizar a criação de uma string de conexão com intuito de estabelecer a comunicação com o DB
    private readonly string _stringDeConexao;

    // Definição das credenciais de acesso do DB. A ideia não seria ter essas informações nem em uma classe, mas sim numa variável de ambiente ou arquivo de configuração não mapeado
    private const string _servidor = "localhost";
    private const string _base = "base";
    private const string _usuario = "root";
    private const string _senha = "12345678";

    // Precisamos criar um método construtor para que a conexao ssempre seja efetuada
    public Conexao()
    {
        _stringDeConexao = $"Server={_servidor};Database={_base};User={_usuario};Password={_senha};";
    }

    // Método para retornar a função string de conexão
    public string RetornaStringConexao()
    {
        return _stringDeConexao;
    }

    //  Método para testar a conexão
    public void TestarConexao()
    {
        try
        {
            using (var connection = new MySqlConnection(_stringDeConexao))
            {
                connection.Open();
                Console.WriteLine("Conexão efetuada com sucesso!");
            }
        }
        catch (Exception excecao)
        {
            Console.WriteLine("Falha ao conectar: " + excecao.Message);
        }
    }

}
