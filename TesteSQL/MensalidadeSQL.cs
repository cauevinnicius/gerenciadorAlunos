using MySql.Data.MySqlClient;
class MensalidadeSQL

{
    private readonly string? _stringDeConexao;

    public MensalidadeSQL(string conexao)
    {
        _stringDeConexao = conexao;
    }

    public void LancarMensalidade(int aluno_id, double valor)
    {
        DateTime dataVencimento = DateTime.Now.AddDays(30);

        string sql = "INSERT INTO mensalidades (aluno_id, valor, data_vencimento) VALUES (@aluno_id, @valor, @data_vencimento)";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@aluno_id", aluno_id);
            comando.Parameters.AddWithValue("@valor", valor);
            comando.Parameters.AddWithValue("@data_vencimento", dataVencimento);

            try
            {
                conexao.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
            }
        }
    }
    // modificação de void para um bool -> true se deu certo, false se deu ruim.
    public bool RegistrarPagamento(int idMensalidade)
    {
        string sql = "UPDATE mensalidades SET data_pagamento = @data_pagamento, status = 'pago' WHERE id = @id";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@id", idMensalidade);
            comando.Parameters.AddWithValue("@data_pagamento", DateTime.Now);

            try
            {
                conexao.Open();
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0; // se afetou mais de 0 linhas, então é verdadeiro.
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao registrar pagamento: {excecao.Message}");
                return false; // se não afetou nada, retorna falso.
            }
        }
    }

    public List<Mensalidade> ListarMensalidades()
    {
        List<Mensalidade> listaMensalidades = new List<Mensalidade>();
        string sql = "SELECT * FROM mensalidades";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            try
            {
                conexao.Open();
                using (var mensalidades = comando.ExecuteReader())
                {
                    if (mensalidades.HasRows)
                    {
                        while (mensalidades.Read())
                        {
                            Mensalidade mensalidadeEncontrada = new Mensalidade();
                            mensalidadeEncontrada.Id = Convert.ToInt32(mensalidades["id"]);
                            mensalidadeEncontrada.AlunoId = Convert.ToInt32(mensalidades["aluno_id"]);
                            mensalidadeEncontrada.ValorMensalidade = Convert.ToDecimal(mensalidades["valor"]);
                            mensalidadeEncontrada.DataVencimento = Convert.ToDateTime(mensalidades["data_vencimento"]);
                            mensalidadeEncontrada.Status = mensalidades["status"].ToString();

                            listaMensalidades.Add(mensalidadeEncontrada);

                        }
                    }
                }
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }
        }
        return listaMensalidades;
    }

    public List<Mensalidade> VerificaPendencias(int aluno_id)
    {
        List<Mensalidade> listaMensalidades = new List<Mensalidade>();
        string sql = "SELECT * FROM mensalidades WHERE aluno_id = @aluno_id AND status = 'pendente'";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@aluno_id", aluno_id);

            try
            {
                conexao.Open();
                using (var mensalidades = comando.ExecuteReader())
                {

                    if (mensalidades.HasRows)
                    {
                        while (mensalidades.Read())
                        {
                            Mensalidade mensalidadeEncontrada = new Mensalidade();
                            mensalidadeEncontrada.Id = Convert.ToInt32(mensalidades["id"]);
                            mensalidadeEncontrada.AlunoId = Convert.ToInt32(mensalidades["aluno_id"]);
                            mensalidadeEncontrada.ValorMensalidade = Convert.ToDecimal(mensalidades["valor"]);
                            mensalidadeEncontrada.DataVencimento = Convert.ToDateTime(mensalidades["data_vencimento"]);
                            mensalidadeEncontrada.Status = mensalidades["status"].ToString();

                            listaMensalidades.Add(mensalidadeEncontrada);

                        }
                    }
                }
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }
        }
        return listaMensalidades;
    }

    public bool EditarMensalidade(decimal valor, DateTime dataVencimento, string status, int id) // nem sempre teremos uma nova data de pagamento
    {
        string sql = "UPDATE mensalidades SET valor = @valor, data_vencimento = @data_vencimento, status = @status, data_pagamento = @data_pagamento WHERE id = @id";


        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@valor", valor);
            comando.Parameters.AddWithValue("@data_vencimento", dataVencimento);
            comando.Parameters.AddWithValue("@status", status);
            //comando.Parameters.AddWithValue("@data_pagamento", dataPagamento);
            comando.Parameters.AddWithValue("@id", id);

            /* essa validação fica pra melhoria futura. Necessária revisão. Auxílio pro Gemini.
            if (dataPagamento.HasValue)
            {
                comando.Parameters.AddWithValue("@data_pagamento", dataPagamento.Value);
            }
            else
            {
                comando.Parameters.AddWithValue("@data_pagamento", DBNull.Value); // quando a mensal tá pendente, ainda não tem data de pagamento. ideia de permissão de "salvamento de espaço na memoria" para que o DB nao quebre 
            }*/

            try
            {
                conexao.Open();
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0; // retorna true se atualizou alguém
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao alterar no banco de dados: {excecao.Message}");
                return false;
            }
        }
    }
    public void ExcluirMensalidade(int id)
    {
        string sql = "DELETE FROM mensalidades WHERE id = @id";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@id", id);

            try
            {
                conexao.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }
        }
    }
}

