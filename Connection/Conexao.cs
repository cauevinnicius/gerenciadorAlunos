using System;
using System.IO;
// Importacao do pacote MySQL, permitindo-se seu uso
using MySql.Data.MySqlClient;
// NOVA IMPLEMENTACAO: importação da Microsoft Extensions Configuration
using Microsoft.Extensions.Configuration;
public class Conexao
{
    // Precisamos realizar a criação de uma string de conexão com intuito de estabelecer a comunicação com o DB
    private readonly string _stringDeConexao;

    // NOVO: Retirada das credenciais de acesso do DB. 
    
    // Precisamos criar um método construtor para que a conexao sempre seja efetuada
    public Conexao()
    {
        //criacao do leitor de arquivos de configuração. Uma observação que encontrei: isso numa aplicação Web já vem "nativamente". Por ser app de console, temos que fornecer os caminhos.
        var builder = new ConfigurationBuilder() // a configuração dessa forma é sempre assim? Resposta após pesquisas: sim - builder pattern. Construtor vazio que vamos "colando" peças nele
            .SetBasePath(Directory.GetCurrentDirectory()) // pega o diretório atual onde o programa está rodando
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // realiza a leitura o nosso arquivo

        IConfiguration config = builder.Build();

        _stringDeConexao = config.GetConnectionString("ConexaoPadrao");
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
