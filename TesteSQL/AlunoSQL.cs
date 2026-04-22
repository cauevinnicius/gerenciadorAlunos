//Importação do pacote MySQL
using MySql.Data.MySqlClient;

class AlunoSQL
{
    // Teremos um atributo para armazenar a string de conexão. Na Conexao.cs há o método criado para este fim.
    // Readonly -> modificações possíveis apenas dentro do método construtor
    private readonly string? _stringDeConexao;

    // Método construtor para retornar o parâmetro de conexão
    public AlunoSQL(string conexao)
    {
        _stringDeConexao = conexao;
    }

    // Método para efetuar o cadastro
    public void Cadastrar(string nome, string cpf, string email, int celular)
    {
        // Comando SQL para inserirmos os nomes e cidades dos alunos
        string sql = "INSERT INTO alunos (nome, cpf, email, celular) VALUES (@nome, @cpf, @email, @celular)";

        // Conexão com o banco de dados e execução do comando SQL
        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            //Necessitamos especificar os parâmetros do SQL
            comando.Parameters.AddWithValue("@nome", nome);
            comando.Parameters.AddWithValue("@cpf", cpf);
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@celular", celular);

            // E, por fim, executar o comando SQL propriamente dito. É seguro usarmos try/catch
            try
            {
                // Abrir a conexao com o banco e executar o comando.
                // eu tive a ideia de inserir validações aqui. Mas fui mais afundo pesquisar e a ideia das classes SQL é serem, de certa forma, "mudas".
                conexao.Open();
                comando.ExecuteNonQuery();

            }
            catch (Exception excecao) //Catch trata exceções apenas. 
            {
                Console.WriteLine($"Falha ao cadastrar: {excecao.Message}");
            }
        }
    }

    // invés de um método vazio (void), vamos retornar uma lista pública que vou chamar de Aluno.
    // essa eu pedi ajuda pro Gemini.
    public List<Aluno> Listar() // método novo
    {
        List<Aluno> listaAlunos = new List<Aluno>();
        string sql = "SELECT * FROM alunos";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            try
            {
                conexao.Open();
                // Listagem de alunos
                using (var alunos = comando.ExecuteReader())
                {
                    // a HasRows eu não conhecia. É umafunção da biblioteca mysql. Legal trocar uma ideia sobre isso
                    if (alunos.HasRows)
                    {
                        while (alunos.Read())
                        {
                            //instanciando nossa classe Aluno em um objeto de nome alunoEncontrado
                            // Console.Clear(); // tava ficando muito sujo a execução
                            Aluno alunoEncontrado = new Aluno();
                            alunoEncontrado.Id = Convert.ToInt32(alunos["id"]); // pq int32? sugestão do gemini
                            alunoEncontrado.Nome = alunos["nome"].ToString();
                            alunoEncontrado.Cpf = alunos["cpf"].ToString();
                            alunoEncontrado.Email = alunos["email"].ToString();
                            alunoEncontrado.Celular = Convert.ToInt32(alunos["celular"]);

                            listaAlunos.Add(alunoEncontrado);

                        }

                    }
                    else
                    {
                        Console.WriteLine("Hmm.. não foram encontrados alunos cadastrados!");
                    }
                }
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }
        }

        //por fim, o retorno da listaAlunos
        return listaAlunos;
    }
    public List<Aluno> Selecionar(string parametroBusca)
    {
        List<Aluno> listaAlunos = new List<Aluno>();
        string sql = "SELECT * FROM alunos WHERE id = @id OR nome LIKE @nome";

        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            // só para eu não esquecer: tive a ideia de deixar algo mais dinamico, buscando tanto pelo nome ou pelo ID, por exemplo. Se for digitado um numero, então o usuário digitou um ID. Se não, o usuário digitou um nome.
            if (int.TryParse(parametroBusca, out int IdBusca))
            {
                comando.Parameters.AddWithValue("@id", IdBusca);
                comando.Parameters.AddWithValue("@nome", ""); // vazio pra nao dar pau
            }
            else
            {
                comando.Parameters.AddWithValue("@id", -1); // valor inválido para garantir que a parte do ID não retorne resultados
                comando.Parameters.AddWithValue("@nome", $"%{parametroBusca}%");
            }


            try
            {
                conexao.Open();
                // Listagem de alunos
                using (var alunos = comando.ExecuteReader())
                {

                    if (alunos.HasRows)
                    {
                        while (alunos.Read())
                        {
                            Aluno alunoEncontrado = new Aluno();
                            alunoEncontrado.Id = Convert.ToInt32(alunos["id"]);
                            alunoEncontrado.Nome = alunos["nome"].ToString();
                            alunoEncontrado.Cpf = alunos["cpf"].ToString();
                            alunoEncontrado.Email = alunos["email"].ToString();
                            alunoEncontrado.Celular = Convert.ToInt32(alunos["celular"]);

                            listaAlunos.Add(alunoEncontrado);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hmm.. não foram encontrados alunos(a) com esse ID ou nome");
                    }
                }
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao buscar: {excecao.Message}");
            }
        }

        return listaAlunos;
    }

    // Método para alterar o cadastro
    public void Alterar(string nome, string cpf, string email, int celular, int id)
    {

        string sql = "UPDATE alunos SET nome = @nome, cpf = @cpf, email = @email, celular = @celular WHERE id = @id";


        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@nome", nome);
            comando.Parameters.AddWithValue("@cpf", cpf);
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@celular", celular);
            comando.Parameters.AddWithValue("@id", id);


            try
            {
                conexao.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao alterar: {excecao.Message}");
            }
        }
    }

    // Método para deletar o cadastro
    public void Deletar(int id)
    {
        // Comando SQL para inserirmos os nomes e cidades dos alunos
        string sql = "DELETE FROM alunos WHERE id = @id";

        // Conexão com o banco de dados e execução do comando SQL
        using (var conexao = new MySqlConnection(_stringDeConexao))
        using (var comando = new MySqlCommand(sql, conexao))
        {
            //Necessitamos especificar os parâmetros do SQL
            comando.Parameters.AddWithValue("@id", id);

            // E, por fim, executar o comando SQL propriamente dito. É seguro usarmos try/catch
            try
            {
                // Abrir a conexao com o banco e executar o comando.
                conexao.Open();
                comando.ExecuteNonQuery();
                Console.WriteLine("Cadastro deletado com sucesso!");
            }
            catch (Exception excecao)
            {
                Console.WriteLine($"Falha ao deletar: {excecao.Message}");
            }
        }
    }
}