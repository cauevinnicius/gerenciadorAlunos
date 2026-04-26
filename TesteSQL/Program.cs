class Program
{

    static void Main(string[] args)
    {
        // Preciso instanciar a conexão com meu db
        Conexao conexaoSQL = new Conexao();
        string _stringDeConexao = conexaoSQL.RetornaStringConexao();

        // e preciso instanciar minha classe AlunoSQL + MensalidadeSQL + Menu 
        AlunoSQL aluno = new AlunoSQL(_stringDeConexao);
        MensalidadeSQL mensalidade = new MensalidadeSQL(_stringDeConexao);
        Menu menu = new Menu(aluno, mensalidade);

        menu.ExibirMenu();
    }
}




