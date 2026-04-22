using MySql.Data.MySqlClient;
class MensalidadeSQL

{
    private readonly string? _stringDeConexao;

    public MensalidadeSQL(string conexao)
    {
       _stringDeConexao = conexao; 
    }

    // aqui eu sempre obrigo a inserir o id do aluno e o valor. Mas e se eu quisesse alternativa?
    public void LancarMensalidade(int aluno_id, double valor)
    {
        DateTime dataVencimento = DateTime.Now.AddDays(30);
        // duvida: tenho que dar um select da minha alunos para buscar o id? Busquei diretao, vamo ver se dá certo.
        string sql = "INSERT INTO mensalidades (aluno_id, valor, data_vencimento) VALUES (@aluno_id, @valor, @data_vencimento)";

        using(var conexao = new MySqlConnection(_stringDeConexao))
        using(var comando = new MySqlCommand(sql, conexao))
        {
              comando.Parameters.AddWithValue("@aluno_id", aluno_id);
              comando.Parameters.AddWithValue("@valor", valor);
              comando.Parameters.AddWithValue("@data_vencimento", dataVencimento);
        
            try
            {
                conexao.Open();
                comando.ExecuteNonQuery();
                Console.WriteLine($"Valor inserido com sucesso! Vencimento: {dataVencimento}");
            }
            catch(Exception excecao) 
            {
                Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
            }
        }
    }

    public void RegistrarPagamento(int id_mensalidade)
    {
        string sql = "UPDATE mensalidades SET data_pagamento = @data_pagamento, status = 'pago' WHERE id = @id";

        using(var conexao = new MySqlConnection(_stringDeConexao))
        using(var comando = new MySqlCommand(sql, conexao))
        {
              comando.Parameters.AddWithValue("@id", id_mensalidade);
              comando.Parameters.AddWithValue("@data_pagamento", DateTime.Now);
        
            try
            {
                conexao.Open();
                // peguei uma ideia na internet que achei interessante para ter uma "prova real" se houve linhas afetadas ou não.
                int linhasAfetadas = comando.ExecuteNonQuery();
                if (linhasAfetadas > 0)
                {
                    Console.WriteLine("Pagamento registrado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Nenhuma linha foi afetada. Verifique o ID da mensalidade.");
                }
            }
            catch(Exception excecao) 
            {
                Console.WriteLine($"Falha ao registrar pagamento: {excecao.Message}");
            }
        }
    }            

    public void SelecionarMensalidadeAluno(int aluno_id) 
    {
        string sql = "SELECT * FROM mensalidades WHERE aluno_id = @aluno_id";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@aluno_id", aluno_id);
            
            try
            {
                conexao.Open();
                // Listagem de alunos
                using (var mensalidades = comando.ExecuteReader())
                {
                    // a HasRows eu não conhecia. É umafunção da biblioteca mysql. Perguntar o que seria isso
                    if(mensalidades.HasRows)
                    {
                        while(mensalidades.Read())
                        {
                            Console.WriteLine("\n === Mensalidade encontrada ===");
                            Console.WriteLine("ID: " + mensalidades["id"]);
                            Console.WriteLine("Valor: " + mensalidades["valor"]);
                            Console.WriteLine("Data de Vencimento: " + mensalidades["data_vencimento"]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hmm.. não foram encontradas mensalidades com esse ID");
                    }    
                }
            }
            catch(Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }    
        }
    }
    
    public void VerificaPendencias(int aluno_id)
    {
        string sql = "SELECT * FROM mensalidades WHERE aluno_id = @aluno_id AND status = 'pendente'";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@aluno_id", aluno_id);
            
            try
            {
                conexao.Open();
                // Listagem de alunos
                using (var mensalidades = comando.ExecuteReader())
                {
                    // a HasRows eu não conhecia. É umafunção da biblioteca mysql. Perguntar o que seria isso
                    if(mensalidades.HasRows)
                    {
                        Console.WriteLine("O aluno possui as seguintes mensalidades pendentes:");
                        while(mensalidades.Read())
                        {
                            Console.WriteLine("\n === Mensalidade pendente ===");
                            Console.WriteLine("ID: " + mensalidades["id"]);
                            Console.WriteLine("Valor: " + mensalidades["valor"]);
                            Console.WriteLine("Data de Vencimento: " + mensalidades["data_vencimento"]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O aluno não possui mensalidades pendentes.");
                    }    
                }
            }
            catch(Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }    
        }
    }
}

